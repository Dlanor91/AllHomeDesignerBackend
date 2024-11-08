using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class DireccionConfiguration : IEntityTypeConfiguration<Direccion>
    {
        public void Configure(EntityTypeBuilder<Direccion> builder)
        {
            builder.ToTable("Direccion");

            builder.Property(d => d.id)
                .IsRequired();
            builder.Property(d => d.calle)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(d => d.nroPuerta)
                .IsRequired();
            builder.Property(d => d.datos)
                .HasMaxLength(50);
            builder.Property(d => d.complemento)
                .HasMaxLength(50);
            builder.Property(d => d.departamento)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(d => d.persona)
                .WithMany(d => d.direcciones)
                .HasForeignKey(d => d.documentoPersona);
            builder.HasOne(d => d.localidad)
                .WithMany(d => d.direcciones)
                .HasForeignKey(d => d.idLocalidad);
            builder.HasOne(d => d.empresa)
                .WithMany(d => d.direcciones)
                .HasForeignKey(d => d.rucEmpresa);

            builder.HasOne(d => d.sucursal)
                .WithOne(d => d.direccion)
                .HasForeignKey<Direccion>(d => d.codigoSucursal);
        }
    }
}
