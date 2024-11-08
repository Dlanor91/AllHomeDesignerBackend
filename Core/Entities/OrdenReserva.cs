namespace Core.Entities
{
    public class OrdenReserva : BaseEntity
    {
        public int cantidad { get; set; }
        public float precioFinal { get; set; }
        public float precioProducto { get; set; }
        public string simboloMoneda { get; set; }

        public string? codigoReservaProducto { get; set; }
        public ReservaProductos reservaProductos { get; set; }

        public string? rucProveedor { get; set; }
        public string? codigoProducto { get; set; }
        public Producto? producto { get; set; }
        public string? codigoModelo { get; set; }
        public Modelo? modelo { get; set; }
    }
}
