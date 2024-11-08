using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class FilialAddDto
    {
        [Required]
        public string ruc { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string estado { get; set; }
    }
}
