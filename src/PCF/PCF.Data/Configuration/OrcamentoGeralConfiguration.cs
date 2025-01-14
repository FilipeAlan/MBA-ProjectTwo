using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PCF.Data.Entities;

namespace PCF.Data.Configuration
{
    public class OrcamentoGeralConfiguration : IEntityTypeConfiguration<OrcamentoGeral>
    {
        public void Configure(EntityTypeBuilder<OrcamentoGeral> builder)
        {
            builder.HasKey(k => k.Id);
            //builder.Property(p => p.Mes);
            builder.Property(p => p.ValorLimite);
        }
    }
}
