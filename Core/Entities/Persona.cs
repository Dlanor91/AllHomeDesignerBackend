using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Persona : BaseEntity
    {
        [Key]
        public string documento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string? nombreUsuario { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string? password { get; set; }

        public int idTipoUsuario { get; set; }
        public TipoUsuario tipoUsuario { get; set; }

        public string? codigoSucursal { get; set; }
        public Sucursal? sucursal { get; set; }

        public ICollection<Telefono>? telefonos { get; set; }
        public ICollection<Direccion>? direcciones { get; set; }
        public ICollection<Modelo>? modelosUsuarios { get; set; }
        public ICollection<Modelo>? modelosCliente { get; set; }
        public ICollection<ModeloCliente>? modelosClienteGenerados { get; set; }
        public ICollection<ReservaProductos>? reservaProductosClientes { get; set; }
        public ICollection<ClienteRegistro>? clientesProfesionales { get; set; }
        public ICollection<ReservaProductos>? reservaProductosProfesionales { get; set; }
    }
}
