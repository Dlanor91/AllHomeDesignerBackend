namespace API.Dtos
{
    public class ProveedorListDto
    {
        public int id { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }
        public ICollection<ProductoDto> productos { get; set; }
    }
}
