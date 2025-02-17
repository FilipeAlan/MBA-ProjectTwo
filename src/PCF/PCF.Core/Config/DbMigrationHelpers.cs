using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PCF.Core.Context;
using PCF.Core.Entities;

namespace PCF.Core.Config
{
    public static class DbMigrationHelpers
    {

        public static async Task UseDbMigrationHelperAsync(this WebApplication app)
        {
            await EnsureSeedDataAsync(app);
        }

        private static async Task EnsureSeedDataAsync(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedDataAsync(services);
        }

        private static async Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                                             .CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                var context = scope.ServiceProvider.GetRequiredService<PCFDBContext>();

                await context.Database.MigrateAsync();
                await EnsureSeedDataAsync(context);
            }
        }

        private static async Task EnsureSeedDataAsync(PCFDBContext dbContext)
        {
            await SeedCategoriasAsync(dbContext);
        }

        private static async Task SeedCategoriasAsync(PCFDBContext dbContext)
        {
            if (await dbContext.Categorias.AnyAsync(c => c.Padrao))
            {
                return;
            }

            var categoriasPadrao = new List<Categoria>
            {
                new() { Nome = "Alimentação", Padrao = true },
                new() { Nome = "Transporte", Padrao = true },
                new() { Nome = "Moradia", Padrao = true },
                new() { Nome = "Investimento", Padrao = true },
                new() { Nome = "Educação", Padrao = true },
                new() { Nome = "Saúde", Padrao = true },
                new() { Nome = "Lazer", Padrao = true },
                new() { Nome = "Salário", Padrao = true }
            };

            await dbContext.Categorias.AddRangeAsync(categoriasPadrao);
            await dbContext.SaveChangesAsync();
        }
    }
}