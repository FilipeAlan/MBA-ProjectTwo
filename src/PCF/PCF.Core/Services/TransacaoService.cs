using Microsoft.AspNetCore.Http.HttpResults;
using PCF.Core.Dtos;
using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Globalization;
using PCF.Core.Interface;
using System.Text;

namespace PCF.Core.Services
{
    public class TransacaoService(IAppIdentityUser appIdentityUser, ITransacaoRepository repository,
        IOrcamentoRepository orcamentoRepository, ICategoriaRepository categoriaRepository) : ITransacaoService
    {
        public async Task<Result<GlobalResult>> AddAsync(Transacao transacao)
        {
            ArgumentNullException.ThrowIfNull(transacao);

            transacao.UsuarioId = appIdentityUser.GetUserId();

            if (!transacao.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }

            var result = await ValidaTransacaoV2(transacao, transacao.Valor);

            await repository.CreateAsync(transacao);
            return Result.Ok(new GlobalResult(default, string.Join(", ", result.Errors.Select(e => e.Message))));
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var Transacao = await GetByIdAsync(id);

            if (Transacao is null)
            {
                return Result.Fail("Transacao inexistente");
            }

            await repository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<IEnumerable<Transacao>> GetAllAsync()
        {
            return await repository.GetAllAsync(appIdentityUser.GetUserId());
        }

        public async Task<IEnumerable<Transacao>> GetAllByCategoriaAsync(int categoriaId)
        {
            return await repository.GetAllByCategoriaAsync(appIdentityUser.GetUserId(), categoriaId);
        }

        public async Task<IEnumerable<Transacao>> GetAllByPeriodoAsync(DateTime dataInicio, DateTime? dataFin)
        {
            return await repository.GetAllByPeriodoAsync(appIdentityUser.GetUserId(), dataInicio, dataFin);
        }

        public async Task<IEnumerable<Transacao>> GetAllByTipoAsync(TipoEnum tipo)
        {
            return await repository.GetAllByTipoAsync(appIdentityUser.GetUserId(), tipo);
        }

        public async Task<Transacao?> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id, appIdentityUser.GetUserId());
        }

        public async Task<Result<GlobalResult>> UpdateAsync(Transacao transacao)
        {
            ArgumentNullException.ThrowIfNull(transacao);

            var TransacaoExistente = await GetByIdAsync(transacao.Id);

            if (TransacaoExistente is null)
            {
                return Result.Fail("Transação não encontrada");
            }

            if (!transacao.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }

            decimal valorTransacao = transacao.Valor - TransacaoExistente.Valor;

            TransacaoExistente.Descricao = transacao.Descricao;
            TransacaoExistente.Valor = transacao.Valor;
            TransacaoExistente.Tipo = transacao.Tipo;
            TransacaoExistente.DataLancamento = transacao.DataLancamento;

            var result = await ValidaTransacaoV2(TransacaoExistente, valorTransacao);

            await repository.UpdateAsync(TransacaoExistente);

            return Result.Ok(new GlobalResult(default, string.Join(", ", result.Errors.Select(e => e.Message))));
        }

        private async Task<Result> ValidaTransacaoV2(Transacao TransacaoExistente, decimal valorMovimentacao)
        {
            if (TransacaoExistente.CategoriaId == default) return Result.Fail("A transação deve ter sempre uma categoria ");
            if (TransacaoExistente.Tipo == 0) return Result.Ok();

            var usuarioId = TransacaoExistente.UsuarioId;

            var categoria = await categoriaRepository.GetGeralByIdAsync(TransacaoExistente.CategoriaId); // retornava null quando a categoria não e padrão.
            if (categoria == null)  categoria = await categoriaRepository.GetByIdAsync(TransacaoExistente.CategoriaId, usuarioId);
            
            if (categoria == null) return Result.Fail($"Categoria com ID {TransacaoExistente.CategoriaId} não encontrada.");


            var orcamentoCategoria = await orcamentoRepository.GetByCategoriaAsync(TransacaoExistente.CategoriaId, usuarioId);
            var orcamentoGeral = await orcamentoRepository.GetGeralAsync(usuarioId);

            var valorOrcamentoCategoria = orcamentoCategoria?.ValorLimite ?? 0;
            var valorOrcamentoGeral = orcamentoGeral?.ValorLimite ?? 0;
            
            var valorReceitaMes = await repository.CheckTotalBudgetCurrentMonthAsync(usuarioId, DateTime.Now);
            var totalUtilizadoCategoriaMes = await repository.CheckAmountUsedByCategoriaCurrentMonthAsync(usuarioId, DateTime.Now, TransacaoExistente.CategoriaId);
            var totalUtilizadoMes = await repository.CheckAmountUsedCurrentMonthAsync(usuarioId, DateTime.Now);

            return ValidarOrcamento(totalUtilizadoMes, totalUtilizadoCategoriaMes, valorMovimentacao, valorOrcamentoCategoria, valorOrcamentoGeral, valorReceitaMes,categoria.Nome);

        }

        private static Result ValidarOrcamento(decimal totalUtilizado,decimal totalGastoCategoriaMes, decimal valorMovimentacao, decimal valorOrcamentoCategoria, decimal valorOrcamentoGeral, decimal valorEntradas, string nomeCategoria)
        {
            var totalComMovimentacao = totalUtilizado + valorMovimentacao;
            var totalCategoriaComMovimentacao = totalGastoCategoriaMes + valorMovimentacao;
            var mensagem = new StringBuilder();

            if (valorOrcamentoCategoria > 0 && totalCategoriaComMovimentacao > valorOrcamentoCategoria)
            {
                mensagem.Append($"O total de gastos {FormatoMoeda.ParaReal(totalComMovimentacao)} ultrapassa o orçamento de {FormatoMoeda.ParaReal(valorOrcamentoCategoria)} da categoria {nomeCategoria} no mês corrente.\n");
            }

            if (valorOrcamentoGeral > 0 && totalComMovimentacao > valorOrcamentoGeral)
            {
                mensagem.Append($"O total de gastos {FormatoMoeda.ParaReal(totalComMovimentacao)} ultrapassa o orçamento geral de {FormatoMoeda.ParaReal(valorOrcamentoGeral)} no mês corrente.\n");
            }
            if (valorEntradas < totalComMovimentacao)
            {
                mensagem.Append($"O total de gastos {FormatoMoeda.ParaReal(totalComMovimentacao)} ultrapassa o total de receitas {FormatoMoeda.ParaReal(valorEntradas)} no mês corrente.\n");
            }

            if (mensagem.Length > 0)  return Result.Fail(mensagem.ToString());

            return Result.Ok();
        }
    }
}