namespace API.Dtos
{
    public class CategoriaByIdDto
    {
        public int id { get; set; }
        public string nombre { get; set; }

        public string descripcion { get; set; }

        public ICollection<ProductoListDto> productos { get; set; }
    }
}
