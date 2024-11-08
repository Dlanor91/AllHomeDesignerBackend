using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PrecioAddDto
    {
        [Required]
        public float precioLista { get; set; }
        [Required]
        public float precioVenta { get; set; }
        [Required]
        public float iva { get; set; }
        [Required]
        public string codigoMoneda { get; set; }
        [Required]
        public string rucProveedor { get; set; }
        [Required]
        public string codigoProducto { get; set; }
    }
}
