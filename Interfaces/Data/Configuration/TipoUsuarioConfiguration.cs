using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class TipoUsuarioConfiguration : IEntityTypeConfiguration<TipoUsuario>
    {
        public void Configure(EntityTypeBuilder<TipoUsuario> builder)
        {
            builder.ToTable("TipoUsuario");

            builder.Property(tu => tu.id)
                .IsRequired();
            builder.Property(tu => tu.rol)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(tu => tu.descripcionRol)
                .IsRequired()
                .HasMaxLength(int.MaxValue);

            builder.HasIndex(tu => tu.rol).IsUnique();
        }
    }
}
