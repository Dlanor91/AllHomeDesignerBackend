using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PersonaUpdateDto
    {
        [Required]
        public string nombre { get; set; }
        [Required]
        public string apellido { get; set; }
        public string nombreUsuario { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
        [Required]
        public int idTipoUsuario { get; set; }
    }
}
