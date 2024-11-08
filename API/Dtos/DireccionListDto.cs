namespace API.Dtos
{
    public class DireccionListDto
    {
        public int id { get; set; }
        public string calle { get; set; }
        public int nroPuerta { get; set; }
        public string datos { get; set; }
        public string complemento { get; set; }
        public string departamento { get; set; }
        public int idLocalidad { get; set; }
        public string nombreLocalidad { get; set; }

        public string? documentoPersona { get; set; }
        public string? rucEmpresa { get; set; }
        public string? codigoSucursal { get; set; }
    }
}
