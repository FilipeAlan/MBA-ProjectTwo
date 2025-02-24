using PCF.API.Configuration;
using PCF.API.Services;
using PCF.Core.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataDependencies(builder.Configuration, builder.Environment)
                .AddSwagger()
                .AddIdentity()
                .SetupWebApi(builder.Configuration)
                .AddApplicationServices()
                .AddRepositories()
                .AddJwtAuthentication(builder.Configuration);

//PDF Service
builder.Services.AddScoped<PdfExportService>();
//Excel Service
builder.Services.AddScoped<ExcelExportService>();


var app = builder.Build();

app.UseCustomSwagger(builder.Environment);
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.UseDbMigrationHelperAsync();
await app.RunAsync();