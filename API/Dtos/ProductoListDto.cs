namespace API.Dtos
{
    public class ProductoListDto
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public float largo { get; set; }
        public float ancho { get; set; }
        public float profundidad { get; set; }
        public int stock { get; set; }
        public int disponibilidad { get; set; }
        public int reserva { get; set; }
        public string imagen { get; set; }
        public string presentacion { get; set; }
        public float rendimiento { get; set; }
        public string textura { get; set; }
        public string sugerencias { get; set; }
        public string rucProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public int idCategoria { get; set; }
        public string nombreCategoria { get; set; }
        public string rucFilial { get; set; }
        public string nombreFilial { get; set; }
        public PrecioDto precio { get; set; }
        public ICollection<OrdenReservaListDto>? ordenesCompraProducto { get; set; }


    }
}
