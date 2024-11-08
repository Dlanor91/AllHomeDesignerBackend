using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Interfaces.Data
{
    public class AHDContext : DbContext
    {
        public AHDContext(DbContextOptions<AHDContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Telefono> Telefonos { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Localidad> Localidades { get; set; }
        public DbSet<TipoUsuario> TiposUsuarios { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<ModeloCliente> ModelosClientes { get; set; }
        public DbSet<OrdenReserva> OrdenesReservas { get; set; }
        public DbSet<Privilegio> Privilegios { get; set; }
        public DbSet<TipoUsuarioPrivilegio> TiposUsuariosPrivilegios { get; set; }
        public DbSet<ReservaProductos> ReservasProductos { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Precio> Precios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Filial> Filial { get; set; }
        public DbSet<Sucursal> Sucursal { get; set; }
        public DbSet<ClienteRegistro> ClientesRegistrados { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
