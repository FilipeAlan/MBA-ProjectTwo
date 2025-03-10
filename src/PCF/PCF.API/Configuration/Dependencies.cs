using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCF.API.Services;
using PCF.Core.Context;
using PCF.Core.Identity;
using PCF.Core.Interface;
using PCF.Core.Repository;
using PCF.Core.Services;
using System.Text.Json.Serialization;

namespace PCF.API.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddDbContext<PCFDBContext>(options =>
            {
                if (hostEnvironment.IsDevelopment())
                {
                    options.UseSqlite(configuration.GetConnectionString("SQLiteConnection") ?? throw new InvalidOperationException("Não localizada connection string para ambiente de desenvolvimento (SQLite)"));
                }
                else
                {
                    options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Não localizada connection string para ambiente de produção (SQL Server)"));
                }

                options.UseLazyLoadingProxies();

                if (hostEnvironment.IsDevelopment())
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            });

            if (hostEnvironment.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IOrcamentoService, OrcamentoService>();
            services.AddScoped<ITransacaoService, TransacaoService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IRelatorioService, RelatorioService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            services.AddScoped<PdfExportService>();
            services.AddScoped<ExcelExportService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IRelatorioRepository, RelatorioRepository>();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<PCFDBContext>()
                    .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection SetupWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetRequiredSection("Security:CorsPolicy:AllowedOrigins").Get<List<string>>()!;

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(allowedOrigins.ToArray())
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            // Adiciona serviços da API
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            return services;
        }
    }
}