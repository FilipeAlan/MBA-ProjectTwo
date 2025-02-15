using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Globalization;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class TransacaoService(IAppIdentityUser appIdentityUser, ITransacaoRepository repository, 
        IOrcamentoRepository orcamentoRepository, ICategoriaRepository categoriaRepository) : ITransacaoService
    {
        private string retorno;

        public async Task<Result<int>> AddAsync(Transacao Transacao)
        {
            ArgumentNullException.ThrowIfNull(Transacao);


            Transacao.UsuarioId = appIdentityUser.GetUserId();

            if (!Transacao.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }

            await ValidaTransacao(Transacao, Transacao.Valor);

            await repository.CreateAsync(Transacao);
            return Result.Ok(Transacao.Id);
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

        public async Task<Result> UpdateAsync(Transacao Transacao)
        {
            ArgumentNullException.ThrowIfNull(Transacao);

            var TransacaoExistente = await GetByIdAsync(Transacao.Id);

            if (TransacaoExistente is null)
            {
                return Result.Fail("Transação não encontrada");
            }

            if (!Transacao.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }

            TransacaoExistente.Descricao = Transacao.Descricao;
            TransacaoExistente.Valor = Transacao.Valor;
            TransacaoExistente.Tipo = Transacao.Tipo;

            decimal valorTransacao = Transacao.Valor - TransacaoExistente.Valor;
            
            await ValidaTransacao(TransacaoExistente, valorTransacao);

            await repository.UpdateAsync(TransacaoExistente);

            return Result.Ok();
        }

        private async Task ValidaTransacao(Transacao TransacaoExistente, decimal valorMovimentacao)
        {
            if (TransacaoExistente.CategoriaId != null)
            {
                decimal totalUtilizadoCategoriaMes = 0;
                decimal totalEntradasMes = await repository.CheckTotalBudgetCurrentMonthAsync(appIdentityUser.GetUserId(), DateTime.Now);
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
                            retorno =
                                $"O total de entradas {FormatoMoeda.ParaReal(totalUtilizadoCategoriaMes)} já ultrapassa a meta de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoCategoria)} da categoria {categoria.Nome} " +
                                $"saldo no mês corrente.";
                        }
                    }
                    else if (valorOrcamentoGeral > 0)
                    {
                        if (totalUtilizadoCategoriaMes + valorMovimentacao > valorOrcamentoGeral)
                        {
                            retorno =
                                $"O total de entradas {FormatoMoeda.ParaReal(totalUtilizadoCategoriaMes)} já ultrapassa a meta de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoGeral)} para o orçamento geral mês corrente.";
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
                            retorno =
                                $"O total de gastos {FormatoMoeda.ParaReal(totalUtilizadoCategoriaMes)} ultrapassa o orçamento de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoCategoria)} da categoria {categoria.Nome} " +
                                $"saldo no mês corrente.";
                        }
                    }
                    else if (valorOrcamentoGeral > 0)
                    {
                        if (totalUtilizadoMes + valorMovimentacao > valorOrcamentoGeral)
                        {
                            retorno =
                                $"O total de gastos {FormatoMoeda.ParaReal(totalUtilizadoMes)} ultrapassa o orçamento de " +
                                $"{FormatoMoeda.ParaReal(valorOrcamentoGeral)} da categoria {categoria.Nome} " +
                                $"saldo no mês corrente.";
                        }
                    }
                }
            }
        }
    }
}