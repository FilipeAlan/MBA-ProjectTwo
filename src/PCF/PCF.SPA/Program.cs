using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PCF.SPA;
using PCF.SPA.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthService>();

// Cria uma instância temporária de HttpClient para carregar o appsettings.json
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var appSettingsJson = await httpClient.GetStringAsync("appsettings.json");

// Adiciona as configurações do appsettings.json ao Configuration
using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(appSettingsJson));
builder.Configuration.AddJsonStream(memoryStream);

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

await builder.Build().RunAsync();