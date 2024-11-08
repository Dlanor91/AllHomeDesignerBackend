using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities
{
    public class Filial : BaseEntity
    {
        [Key]
        public string ruc { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public DateTime fechaRegistro { get; set; } = GetUpdate();
        public string? estado { get; set; }

        public ICollection<ClienteRegistro>? clientesFiliales { get; set; }
        public ICollection<Sucursal>? sucursales { get; set; }
        public ICollection<Producto>? productos { get; set; }
        public ICollection<ReservaProductos>? reservaProductos { get; set; }

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
