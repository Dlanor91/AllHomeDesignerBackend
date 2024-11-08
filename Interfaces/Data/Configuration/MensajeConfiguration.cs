using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class MensajeConfiguration : IEntityTypeConfiguration<Mensaje>
    {
        public void Configure(EntityTypeBuilder<Mensaje> builder)
        {
            builder.ToTable("Mensaje");

            builder.Property(m => m.id)
                .IsRequired();
            builder.Property(m => m.nombre)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(m => m.email)
                .IsRequired();
            builder.Property(m => m.telefono);
            builder.Property(m => m.mensaje)
               .IsRequired()
               .HasMaxLength(int.MaxValue);
        }
    }
}
