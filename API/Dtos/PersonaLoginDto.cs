using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PersonaLoginDto
    {
        [Required]
        public string nombreUsuario { get; set; }
        [Required]
        public string password { get; set; }
    }
}
