using PCF.Core.Dtos;
using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Globalization;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class TransacaoService(IAppIdentityUser appIdentityUser, ITransacaoRepository repository,
        IOrcamentoRepository orcamentoRepository, ICategoriaRepository categoriaRepository) : ITransacaoService
    {
        public async Task<Result<TransacaoResult>> AddAsync(Transacao transacao)
        {
            ArgumentNullException.ThrowIfNull(transacao);

            transacao.UsuarioId = appIdentityUser.GetUserId();

            if (!transacao.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }

            var result = await ValidaTransacao(transacao, transacao.Valor);

            await repository.CreateAsync(transacao);
            return Result.Ok(new TransacaoResult(default, string.Join(", ", result.Errors.Select(e => e.Message))));
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

        public async Task<Result<TransacaoResult>> UpdateAsync(Transacao transacao)
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

            TransacaoExistente.Descricao = transacao.Descricao;
            TransacaoExistente.Valor = transacao.Valor;
            TransacaoExistente.Tipo = transacao.Tipo;

            decimal valorTransacao = transacao.Valor - TransacaoExistente.Valor;

            var result = await ValidaTransacao(TransacaoExistente, valorTransacao);

            await repository.UpdateAsync(TransacaoExistente);

            return Result.Ok(new TransacaoResult(default, string.Join(", ", result.Errors.Select(e => e.Message))));
        }

        private async Task<Result> ValidaTransacao(Transacao TransacaoExistente, decimal valorMovimentacao)
        {
            if (TransacaoExistente.CategoriaId != default)
            {
                decimal totalUtilizadoCategoriaMes = 0;
                decimal totalUtilizadoMes = await repository.CheckAmountUsedCurrentMonthAsync(appIdentityUser.GetUserId(), DateTime.Now);

                var categoria = await categoriaRepository.GetGeralByIdAsync(TransacaoExistente.CategoriaId);
                var orcamentoCategoria = await orcamentoRepository.GetByCategoriaAsync(TransacaoExistente.CategoriaId, appIdentityUser.GetUserId());
                var orcamentoGeral = await orcamentoRepository.GetGeralAsync(appIdentityUser.GetUserId());

                decimal valorOrcamentoCategoria = orcamentoCategoria?.ValorLimite ?? 0;
                decimal valorOrcamentoGeral = orcamentoGeral?.ValorLimite ?? 0;

                totalUtilizadoCategoriaMes = await repository.CheckAmountUsedByCategoriaCurrentMonthAsync(appIdentityUser.GetUserId(), DateTime.Now,
                    TransacaoExistente.CategoriaId);

                //Caso entradas
                if (TransacaoExistente.Tipo == 0)
                {
                    if (valorOrcamentoCategoria > 0)
                    {
                        if (totalUtilizadoCategoriaMes + valorMovimentacao > valorOrcamentoCategoria)
                        {
                            return Result.Fail(
                                $"O total de entradas {FormatoMoeda.ParaReal(totalUtilizadoCategoriaMes)} já ultrapassa a meta de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoCategoria)} da categoria {categoria!.Nome} " +
                                $"saldo no mês corrente.");
                        }
                    }
                    else if (valorOrcamentoGeral > 0)
                    {
                        if (totalUtilizadoCategoriaMes + valorMovimentacao > valorOrcamentoGeral)
                        {
                            return Result.Fail(
                                $"O total de entradas {FormatoMoeda.ParaReal(totalUtilizadoCategoriaMes)} já ultrapassa a meta de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoGeral)} para o orçamento geral mês corrente.");
                        }
                    }
                }
                //Caso saídas
                else
                {
                    if (valorOrcamentoCategoria > 0)
                    {
                        if (totalUtilizadoCategoriaMes + valorMovimentacao > valorOrcamentoCategoria)
                        {
                            return Result.Fail(
                                $"O total de gastos {FormatoMoeda.ParaReal(totalUtilizadoCategoriaMes)} ultrapassa o orçamento de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoCategoria)} da categoria {categoria!.Nome} " +
                                $"saldo no mês corrente.");
                        }
                    }
                    else if (valorOrcamentoGeral > 0)
                    {
                        if (totalUtilizadoMes + valorMovimentacao > valorOrcamentoGeral)
                        {
                            return Result.Fail(
                                $"O total de gastos {FormatoMoeda.ParaReal(totalUtilizadoMes)} ultrapassa o orçamento de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoGeral)} da categoria {categoria!.Nome} " +
                                $"saldo no mês corrente.");
                        }
                    }
                }
            }

            return Result.Ok();
        }
    }
}