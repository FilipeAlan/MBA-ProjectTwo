using Microsoft.AspNetCore.Mvc;
using PCF.API.Services;
using PCF.Core.Dtos.Relatorio;
using System.Collections.Generic;

namespace PCF.API.Controllers
{
    [ApiController]
    [Route("api/excel")]
    public class ExcelController : ControllerBase
    {
        private readonly ExcelExportService _excelService;

        public ExcelController(ExcelExportService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost("generate")]
        public IActionResult GenerateExcel([FromBody] List<RelatorioOrcamentoResponse> _relatorioOrcamento)
        {
            var formattedList = _relatorioOrcamento
                .Select(r => new
                {
                    r.TransacaoId,
                    DataLancamento = r.DataLancamento.ToString("dd/MM/yyyy"),
                    r.TipoLancamento,
                    r.Valor,
                    r.ValorLimite,
                    r.Descricao,
                    r.CategoriaId,
                    r.Categoria,
                    r.Tipo,
                    r.UsuarioId,
                    r.Usuario
                })
                .ToList();

            var excelBytes = _excelService.GenerateExcelFromTable(formattedList, "Relatório Excel");

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatorio.xlsx");
        }
    }

}
