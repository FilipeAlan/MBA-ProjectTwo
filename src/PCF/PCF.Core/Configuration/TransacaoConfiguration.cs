using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCF.Core.Configuration.Base;
using PCF.Core.Entities;

namespace PCF.Core.Configuration
{
    public class TransacaoConfiguration : EntityBaseConfiguration<Transacao>
    {
        public override void Configure(EntityTypeBuilder<Transacao> builder)
        {
            base.Configure(builder);

            builder.ToTable("Transacao");

            builder.Property(t => t.Valor)
                   .IsRequired();

            builder.Property(t => t.DataLancamento)
                   .IsRequired();
        }
    }
}