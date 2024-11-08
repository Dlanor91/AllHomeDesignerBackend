using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class TelefonoAddDto
    {
        public int id { get; set; }
        [Required]
        public string numero { get; set; }
        public string? documentoPersona { get; set; }
        public string? rucEmpresa { get; set; }
        public string? codigoSucursal { get; set; }

    }
}
