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
            var response = await _httpClient.PostAsJsonAsync("/api/excel/generateOrcamento", relatorioOrcamento);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
        public async Task<byte[]?> ExportToExcelAsync(List<RelatorioGastoPorCategoriaResponse> relatorioGastoPorCategoriaResponse)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/excel/generateGastoPorCategoria", relatorioGastoPorCategoriaResponse);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
    }
}
