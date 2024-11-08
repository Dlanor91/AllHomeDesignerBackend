namespace API.Dtos
{
    public class PersonaPerfilDto
    {
        public string documento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombreUsuario { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int idTipoUsuario { get; set; }
        public string rol { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
    }
}
