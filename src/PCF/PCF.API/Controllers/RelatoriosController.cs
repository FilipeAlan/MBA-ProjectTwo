using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos;
using PCF.Core.Interface;

namespace PCF.API.Controllers;

[Route("api/relatorios")]
public class RelatoriosController(IRelatorioService relatorioService) : ApiControllerBase
{


    [HttpGet]
    public async Task<Ok<IEnumerable<RelatorioOrcamentoResponse>>> GetOrcamentoRealizado(DateTime dataIncial,DateTime dataFinal)
    {
        var list = await relatorioService.GetOrcamentoRealizado(dataIncial,dataFinal);

        return TypedResults.Ok(list.Adapt<IEnumerable<RelatorioOrcamentoResponse>>());
    }
}
