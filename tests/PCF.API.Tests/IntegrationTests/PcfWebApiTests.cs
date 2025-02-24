using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PCF.Core.Dtos.Dashboard;
using PCF.Core.Dtos.Login;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PCF.API.Tests.IntegrationTests
{
    public class PcfWebApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string _registerEndpoint = "/api/auth/register";
        private const string _loginEndpoint = "/api/auth/login";
        private const string _dashboardResumoEndpoint = "/api/dashboard/resumo";

        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private string? _jwtToken = null;

        public PcfWebApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateUser_ReturnsOk_ForValidModel()
        {
            // Arrange
            var model = new RegisterRequest()
            {
                Login = GetFakeEmail(),
                Password = GetFakePassword(),
                Name = "Maria"
            };

            // Act
            var response = await _client.PostAsJsonAsync(_registerEndpoint, model);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_ForInvalidEmail()
        {
            // Arrange
            var model = new RegisterRequest()
            {
                Login = "test.com",
                Password = GetFakePassword(),
                Name = "Maria"
            };

            // Act
            var response = await _client.PostAsJsonAsync(_registerEndpoint, model);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_ForEmptyName()
        {
            // Arrange
            var model = new RegisterRequest()
            {
                Login = GetFakeEmail(),
                Password = GetFakePassword(),
                Name = null!
            };

            // Act
            var response = await _client.PostAsJsonAsync(_registerEndpoint, model);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var registerRequest = new RegisterRequest()
            {
                Login = GetFakeEmail(),
                Password = GetFakePassword(),
                Name = "Maria"
            };

            var responseRegister = await _client.PostAsJsonAsync(_registerEndpoint, registerRequest);
            responseRegister.EnsureSuccessStatusCode();

            var validCredentials = new LoginRequest()
            {
                Login = registerRequest.Login,
                Password = registerRequest.Password
            };

            // Act
            var response = await _client.PostAsJsonAsync(_loginEndpoint, validCredentials);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNull();
            _jwtToken = result.Trim('"');
        }

        [Fact]
        public async Task Login_Fail_ForBadCredentials()
        {
            // Arrange
            var invalidCredentials = new LoginRequest()
            {
                Login = GetFakeEmail(),
                Password = GetFakePassword()
            };

            // Act
            var response = await _client.PostAsJsonAsync(_loginEndpoint, invalidCredentials);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Dashboard_Fail_With_Unauthorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.GetAsync($"{_dashboardResumoEndpoint}?request={DateTime.Now:yyyy-MM-dd}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Dashboard_ReturnsOk()
        {
            // Arrange
            await Login_ReturnsOk_WhenCredentialsAreValid();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            // Act
            var response = await _client.GetAsync($"{_dashboardResumoEndpoint}?request={DateTime.Now:yyyy-MM-dd}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<DashboardSummary>();
            result.Should().NotBeNull();
        }

        private static string GetFakeEmail() => $"example{Random.Shared.Next(0, int.MaxValue)}@test.com";

        private static string GetFakePassword()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*";

            var random = new Random();
            var password = new char[8];
            
            password[0] = upper[random.Next(upper.Length)];
            password[1] = lower[random.Next(lower.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = special[random.Next(special.Length)];

            for (int i = 4; i < 8; i++)
            {
                string allChars = upper + lower + digits + special;
                password[i] = allChars[random.Next(allChars.Length)];
            }

            return new string(password.OrderBy(c => random.Next()).ToArray());
        }
    }
}