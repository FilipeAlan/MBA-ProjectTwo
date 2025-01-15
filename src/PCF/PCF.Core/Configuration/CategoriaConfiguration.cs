using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCF.Core.Entities;

namespace PCF.Core.Configuration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Descricao);
            builder.Property(p => p.Nome);
            builder.Property(p => p.Padrao);
            
        }
    }
}
