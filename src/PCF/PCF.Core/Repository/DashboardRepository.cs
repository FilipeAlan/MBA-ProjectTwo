using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Dtos.Dashboard;
using PCF.Core.Enumerables;
using PCF.Core.Interface;

namespace PCF.Core.Repository
{
    public class DashboardRepository
        (PCFDBContext dbContext) : IDashboardRepository
    {
        public async Task<DashboardTransacoesPorCategoriaResponse> ObterTransacoesAgrupadasPorCategoriaAsync(DashboardTransacoesPorCategoriaRequest request, int usuarioId)
        {
            var lastDay = DateTime.DaysInMonth(request.Periodo.Year, request.Periodo.Month);

            var result = await dbContext.Transacoes.Where(t => t.Tipo == request.Tipo &&
                                                               t.UsuarioId == usuarioId &&
                                                               t.DataLancamento >= new DateTime(request.Periodo.Year, request.Periodo.Month, 1) &&
                                                               t.DataLancamento <= new DateTime(request.Periodo.Year, request.Periodo.Month, lastDay))
                                                   .GroupBy(t => t.CategoriaId)
                                                   .Select(t => new
                                                   {
                                                       CategoriaId = t.Key,
                                                       Valor = t.Sum(t => t.Valor)
                                                   })
                                                   .Join(dbContext.Categorias,
                                                       grp => grp.CategoriaId,
                                                       categoria => categoria.Id,
                                                       (grp, categoria) => new DashboardTransacaoAgrupadaPorCategoria(categoria.Nome, grp.Valor))
                                                   .AsNoTracking()
                                                   .ToListAsync();

            return new DashboardTransacoesPorCategoriaResponse(result.OrderByDescending(t => t.Valor));
        }

        public async Task<DashboardHistoricoMensalResponse> ObterHistoricoMensalAsync(int usuarioId)
        {
            const int totalMonths = 12;
            var lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var firstDay = lastDay.AddMonths(-totalMonths);

            var result = await dbContext.Transacoes.Where(t => t.UsuarioId == usuarioId &&
                                                               t.DataLancamento >= firstDay &&
                                                               t.DataLancamento <= lastDay)
                                                   .GroupBy(t => new
                                                   {
                                                       Tipo = t.Tipo,
                                                       Mes = t.DataLancamento.Month,
                                                       Ano = t.DataLancamento.Year
                                                   })
                                                   .Select(t => new
                                                   {
                                                       Tipo = t.Key.Tipo,
                                                       Periodo = new DateOnly(t.Key.Ano, t.Key.Mes, 1),
                                                       Valor = t.Sum(t => t.Valor)
                                                   })
                                                   .AsNoTracking()
                                                   .ToListAsync();

            var items = new List<DashboardHistoricoMensal>();

            for (int i = 0; i <= totalMonths; i++)
            {
                var periodo = new DateOnly(firstDay.Year, firstDay.Month, 1).AddMonths(i);
                var grp = result.Where(r => r.Periodo == periodo);

                items.Add(
                    new DashboardHistoricoMensal(
                        Entradas: grp.FirstOrDefault(x => x.Tipo == TipoEnum.Entrada)?.Valor ?? 0,
                        Saidas: grp.FirstOrDefault(x => x.Tipo == TipoEnum.Saida)?.Valor ?? 0,
                        Periodo: periodo)
                    );
            }

            return new DashboardHistoricoMensalResponse(items);
        }

        public async Task<DashboardSummary> ObterResumoAsync(DashboardSummaryRequest request, int usuarioId)
        {
            var lastDay = DateTime.DaysInMonth(request.Periodo.Year, request.Periodo.Month);

            var entradas = await dbContext.Transacoes.Where(t => t.Tipo == TipoEnum.Entrada &&
                                                                 t.UsuarioId == usuarioId &&
                                                                 t.DataLancamento >= new DateTime(request.Periodo.Year, request.Periodo.Month, 1) &&
                                                                 t.DataLancamento <= new DateTime(request.Periodo.Year, request.Periodo.Month, lastDay))
                                                     .SumAsync(t => t.Valor);

            var saidas = await dbContext.Transacoes.Where(t => t.Tipo == TipoEnum.Saida &&
                                                                t.UsuarioId == usuarioId &&
                                                                t.DataLancamento >= new DateTime(request.Periodo.Year, request.Periodo.Month, 1) &&
                                                                t.DataLancamento <= new DateTime(request.Periodo.Year, request.Periodo.Month, lastDay))
                                                    .SumAsync(t => t.Valor);

            return new DashboardSummary(entradas, saidas);
        }
    }
}