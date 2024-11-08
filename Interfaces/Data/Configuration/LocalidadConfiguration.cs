using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class LocalidadConfiguration : IEntityTypeConfiguration<Localidad>
    {
        public void Configure(EntityTypeBuilder<Localidad> builder)
        {
            builder.ToTable("Localidad");

            builder.Property(c => c.id)
                .IsRequired();
            builder.Property(c => c.nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(c => c.departamento)
                .WithMany(c => c.localidades)
                .HasForeignKey(c => c.idDepartamento);
        }
    }
}
