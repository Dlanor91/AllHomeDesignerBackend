namespace API.Dtos
{
    public class TipoUsuarioListDto
    {
        public int id { get; set; }
        public string rol { get; set; }
        public string descripcionRol { get; set; }
        public ICollection<TipoUsuarioPrivilegioListDto>? tiposUsuariosPrivilegios { get; set; }
        public ICollection<PersonaListDto>? personas { get; set; }
        public ICollection<EmpresaListDto> empresas { get; set; }
    }
}
