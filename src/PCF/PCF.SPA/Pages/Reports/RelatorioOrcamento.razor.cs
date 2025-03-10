using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PCF.SPA.Services;
namespace PCF.SPA.Pages.Reports
{
    public partial class RelatorioOrcamento
    {
        private IEnumerable<RelatorioOrcamentoResponse> _relatorioOrcamento = new List<RelatorioOrcamentoResponse>(); 
                                                                                                                      
        private bool _loading = false;
        private MudDateRangePicker _picker = default!;
        private DateRange _dateRange = new DateRange(
            new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Local),
            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local)
        );
        private bool _autoClose= false;
        private decimal _saldo = 0;
        private decimal _valorEntrada = 0;
        private decimal _valorSaida = 0;



        [Inject] private IWebApiClient WebApiClient { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadOrcamentosAsync();
        }
        private async Task ExportExcel()
        {
            var excelBytes = await ExcelExportService.ExportToExcelAsync(_relatorioOrcamento.ToList());

            if (excelBytes != null)
            {
                var fileUrl = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{Convert.ToBase64String(excelBytes)}";
                await JS.InvokeVoidAsync("downloadFile", "relatorio.xlsx", fileUrl);
            }
        }

        private async Task ExportPdf()
        {
            var pdfBytes = await PdfExportService.ExportToPdfAsync(_relatorioOrcamento.ToList());

            if (pdfBytes != null)
            {
                var fileUrl = $"data:application/pdf;base64,{Convert.ToBase64String(pdfBytes)}";
                await JS.InvokeVoidAsync("downloadFile", "relatorio.pdf", fileUrl);
            }
        }
        private async Task LoadOrcamentosAsync()
        {
            _loading = true;
            try
            {
                var dataInicial = _dateRange.Start;
                var dataFinal = _dateRange.End?.AddDays(1).AddSeconds(-1);

                if (dataInicial.HasValue && dataFinal.HasValue)
                {
                    _relatorioOrcamento = await WebApiClient.OrcamentorealizadoAsync(dataInicial.Value, dataFinal.Value);
                    _valorEntrada = (decimal) _relatorioOrcamento.Where(t=>t.TipoLancamento=="Entrada").Sum(x => x.Valor );
                    _valorSaida = (decimal)_relatorioOrcamento.Where(t => t.TipoLancamento != "Entrada").Sum(x => x.Valor);
                    _saldo = _valorEntrada - _valorSaida;
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Erro não esperado: {ex.Message}", Severity.Error);
            }
            finally
            {
                _loading = false;
            }
        }
    }
}
