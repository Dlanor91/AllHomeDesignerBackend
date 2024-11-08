namespace API.Dtos
{
    public class ClienteRegistroListDto
    {
        public int id { get; set; }
        public string? documentoCliente { get; set; }
        public string? rucEmpresa { get; set; }
        public string? documentoProfesional { get; set; }
        public string? rucFilial { get; set; }
    }
}
