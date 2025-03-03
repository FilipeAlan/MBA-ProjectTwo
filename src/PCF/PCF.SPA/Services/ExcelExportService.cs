using System.Net.Http.Json;

namespace PCF.SPA.Services
{
    public class ExcelExportService
    {

        private readonly HttpClient _httpClient;

        public ExcelExportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]?> ExportToExcelAsync(List<RelatorioOrcamentoResponse> relatorioOrcamento)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/excel/generate", relatorioOrcamento);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
    }
}
