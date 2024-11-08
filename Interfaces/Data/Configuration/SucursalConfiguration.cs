using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class SucursalConfiguration : IEntityTypeConfiguration<Sucursal>
    {
        public void Configure(EntityTypeBuilder<Sucursal> builder)
        {
            builder.ToTable("Sucursal");

            builder.Property(s => s.codigo)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(s => s.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(c => c.nombre)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.email)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.fechaRegistro)
                .IsRequired();
            builder.Property(c => c.detalles)
                .HasMaxLength(50);

            builder.HasIndex(pe => pe.email).IsUnique();

            builder.HasOne(pe => pe.filial)
                .WithMany(pe => pe.sucursales)
                .HasForeignKey(pe => pe.rucFilial);

            builder.HasOne(d => d.direccion)
                .WithOne(d => d.sucursal)
                .HasForeignKey<Sucursal>(d => d.idDireccion);
            builder.HasOne(d => d.telefono)
                .WithOne(d => d.sucursal)
                .HasForeignKey<Sucursal>(d => d.idTelefono);
        }
    }
}
