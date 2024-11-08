namespace API.Dtos
{
    public class EmpresaListDto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string? ruc { get; set; }
        public string? razonSocial { get; set; }
        public string email { get; set; }
        public int idTipoUsuario { get; set; }
        public string rol { get; set; }
        public string? comentarios { get; set; }
        public ICollection<TelefonoListDto>? telefonos { get; set; }
        public ICollection<DireccionDto>? direcciones { get; set; }
        public ICollection<ReservaProductosDto>? reservasProductos { get; set; }
    }
}
