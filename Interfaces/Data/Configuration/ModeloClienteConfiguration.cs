using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class ModeloClienteConfiguration : IEntityTypeConfiguration<ModeloCliente>
    {
        public void Configure(EntityTypeBuilder<ModeloCliente> builder)
        {
            builder.ToTable("ModeloCliente");

            builder.Property(mc => mc.id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(mc => mc.cantidad)
                .IsRequired();

            builder.HasKey(mc => new { mc.documentoCliente, mc.codigoModelo });

            builder.HasOne(mc => mc.personaCliente)
               .WithMany(mc => mc.modelosClienteGenerados)
               .HasForeignKey(mc => mc.documentoCliente);

            builder.HasOne(mc => mc.modelo)
               .WithMany(mc => mc.modelosCodigosGenerados)
               .HasForeignKey(mc => mc.codigoModelo);
        }
    }
}
