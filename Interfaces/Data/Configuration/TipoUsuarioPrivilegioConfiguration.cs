using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuration
{
    public class TipoUsuarioPrivilegioConfiguration : IEntityTypeConfiguration<TipoUsuarioPrivilegio>
    {
        public void Configure(EntityTypeBuilder<TipoUsuarioPrivilegio> builder)
        {
            builder.ToTable("TipoUsuarioPrivilegio");


            builder.Property(tup => tup.id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.HasKey(tup => new { tup.idPrivilegio, tup.idTipoUsuario });
            builder.HasIndex(tup => new { tup.idPrivilegio, tup.idTipoUsuario }).IsUnique();

            builder.HasOne(tup => tup.tipoUsuarioPrivilegios)
            .WithMany(tup => tup.tiposUsuariosPrivilegios)
            .HasForeignKey(tup => tup.idTipoUsuario);

            builder.HasOne(tup => tup.privilegioTipoUsuario)
                .WithMany(tup => tup.privilegiosTiposUsuarios)
                .HasForeignKey(tup => tup.idPrivilegio);
        }
    }
}
