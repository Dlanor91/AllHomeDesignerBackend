using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Producto : BaseEntity
    {
        public string codigo { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public float largo { get; set; }
        [Required]
        public float ancho { get; set; }
        [Required]
        public float profundidad { get; set; }
        [Required]
        public int stock { get; set; }
        public int disponibilidad { get; set; }
        public int reserva { get; set; } = 0;
        public string? imagen { get; set; }
        public string? presentacion { get; set; }
        public float? rendimiento { get; set; }
        public string? textura { get; set; }
        public string? sugerencias { get; set; }

        [Required]
        public string rucProveedor { get; set; }
        public Proveedor proveedor { get; set; }
        [Required]
        public int idCategoria { get; set; }
        public Categoria categoria { get; set; }
        [Required]
        public string rucFilial { get; set; }
        public Filial filial { get; set; }

        public ICollection<OrdenReserva> ordenesReservaProducto { get; set; }
        public ICollection<Precio> precios { get; set; }

    }
}
