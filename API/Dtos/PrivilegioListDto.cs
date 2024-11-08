namespace API.Dtos
{
    public class PrivilegioListDto
    {
        public int Id { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public ICollection<TipoUsuarioPrivilegioListDto>? privilegiosTiposUsuarios { get; set; }
    }
}
