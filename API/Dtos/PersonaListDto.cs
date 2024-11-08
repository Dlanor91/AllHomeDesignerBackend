namespace API.Dtos
{
    public class PersonaListDto
    {
        public int id { get; set; }
        public string documento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombreUsuario { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int idTipoUsuario { get; set; }
        public string rol { get; set; }
        public string? codigoSucursal { get; set; }
        public ICollection<TelefonoListDto>? telefonos { get; set; }
        public ICollection<DireccionDto>? direcciones { get; set; }
        public ICollection<ReservaProductosDto>? reservasProductos { get; set; }
        public ICollection<ClienteRegistroListDto>? clientesProfesionales { get; set; }

    }
}
