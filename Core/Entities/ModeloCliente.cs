namespace Core.Entities
{
    public class ModeloCliente : BaseEntity
    {
        public int cantidad { get; set; }

        public string documentoCliente { get; set; }
        public Persona personaCliente { get; set; }
        public string codigoModelo { get; set; }
        public Modelo modelo { get; set; }
    }
}
