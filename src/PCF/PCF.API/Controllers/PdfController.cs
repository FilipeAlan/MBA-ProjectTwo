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

        [HttpPost("generate")]
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

            var pdfBytes = _pdfService.GeneratePdfFromTable(formattedList, "Relatório PDF");

            return File(pdfBytes, "application/pdf", "relatorio.pdf");
        }
    }
}
