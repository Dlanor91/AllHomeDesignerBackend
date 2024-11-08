using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categoria");

            builder.Property(c => c.id)
                .IsRequired();
            builder.Property(c => c.nombre)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.descripcion)
                .IsRequired()
                .HasMaxLength(int.MaxValue);
        }
    }
}
