using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class OrdenReservaAddDto
    {
        [Required]
        public string rucProveedor { get; set; }
        [Required]
        public string codigoProducto { get; set; }
        [Required]
        public int cantidad { get; set; }
        [Required]
        public float precioFinal { get; set; }
        [Required]
        public float precioProducto { get; set; }
        [Required]
        public string simboloMoneda { get; set; }


    }
}
