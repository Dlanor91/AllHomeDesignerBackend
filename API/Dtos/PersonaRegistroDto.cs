using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PersonaRegistroDto
    {
        [Required]
        public string documento { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string apellido { get; set; }
        [Required]
        public string nombreUsuario { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public int idTipoUsuario { get; set; }
    }
}
