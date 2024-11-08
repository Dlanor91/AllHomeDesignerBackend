namespace Core.Entities
{
    public class ClienteRegistro : BaseEntity
    {
        public string? documentoCliente { get; set; }
        public Persona? cliente { get; set; }
        public string? rucEmpresa { get; set; }
        public Empresa? empresa { get; set; }
        public string? documentoProfesional { get; set; }
        public string? rucFilial { get; set; }
        public Filial? filial { get; set; }
    }
}
