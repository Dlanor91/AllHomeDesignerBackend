using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            //aqui implemento el nombre de tabla como context y annado los parametros
            builder.ToTable("Producto");

            builder.Property(p => p.codigo)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(p => p.nombre)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.descripcion)
                .IsRequired()
                .HasMaxLength(int.MaxValue);
            builder.Property(p => p.largo)
                .IsRequired();
            builder.Property(p => p.ancho)
                .IsRequired();
            builder.Property(p => p.profundidad)
                .IsRequired();
            builder.Property(p => p.stock)
                .IsRequired();
            builder.Property(p => p.disponibilidad);
            builder.Property(p => p.reserva);
            builder.Property(p => p.imagen)
                .HasMaxLength(100);
            builder.Property(p => p.presentacion)
                .HasMaxLength(50);
            builder.Property(p => p.rendimiento)
                .HasMaxLength(100);
            builder.Property(p => p.textura)
                .HasMaxLength(100);
            builder.Property(p => p.sugerencias)
                .HasMaxLength(int.MaxValue);

            builder.HasKey(p => new { p.codigo, p.rucProveedor });

            builder.HasOne(p => p.proveedor)
                .WithMany(p => p.productos)
                .HasForeignKey(p => p.rucProveedor)
                .IsRequired();
            builder.HasOne(p => p.categoria)
                .WithMany(p => p.productos)
                .HasForeignKey(p => p.idCategoria)
                .IsRequired();
            builder.HasOne(p => p.filial)
                .WithMany(p => p.productos)
                .HasForeignKey(p => p.rucFilial)
                .IsRequired();
        }
    }
}
