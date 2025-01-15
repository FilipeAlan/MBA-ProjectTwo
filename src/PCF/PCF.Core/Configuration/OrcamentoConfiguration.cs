using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCF.Core.Entities;

namespace PCF.Core.Configuration
{
    public class OrcamentoConfiguration : IEntityTypeConfiguration<Orcamento>
    {
        public void Configure(EntityTypeBuilder<Orcamento> builder)
        {
            builder.HasKey(k => k.Id);
            //builder.Property(p => p.);
            builder.Property(p => p.ValorLimite);
        }
    }
}
