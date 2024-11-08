using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class DireccionAddDto
    {
        public int id { get; set; }
        [Required]
        public string calle { get; set; }
        [Required]
        public int nroPuerta { get; set; }
        public string? datos { get; set; }
        public string? complemento { get; set; }
        [Required]
        public int idLocalidad { get; set; }

        public string? documentoPersona { get; set; }
        public string? rucEmpresa { get; set; }
        public string? codigoSucursal { get; set; }

    }
}
