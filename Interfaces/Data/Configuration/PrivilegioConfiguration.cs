using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class PrivilegioConfiguration : IEntityTypeConfiguration<Privilegio>
    {
        public void Configure(EntityTypeBuilder<Privilegio> builder)
        {
            builder.ToTable("Privilegio");

            builder.Property(p => p.id)
                .IsRequired();
            builder.Property(p => p.tipo)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.descripcion)
                .IsRequired()
                .HasMaxLength(int.MaxValue);

            builder.HasIndex(p => p.tipo).IsUnique();
        }
    }
}
