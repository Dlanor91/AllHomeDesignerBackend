namespace Core.Entities
{
    public class Localidad : BaseEntity
    {
        public string nombre { get; set; }

        public int idDepartamento { get; set; }
        public Departamento departamento { get; set; }

        public ICollection<Direccion> direcciones { get; set; }
    }
}
