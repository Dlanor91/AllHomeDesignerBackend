using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class ClienteRegistroConfiguration : IEntityTypeConfiguration<ClienteRegistro>
    {
        public void Configure(EntityTypeBuilder<ClienteRegistro> builder)
        {
            builder.ToTable("ClienteRegistro");

            builder.HasOne(cr => cr.cliente)
                .WithMany(cr => cr.clientesProfesionales)
                .HasForeignKey(cr => cr.documentoCliente);

            builder.HasOne(cr => cr.empresa)
                .WithMany(cr => cr.vendedoresServicios)
                .HasForeignKey(cr => cr.rucEmpresa);

            builder.HasOne(cr => cr.filial)
                .WithMany(cr => cr.clientesFiliales)
                .HasForeignKey(cr => cr.rucFilial);
        }
    }
}
