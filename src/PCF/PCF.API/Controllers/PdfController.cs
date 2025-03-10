using Microsoft.AspNetCore.Mvc;
using PCF.API.Services;
using PCF.Core.Dtos.Relatorio;
using System.Collections.Generic;

namespace PCF.API.Controllers
{
    [ApiController]
    [Route("api/pdf")]
    public class PdfController : ControllerBase
    {
        private readonly PdfExportService _pdfService;

        public PdfController(PdfExportService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("generateOrcamento")]
        public IActionResult GeneratePdf([FromBody] List<RelatorioOrcamentoResponse> _relatorioOrcamento)
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

            var pdfBytes = _pdfService.GeneratePdfFromTable(formattedList, "Relatório Orcamento");

            return File(pdfBytes, "application/pdf", "relatorioOrcamento.pdf");
        }
        [HttpPost("generateGastoPorCategoria")]
        public IActionResult GeneratePdf([FromBody] List<RelatorioGastoPorCategoriaResponse> _relatorioGastoPorCategoria)
        {
            var formattedList = _relatorioGastoPorCategoria
                .Select(r => new
                {
                    r.CategoriaId,

                    r.Categoria,
                    r.ValorLimite,
                    r.ValorTotal                    
                }).ToList();

            var pdfBytes = _pdfService.GeneratePdfFromTable(formattedList, "Relatório Gastos Por Categoria");

            return File(pdfBytes, "application/pdf", "relatorioGastoPorCategoria.pdf");
        }
    }
}
