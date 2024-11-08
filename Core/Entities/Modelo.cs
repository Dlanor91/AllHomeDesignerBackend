using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities
{
    public class Modelo : BaseEntity
    {
        [Key]
        public string codigo { get; set; }
        public DateTime fecha { get; set; } = GetUpdate();

        public string? personaUsuario { get; set; }
        public Persona usuario { get; set; }
        public string? personaCliente { get; set; }
        public Persona cliente { get; set; }

        public ICollection<ModeloCliente> modelosCodigosGenerados { get; set; }
        public ICollection<OrdenReserva> ordenesReservaModelo { get; set; }

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
