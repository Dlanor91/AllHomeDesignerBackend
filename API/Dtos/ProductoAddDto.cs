using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ProductoAddDto
    {
        [Required]
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

        public string presentacion { get; set; }
        public float? rendimiento { get; set; }
        public string? textura { get; set; }
        public string? sugerencias { get; set; }
        [Required]
        public string rucFilial { get; set; }
        [Required]
        public string rucProveedor { get; set; }
        [Required]
        public int idCategoria { get; set; }

    }
}
