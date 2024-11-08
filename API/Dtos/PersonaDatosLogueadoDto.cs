namespace API.Dtos
{
    public class PersonaDatosLogueadoDto
    {
        public string mensaje { get; set; }
        public bool estaAutenticado { get; set; }
        public string nombreCompleto { get; set; }
        public string nombreUsuario { get; set; }
        public string rol { get; set; }
        public string documentoProfesional { get; set; }
        public string codigoSucursal { get; set; }
        public string rucFilial { get; set; }
        public string token { get; set; }
    }
}
