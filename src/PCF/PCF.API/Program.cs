using PCF.API.Configuration;
using PCF.Core.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataDependencies(builder.Configuration, builder.Environment)
                .AddSwagger()
                .AddIdentity()
                .SetupWebApi()
                .AddApplicationServices()
                .AddRepositories()
                .AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseCustomSwagger(builder.Environment);
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.UseDbMigrationHelperAsync();
await app.RunAsync();