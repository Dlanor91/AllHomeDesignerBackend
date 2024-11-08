using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class MonedaAddDto
    {
        [Required]
        public string codigo { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public float cotizacion { get; set; }
        [Required]
        public string simbolo { get; set; }
    }
}
