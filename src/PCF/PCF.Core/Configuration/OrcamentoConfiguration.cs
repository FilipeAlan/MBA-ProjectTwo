using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCF.Core.Configuration.Base;
using PCF.Core.Entities;

namespace PCF.Core.Configuration
{
    public class OrcamentoConfiguration : EntityBaseConfiguration<Orcamento>
    {
        public override void Configure(EntityTypeBuilder<Orcamento> builder)
        {
            base.Configure(builder);

            builder.ToTable("Orcamento");

            builder.Property(o => o.ValorLimite)
                   .IsRequired();

            builder.Property(o => o.UsuarioId)
                   .IsRequired();

            builder.HasOne(o => o.Usuario)
                   .WithMany(u => u.Orcamentos)
                   .HasForeignKey(o => o.UsuarioId);

            builder.HasOne(o => o.Categoria)
                   .WithMany(c => c.Orcamentos)
                   .HasForeignKey(o => o.CategoriaId);
        }
    }
}