using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCF.Core.Configuration.Base;
using PCF.Core.Entities;

namespace PCF.Core.Configuration
{
    public class CategoriaConfiguration : EntityBaseConfiguration<Categoria>
    {
        public override void Configure(EntityTypeBuilder<Categoria> builder)
        {
            base.Configure(builder);

            builder.ToTable("Categoria");

            builder.Property(p => p.Nome)
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(p => p.Descricao)
                   .HasMaxLength(500);

            builder.Property(p => p.Padrao)
                   .HasDefaultValue(false);

            builder.HasIndex(p => p.UsuarioId);

            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Categorias)
                   .HasForeignKey(fk => fk.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}