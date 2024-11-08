using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class ModeloConfiguration : IEntityTypeConfiguration<Modelo>
    {
        public void Configure(EntityTypeBuilder<Modelo> builder)
        {
            builder.ToTable("Modelo");

            builder.Property(m => m.codigo)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(m => m.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(m => m.fecha)
                .IsRequired();

            builder.HasOne(m => m.usuario)
                .WithMany(m => m.modelosUsuarios)
                .HasForeignKey(m => m.personaUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(m => m.cliente)
                .WithMany(m => m.modelosCliente)
                .HasForeignKey(m => m.personaCliente)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
