
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PCF.Data.Context
{
    public class PCFDBContext: IdentityDbContext
    {
        public PCFDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(PCFDBContext).Assembly);
        }
    }
}
