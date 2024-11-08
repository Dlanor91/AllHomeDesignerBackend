using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities
{
    public class ReservaProductos : BaseEntity
    {
        [Key]
        public string codigo { get; set; }
        [Required]
        public DateTime fechaCompra { get; set; } = GetUpdate();

        public string? documentoCliente { get; set; }
        public Persona? cliente { get; set; }
        public string? documentoProfesional { get; set; }
        public Persona? profesional { get; set; }
        public string? rucEmpresa { get; set; }
        public Empresa? empresa { get; set; }
        public string? rucFilial { get; set; }
        public Filial? filial { get; set; }

        public ICollection<OrdenReserva> ordenesReservas { get; set; }

        private static DateTime GetUpdate()
        {
            DateTime now = DateTime.Now;
            string timeZoneId = "Montevideo Standard Time";
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime targetDateTime = TimeZoneInfo.ConvertTime(now, timeZone);
            string formattedDate = targetDateTime.ToString("yyyy-MM-dd HH:mm");
            return DateTime.ParseExact(formattedDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}
