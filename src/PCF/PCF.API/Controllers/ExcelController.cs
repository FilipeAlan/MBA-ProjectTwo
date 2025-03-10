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

        [HttpPost("generateOrcamento")]
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

            var excelBytes = _excelService.GenerateExcelFromTable(formattedList, "Relatório Orcamento");

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatorioOrcamento.xlsx");
        }

        [HttpPost("generateGastoPorCategoria")]
        public IActionResult GenerateExcel([FromBody] List<RelatorioGastoPorCategoriaResponse> _relatorioGastoPorCategoria)
        {
            var formattedList = _relatorioGastoPorCategoria
                .Select(r => new
                {
                    r.CategoriaId,

                    r.Categoria,
                    r.ValorLimite,
                    r.ValorTotal
                }).ToList();

            var excelBytes = _excelService.GenerateExcelFromTable(formattedList, "Relatório Gastos Por Categoria");

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatorioGastoPorCategoria.xlsx");
        }
    }

}
