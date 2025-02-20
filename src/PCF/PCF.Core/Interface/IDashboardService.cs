using PCF.Core.Dtos.Dashboard;

namespace PCF.Core.Interface
{
    public interface IDashboardService
    {
        Task<DashboardHistoricoMensalResponse> ObterHistoricoMensalAsync();

        Task<DashboardSummary> ObterResumoAsync(DashboardSummaryRequest request);

        Task<DashboardTransacoesPorCategoriaResponse> ObterTransacoesAgrupadasPorCategoriaAsync(DashboardTransacoesPorCategoriaRequest request);
    }
}