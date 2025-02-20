using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos.Dashboard;
using PCF.Core.Interface;

namespace PCF.API.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController(IDashboardService dashboardService) : ApiControllerBase
    {
        [HttpGet("historico-mensal")]
        public async Task<Ok<DashboardHistoricoMensalResponse>> ObterHistoricoMensalAsync()
        {
            return TypedResults.Ok(await dashboardService.ObterHistoricoMensalAsync());
        }

        [HttpGet("transacoes-por-categoria")]
        public async Task<Ok<DashboardTransacoesPorCategoriaResponse>> ObterTransacoesAgrupadasPorCategoriaAsync([FromQuery] DashboardTransacoesPorCategoriaRequest request)
        {
            return TypedResults.Ok(await dashboardService.ObterTransacoesAgrupadasPorCategoriaAsync(request));
        }

        [HttpGet("resumo")]
        public async Task<Ok<DashboardSummary>> ObterResumoAsync([FromQuery] DashboardSummaryRequest request)
        {
            return TypedResults.Ok(await dashboardService.ObterResumoAsync(request));
        }
    }
}