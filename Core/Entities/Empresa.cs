using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Empresa : BaseEntity
    {
        public string? nombre { get; set; }
        [Key]
        [Required]
        public string ruc { get; set; }
        [Required]
        public string razonSocial { get; set; }
        [EmailAddress]
        [Required]
        public string email { get; set; }
        public string? comentarios { get; set; }
        [Required]
        public int idTipoUsuario { get; set; }
        public TipoUsuario tipoUsuario { get; set; }
        public ICollection<Telefono>? telefonos { get; set; }
        public ICollection<Direccion>? direcciones { get; set; }
        public ICollection<ClienteRegistro>? vendedoresServicios { get; set; }
        public ICollection<ReservaProductos>? reservaProductos { get; set; }
    }
}
