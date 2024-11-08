using System.Globalization;

namespace API.Dtos
{
    public class MonedaUpdateDto
    {
        public float cotizacion { get; set; }
        public DateTime fecha { get; set; } = GetUpdate();

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
