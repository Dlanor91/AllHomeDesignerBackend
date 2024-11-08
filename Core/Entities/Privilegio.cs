namespace Core.Entities
{
    public class Privilegio : BaseEntity
    {
        public string tipo { get; set; }
        public string descripcion { get; set; }

        public ICollection<TipoUsuarioPrivilegio>? privilegiosTiposUsuarios { get; set; }
    }
}
