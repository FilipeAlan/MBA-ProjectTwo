namespace PCF.SPA.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Método genérico para realizar requisições GET.
        /// </summary>
        /// <typeparam name="T">O tipo do objeto esperado na resposta.</typeparam>
        /// <param name="url">A URL para onde a requisição será feita.</param>
        /// <returns>O objeto desserializado da resposta.</returns>
        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao realizar GET na URL {url}: {ex.Message}");
            }
        }

        /// <summary>
        /// Método genérico para realizar requisições POST.
        /// </summary>
        /// <typeparam name="TRequest">O tipo do objeto enviado no corpo da requisição.</typeparam>
        /// <typeparam name="TResponse">O tipo do objeto esperado na resposta.</typeparam>
        /// <param name="url">A URL para onde a requisição será feita.</param>
        /// <param name="data">O objeto a ser enviado no corpo da requisição.</param>
        /// <returns>O objeto desserializado da resposta.</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao realizar POST na URL {url}: {ex.Message}");
            }
        }
    }

}
