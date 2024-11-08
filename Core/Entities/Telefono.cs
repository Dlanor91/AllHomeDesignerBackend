using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Telefono : BaseEntity
    {
        [Required]
        public string numero { get; set; }

        public string? documentoPersona { get; set; }
        public Persona? persona { get; set; }
        public string? rucEmpresa { get; set; }
        public Empresa? empresa { get; set; }
        public string? codigoSucursal { get; set; }
        public Sucursal? sucursal { get; set; }
    }
}
