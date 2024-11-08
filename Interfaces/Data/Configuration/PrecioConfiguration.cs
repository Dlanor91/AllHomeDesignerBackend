using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class PrecioConfiguration : IEntityTypeConfiguration<Precio>
    {
        public void Configure(EntityTypeBuilder<Precio> builder)
        {
            builder.ToTable("Precio");

            builder.HasKey(p => p.id);
            builder.Property(p => p.precioLista)
                .IsRequired();
            builder.Property(p => p.precioVenta)
                .IsRequired();
            builder.Property(p => p.iva)
                .IsRequired();
            builder.Property(p => p.precioFinal)
                .IsRequired();
            builder.Property(p => p.fecha)
                .IsRequired()
                .HasColumnType("date");

            builder.HasOne(p => p.moneda)
                .WithMany(p => p.precios)
                .HasForeignKey(p => p.codigoMoneda)
                .IsRequired();

            builder.HasOne(p => p.producto)
               .WithMany(p => p.precios)
               .HasForeignKey(p => new { p.codigoProducto, p.rucProveedor })
               .IsRequired();
        }
    }
}
