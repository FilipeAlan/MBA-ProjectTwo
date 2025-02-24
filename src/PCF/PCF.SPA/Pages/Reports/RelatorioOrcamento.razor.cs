using PCF.SPA.Services;
using PCF.SPA.Components.Orcamento;
using Microsoft.AspNetCore.Components;
namespace PCF.SPA.Pages.Reports
{
    public partial class RelatorioOrcamento
    {
        private IEnumerable<RelatorioOrcamentoResponse> _relatorioOrcamento = new List<RelatorioOrcamentoResponse>(); // Initialize the list
                                                                                                                      // private IEnumerable<TransacaoResponse> _transacao = new List<TransacaoResponse>(); // Initialize the list
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

        private async Task LoadOrcamentosAsync()
        {
            _loading = true;
            try
            {
                var dataInicial = _dateRange.Start;
                var dataFinal = _dateRange.End?.AddDays(1).AddSeconds(-1);

                if (dataInicial.HasValue && dataFinal.HasValue)
                {
                    _relatorioOrcamento = await WebApiClient.RelatoriosAsync(dataInicial.Value, dataFinal.Value);
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
