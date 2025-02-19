using PCF.Core.Dtos.Dashboard;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class DashboardService
        (IDashboardRepository dashboardRepository, IAppIdentityUser appIdentityUser) : IDashboardService
    {
        public async Task<DashboardHistoricoMensalResponse> ObterHistoricoMensalAsync()
        {
            return await dashboardRepository.ObterHistoricoMensalAsync(appIdentityUser.GetUserId());
        }

        public async Task<DashboardSummary> ObterResumoAsync(DashboardSummaryRequest request)
        {
            return await dashboardRepository.ObterResumoAsync(request, appIdentityUser.GetUserId());
        }

        public async Task<DashboardTransacoesPorCategoriaResponse> ObterTransacoesAgrupadasPorCategoriaAsync(DashboardTransacoesPorCategoriaRequest request)
        {
            return await dashboardRepository.ObterTransacoesAgrupadasPorCategoriaAsync(request, appIdentityUser.GetUserId());
        }
    }
}