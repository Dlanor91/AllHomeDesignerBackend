namespace Core.Entities
{
    public class Departamento : BaseEntity
    {
        public string nombre { get; set; }

        public ICollection<Localidad> localidades { get; set; }
    }
}
