using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Mensaje : BaseEntity
    {
        [Required]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string telefono { get; set; }
        [Required]
        public string mensaje { get; set; }
    }
}
