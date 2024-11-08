using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class FilialConfiguration : IEntityTypeConfiguration<Filial>
    {
        public void Configure(EntityTypeBuilder<Filial> builder)
        {
            builder.ToTable("Filial");

            builder.Property(a => a.ruc)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(a => a.nombre)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.email)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.fechaRegistro)
                .IsRequired();
            builder.Property(a => a.estado)
                .HasMaxLength(50);
        }
    }
}
