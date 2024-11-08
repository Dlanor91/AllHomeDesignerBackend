using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PersonaRegistroUpdateClienteDto
    {
        [Required]
        public string documento { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string apellido { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }

    }
}
