namespace API.Dtos
{
    public class FilialListDto
    {
        public int id { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string estado { get; set; }
        public ICollection<ClienteRegistroListDto>? clientesFiliales { get; set; }
        public ICollection<SucursalListDto>? sucursales { get; set; }
        public ICollection<ProductoDto>? productos { get; set; }
    }
}
