namespace Core.Entities
{
    public class Categoria : BaseEntity
    {
        public string nombre { get; set; }

        public string descripcion { get; set; }

        public ICollection<Producto> productos { get; set; }
    }
}
