using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class ReservaProductosConfiguration : IEntityTypeConfiguration<ReservaProductos>
    {
        public void Configure(EntityTypeBuilder<ReservaProductos> builder)
        {
            builder.ToTable("ReservaProductos");

            builder.Property(rp => rp.codigo)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(rp => rp.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(rp => rp.fechaCompra)
                .IsRequired();

            builder.HasOne(rp => rp.cliente)
               .WithMany(rp => rp.reservaProductosClientes)
               .HasForeignKey(rp => rp.documentoCliente);

            builder.HasOne(rp => rp.profesional)
               .WithMany(rp => rp.reservaProductosProfesionales)
               .HasForeignKey(rp => rp.documentoProfesional);

            builder.HasOne(rp => rp.empresa)
               .WithMany(rp => rp.reservaProductos)
               .HasForeignKey(rp => rp.rucEmpresa);

            builder.HasOne(rp => rp.filial)
               .WithMany(rp => rp.reservaProductos)
               .HasForeignKey(rp => rp.rucFilial);

        }
    }
}
