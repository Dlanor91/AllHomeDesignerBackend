using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class SucursalAddDto
    {
        [Required]
        public string codigo { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string? detalles { get; set; }

    }
}
