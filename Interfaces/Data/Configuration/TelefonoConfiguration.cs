using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class TelefonoConfiguration : IEntityTypeConfiguration<Telefono>
    {
        public void Configure(EntityTypeBuilder<Telefono> builder)
        {
            builder.ToTable("Telefono");

            builder.Property(t => t.id)
                .IsRequired();

            builder.Property(t => t.numero)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(t => t.persona)
                .WithMany(t => t.telefonos)
                .HasForeignKey(t => t.documentoPersona);

            builder.HasOne(t => t.empresa)
                .WithMany(t => t.telefonos)
                .HasForeignKey(t => t.rucEmpresa);

            builder.HasOne(d => d.sucursal)
                .WithOne(d => d.telefono)
                .HasForeignKey<Telefono>(d => d.codigoSucursal);
        }
    }
}
