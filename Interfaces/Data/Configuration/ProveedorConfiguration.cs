using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("Proveedor");
            builder.Property(p => p.ruc)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(p => p.nombre)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
