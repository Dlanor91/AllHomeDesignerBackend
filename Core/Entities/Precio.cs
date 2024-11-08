using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities
{
    public class Precio : BaseEntity
    {
        [Required]
        public float precioLista { get; set; }
        [Required]
        public float precioVenta { get; set; }
        [Required]
        public float iva { get; set; }
        [Required]
        public float precioFinal { get; set; }
        [Required]
        public DateTime fecha { get; set; } = GetUpdate();
        [Required]
        public string codigoMoneda { get; set; }
        public Moneda moneda { get; set; }
        [Required]
        public string rucProveedor { get; set; }
        [Required]
        public string codigoProducto { get; set; }
        public Producto producto { get; set; }

        private static DateTime GetUpdate()
        {
            DateTime now = DateTime.Now;
            string timeZoneId = "Montevideo Standard Time";
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime targetDateTime = TimeZoneInfo.ConvertTime(now, timeZone);
            string formattedDate = targetDateTime.ToString("yyyy-MM-dd");
            return DateTime.ParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
