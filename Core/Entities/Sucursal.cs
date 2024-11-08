using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities
{
    public class Sucursal : BaseEntity
    {
        [Key]
        public string codigo { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public DateTime fechaRegistro { get; set; } = GetUpdate();
        public string? detalles { get; set; }

        public int? idTelefono { get; set; }
        public Telefono? telefono { get; set; }
        public int? idDireccion { get; set; }
        public Direccion? direccion { get; set; }
        public string rucFilial { get; set; }
        public Filial filial { get; set; }

        public ICollection<Persona>? trabajadores { get; set; }

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
