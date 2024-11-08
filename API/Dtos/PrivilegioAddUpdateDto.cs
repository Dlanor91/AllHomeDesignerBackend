using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PrivilegioAddUpdateDto
    {
        [Required]
        public string tipo { get; set; }
        [Required]
        public string descripcion { get; set; }
    }
}
