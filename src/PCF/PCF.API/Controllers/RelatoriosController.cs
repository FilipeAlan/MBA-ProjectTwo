using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos.Relatorio;
using PCF.Core.Interface;

namespace PCF.API.Controllers;

[Route("api/[controller]")]
public class RelatoriosController(IRelatorioService relatorioService) : ApiControllerBase
{


    [HttpGet]
    [Route("OrcamentoRealizado")]
    public async Task<Ok<IEnumerable<RelatorioOrcamentoResponse>>> GetOrcamentoRealizado(DateTime dataIncial,DateTime dataFinal)
    {
        var list = await relatorioService.GetOrcamentoRealizado(dataIncial,dataFinal);

        return TypedResults.Ok(list.Adapt<IEnumerable<RelatorioOrcamentoResponse>>());
    }

    [HttpGet]
    [Route("GastoPorCategoria")]
    public async Task<Ok<IEnumerable<RelatorioGastoPorCategoriaResponse>>> GetGastoPorCategoria(DateTime dataIncial, DateTime dataFinal)
    {
        var list = await relatorioService.GetGastoPorCategoria(dataIncial, dataFinal);

        return TypedResults.Ok(list.Adapt<IEnumerable<RelatorioGastoPorCategoriaResponse>>());
    }
}
