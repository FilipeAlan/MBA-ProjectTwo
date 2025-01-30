using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Entities;

namespace PCF.Core.Config
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedDataAsync(this PCFDBContext dbContext)
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