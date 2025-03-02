using Microsoft.JSInterop;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PCF.API.Services
{
    public class ExportService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public ExportService(IConfiguration configuration, HttpClient http, IJSRuntime js)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7113/api";
            _http = http;
            _js = js;
        }

        public async Task ExportExcel(object relatorioOrcamento)
        {
            var url = $"{_apiBaseUrl}/excel/generate";
            var response = await _http.PostAsJsonAsync(url, relatorioOrcamento);

            if (response.IsSuccessStatusCode)
            {
                var excelBytes = await response.Content.ReadAsByteArrayAsync();
                var fileUrl = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{Convert.ToBase64String(excelBytes)}";
                await _js.InvokeVoidAsync("downloadFile", "relatorio.xlsx", fileUrl);
            }
        }

        public async Task ExportPdf(object relatorioOrcamento)
        {
            var url = $"{_apiBaseUrl}/pdf/generate";
            var response = await _http.PostAsJsonAsync(url, relatorioOrcamento);

            if (response.IsSuccessStatusCode)
            {
                var pdfBytes = await response.Content.ReadAsByteArrayAsync();
                var fileUrl = $"data:application/pdf;base64,{Convert.ToBase64String(pdfBytes)}";
                await _js.InvokeVoidAsync("downloadFile", "relatorio.pdf", fileUrl);
            }
        }
    }
}
