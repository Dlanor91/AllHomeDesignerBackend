namespace Core.Entities
{
    public class TipoUsuarioPrivilegio : BaseEntity
    {
        public int idTipoUsuario { get; set; }
        public TipoUsuario tipoUsuarioPrivilegios { get; set; }
        public int idPrivilegio { get; set; }
        public Privilegio privilegioTipoUsuario { get; set; }
    }
}
