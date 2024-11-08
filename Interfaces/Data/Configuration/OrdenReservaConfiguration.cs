using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class OrdenReservaConfiguration : IEntityTypeConfiguration<OrdenReserva>
    {
        public void Configure(EntityTypeBuilder<OrdenReserva> builder)
        {
            builder.ToTable("OrdenReserva");

            builder.Property(oc => oc.id)
                .IsRequired();
            builder.Property(oc => oc.cantidad)
                .IsRequired();
            builder.Property(oc => oc.precioFinal)
                .IsRequired();
            builder.Property(oc => oc.precioProducto)
                .IsRequired();
            builder.Property(oc => oc.simboloMoneda)
                .IsRequired();

            builder.HasOne(oc => oc.producto)
               .WithMany(oc => oc.ordenesReservaProducto)
               .HasForeignKey(p => new { p.codigoProducto, p.rucProveedor });

            builder.HasOne(oc => oc.modelo)
               .WithMany(oc => oc.ordenesReservaModelo)
               .HasForeignKey(oc => oc.codigoModelo)
               .IsRequired(false);

            builder.HasOne(oc => oc.reservaProductos)
               .WithMany(oc => oc.ordenesReservas)
               .HasForeignKey(oc => oc.codigoReservaProducto)
               .IsRequired(false);
        }
    }
}
