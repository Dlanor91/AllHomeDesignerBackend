namespace Core.Entities
{
    public class TipoUsuario : BaseEntity
    {
        public string rol { get; set; }
        public string descripcionRol { get; set; }

        public ICollection<Persona> personas { get; set; }
        public ICollection<Empresa> empresas { get; set; }
        public ICollection<TipoUsuarioPrivilegio>? tiposUsuariosPrivilegios { get; set; }
    }
}
