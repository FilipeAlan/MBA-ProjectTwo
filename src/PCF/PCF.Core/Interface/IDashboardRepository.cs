using PCF.Core.Dtos;

namespace PCF.Core.Interface
{
    public interface IDashboardRepository
    {
        Task<DashboardHistoricoMensalResponse> ObterHistoricoMensalAsync(int usuarioId);

        Task<DashboardSummary> ObterResumoAsync(DashboardSummaryRequest request, int usuarioId);

        Task<DashboardTransacoesPorCategoriaResponse> ObterTransacoesAgrupadasPorCategoriaAsync(DashboardTransacoesPorCategoriaRequest request, int usuarioId);
    }
}