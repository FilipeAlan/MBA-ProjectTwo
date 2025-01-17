using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCF.Core.Configuration.Base;
using PCF.Core.Entities;

namespace PCF.Core.Configuration
{
    public class UsuarioConfiguration : EntityBaseConfiguration<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);

            builder.ToTable("Usuario");

            builder.Property(u => u.Nome)
                   .HasMaxLength(250)
                   .IsRequired();

            builder.HasMany(u => u.Transacoes)
                   .WithOne(t => t.Usuario)
                   .HasForeignKey(t => t.UsuarioId);

            builder.HasMany(u => u.Categorias)
                   .WithOne(c => c.Usuario)
                   .HasForeignKey(c => c.UsuarioId);

            builder.HasMany(u => u.Orcamentos)
                   .WithOne(c => c.Usuario)
                   .HasForeignKey(c => c.UsuarioId);
        }
    }
}