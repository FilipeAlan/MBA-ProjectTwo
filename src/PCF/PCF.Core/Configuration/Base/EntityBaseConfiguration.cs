using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCF.Core.Entities.Base;

namespace PCF.Core.Configuration.Base
{
    public abstract class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
             where TEntity : Entity

    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(e => e.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.CriadoEm)
                   .IsRequired()
                   .HasPrecision(0);

            builder.Property(e => e.ModificadoEm)
                   .HasPrecision(0);
        }
    }
}