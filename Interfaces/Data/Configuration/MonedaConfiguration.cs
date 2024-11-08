using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
    {
        public void Configure(EntityTypeBuilder<Moneda> builder)
        {
            builder.ToTable("Moneda");

            builder.Property(m => m.codigo)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(m => m.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(m => m.descripcion)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(m => m.cotizacion)
                .IsRequired();
            builder.Property(m => m.fecha)
                .IsRequired()
                .HasColumnType("date");
            builder.Property(m => m.simbolo)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
