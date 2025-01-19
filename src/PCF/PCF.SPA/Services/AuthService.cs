using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace PCF.SPA.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        // Evento para notificar mudanças no estado de autenticação
        public event Action? OnAuthenticationStateChanged;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async Task<bool> LoginAsync(LoginResponseDto loginResponseDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginResponseDto);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        await _localStorage.SetItemAsync("authToken", token);
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                        // Notifica a mudança de estado de autenticação
                        NotifyAuthenticationStateChanged();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no login: {ex.Message}");
                return false;
            }
        }
        public async Task LogoutAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = null;

                // Notifica a mudança de estado de autenticação
                NotifyAuthenticationStateChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar logout: {ex.Message}");
                throw; // Re-lança a exceção para ser tratada em um nível superior, se necessário
            }
        }
        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task<bool> IsLoggedIn()
        {
            return !string.IsNullOrEmpty(await GetTokenAsync());
        }

        public async Task<string> RegisterAsync(LoginResponseDto loginResponseDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/register", loginResponseDto);
                if (response.IsSuccessStatusCode)
                {
                    return "Registrado com sucesso";
                }
                // Captura a mensagem de erro retornada pelo servidor
                var errorMessage = await response.Content.ReadAsStringAsync();
                return string.IsNullOrWhiteSpace(errorMessage)
                    ? "Falha ao registrar usuário."
                    : errorMessage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar usuário: {ex.Message}");
                return "Erro ao registrar usuário. Tente novamente mais tarde.";
            }
        }
        // Método para notificar a mudança de estado de autenticação
        private void NotifyAuthenticationStateChanged()
        {
            OnAuthenticationStateChanged?.Invoke();
        }
    }
}


