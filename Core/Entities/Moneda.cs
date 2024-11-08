using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities
{
    public class Moneda : BaseEntity
    {
        [Key]
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public float cotizacion { get; set; }
        public DateTime fecha { get; set; } = GetUpdate();
        public string simbolo { get; set; }

        public ICollection<Precio> precios { get; set; }

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
