using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ProveedorUpdateDto
    {
        [Required]
        public string nombre { get; set; }
    }
}
