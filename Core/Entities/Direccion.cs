namespace Core.Entities
{
    public class Direccion : BaseEntity
    {
        public string calle { get; set; }
        public int nroPuerta { get; set; }
        public string? datos { get; set; }
        public string? complemento { get; set; }
        public string departamento { get; set; }
        public int idLocalidad { get; set; }
        public Localidad localidad { get; set; }

        public string? documentoPersona { get; set; }
        public Persona? persona { get; set; }
        public string? rucEmpresa { get; set; }
        public Empresa? empresa { get; set; }
        public string? codigoSucursal { get; set; }
        public Sucursal? sucursal { get; set; }

    }
}
