using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PCF.SPA.Services;

namespace PCF.SPA.Pages.Reports
{
    public partial class RelatorioGastosPorCategoria
    {
        private IEnumerable<RelatorioGastoPorCategoriaResponse> _relatorioGastoPorCategoria = new List<RelatorioGastoPorCategoriaResponse>();

        private bool _loading = false;
        private MudDateRangePicker _picker = default!;
        private DateRange _dateRange = new DateRange(
            new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Local),
            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local)
        );
        private bool _autoClose = false;
        private decimal _saldo = 0;
        private decimal _valorGasto = 0;
        private decimal _valorLimite = 0;

        [Inject] private IWebApiClient WebApiClient { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            await LoadOrcamentosAsync();
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
                    _relatorioGastoPorCategoria = await WebApiClient.GastoporcategoriaAsync(dataInicial.Value, dataFinal.Value);
                    _valorGasto = (decimal)_relatorioGastoPorCategoria.Sum(x => x.ValorTotal);
                    _valorLimite = (decimal)_relatorioGastoPorCategoria.Sum(x => x.ValorLimite);
                    _saldo = _valorLimite - _valorGasto;
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
        private async Task ExportExcel()
        {
            var excelBytes = await ExcelExportService.ExportToExcelAsync(_relatorioGastoPorCategoria.ToList());

            if (excelBytes != null)
            {
                var fileUrl = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{Convert.ToBase64String(excelBytes)}";
                await JS.InvokeVoidAsync("downloadFile", "relatorio.xlsx", fileUrl);
            }
        }

        private async Task ExportPdf()
        {
            var pdfBytes = await PdfExportService.ExportToPdfAsync(_relatorioGastoPorCategoria.ToList());

            if (pdfBytes != null)
            {
                var fileUrl = $"data:application/pdf;base64,{Convert.ToBase64String(pdfBytes)}";
                await JS.InvokeVoidAsync("downloadFile", "relatorio.pdf", fileUrl);
            }
        }
        private static string LinhaEstilo(RelatorioGastoPorCategoriaResponse item,int index)
        {
            if (item.ValorLimite < item.ValorTotal)
            {
                return "background-color: red; color: white;"; // Altera a cor de fundo para vermelho e a cor do texto para branco
            }
            return ""; // Retorna uma string vazia para o estilo padrão
        }
    }
}
