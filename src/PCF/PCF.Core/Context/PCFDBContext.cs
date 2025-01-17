using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCF.Core.Entities;
using PCF.Core.Entities.Base;
using PCF.Core.Identity;

namespace PCF.Core.Context
{
    public class PCFDBContext : IdentityDbContext<ApplicationUser, ApplicationRole , int>
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public PCFDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(PCFDBContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateModifiedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateModifiedAt();
            return base.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await SaveChangesAsync().ConfigureAwait(false);
        }

        private void UpdateModifiedAt()
        {
            var entries = ChangeTracker
                            .Entries()
                            .Where(e => e.Entity is Entity && e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                ((Entity)entityEntry.Entity).ModificadoEm = DateTime.Now;
            }
        }
    }
}