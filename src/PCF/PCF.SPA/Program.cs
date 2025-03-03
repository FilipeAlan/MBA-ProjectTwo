using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PCF.SPA;
using PCF.SPA.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthManagerService>();

// Cria uma instância temporária de HttpClient para carregar o appsettings.json
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var appSettingsJson = await httpClient.GetStringAsync("appsettings.json");

// Adiciona as configurações do appsettings.json ao Configuration
using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(appSettingsJson));
builder.Configuration.AddJsonStream(memoryStream);

// Adiciona os serviços de exportação de PDF e Excel
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<PdfExportService>();

// Obtém o valor de ApiUrl
var apiUrl = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;

// Configura o HttpClient para o restante da aplicação
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
builder.Services.AddScoped<IWebApiClient>(sp =>
{
    var uri = new Uri(apiUrl);
    var baseUri = new Uri(uri, "/").ToString();
    return new WebApiClient(baseUri, sp.GetRequiredService<HttpClient>());
}
);

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthManagerService>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddLocalization();
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

await builder.Build().RunAsync();