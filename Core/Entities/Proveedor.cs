using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Proveedor : BaseEntity
    {
        [Key]
        public string ruc { get; set; }
        public string nombre { get; set; }

        //Agrego la relacion 1 a muchos
        public ICollection<Producto> productos { get; set; }
    }
}
