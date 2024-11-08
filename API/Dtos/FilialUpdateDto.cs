using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class FilialUpdateDto
    {
        [Required]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string? estado { get; set; }
    }
}
