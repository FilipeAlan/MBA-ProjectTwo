using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PCF.API.Configuration;
using PCF.API.Services;
using PCF.Core.Config;
using PCF.Core.Context;
using PCF.Core.Identity;
using PCF.Core.Interface;
using PCF.Core.Repository;
using PCF.Core.Services;
using Vernou.Swashbuckle.HttpResultsAdapter;

// TODO: organizar classe usando extension methods

var builder = WebApplication.CreateBuilder(args);

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin() // Permite qualquer origem (ajuste conforme necessário)
               .AllowAnyMethod() // Permite todos os métodos (GET, POST, etc.)
               .AllowAnyHeader(); // Permite todos os cabeçalhos
    });
});

// Adiciona a Connection String do SQLite
var connectionString = builder.Configuration.GetConnectionString("SQLiteConnection");
builder.Services.AddDbContext<PCFDBContext>(options =>
{
    options.UseSqlite(connectionString);
    options.UseLazyLoadingProxies();

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
}
);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

// TODO: adicionar config para usar SQL Server quando for production

// Configura serviços do Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<PCFDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAppIdentityUser, AppIdentityUser>();

// Adiciona serviços da API
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
builder.Services.AddScoped<IOrcamentoService, OrcamentoService>();

builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

builder.Services.AddJwtAuthentication(builder.Configuration);

// Configura Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<HttpResultsOperationFilter>();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "PCF WebAPI", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<PCFDBContext>();
            await dbContext.Database.MigrateAsync();
            await dbContext.EnsureSeedDataAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao aplicar migrations: {ex.Message}");
        }
    }

    // Configurações do Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PCF API v1");
        c.RoutePrefix = string.Empty; // Faz com que o Swagger seja a página inicial
    });
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

await app.RunAsync();