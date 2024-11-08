using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresa");

            builder.Property(e => e.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsRequired(false);
            builder.Property(e => e.ruc)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(e => e.razonSocial)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(e => e.email)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(e => e.comentarios)
               .HasMaxLength(500);

            builder.HasOne(e => e.tipoUsuario)
                .WithMany(e => e.empresas)
                .HasForeignKey(e => e.idTipoUsuario);

            builder.HasIndex(e => e.email).IsUnique();
            builder.HasIndex(e => e.ruc).IsUnique();
            builder.HasIndex(e => e.razonSocial).IsUnique();
        }
    }
}
