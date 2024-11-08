using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class SucursalUpdateDto
    {
        [Required]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string? detalles { get; set; }
        public int? idTelefono { get; set; }
        public int? idDireccion { get; set; }
    }
}
