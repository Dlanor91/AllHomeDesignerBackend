using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CategoriaDto
    {
        public int id { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string descripcion { get; set; }

    }
}
