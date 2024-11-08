using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EmpresaAddUpdateDto
    {

        public string? nombre { get; set; }
        [Required]
        public string ruc { get; set; }
        [Required]
        public string razonSocial { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string? comentarios { get; set; }
    }
}
