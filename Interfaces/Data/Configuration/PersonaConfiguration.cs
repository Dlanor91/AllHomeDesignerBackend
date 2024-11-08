using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Persona");

            builder.Property(pe => pe.documento)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(pe => pe.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(pe => pe.nombre)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(pe => pe.apellido)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(pe => pe.nombreUsuario)
                .HasMaxLength(50);
            builder.Property(pe => pe.email)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(pe => pe.password)
                .HasMaxLength(int.MaxValue);

            builder.HasIndex(pe => pe.nombreUsuario).IsUnique();
            builder.HasIndex(pe => pe.email).IsUnique();

            builder.HasOne(pe => pe.tipoUsuario)
                .WithMany(pe => pe.personas)
                .HasForeignKey(pe => pe.idTipoUsuario);
            builder.HasOne(pe => pe.sucursal)
                .WithMany(pe => pe.trabajadores)
                .HasForeignKey(pe => pe.codigoSucursal);
        }
    }
}
