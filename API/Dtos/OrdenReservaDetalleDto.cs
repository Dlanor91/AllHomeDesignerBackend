namespace API.Dtos
{
    public class OrdenReservaDetalleDto
    {
        public string nombreFilial { get; set; }
        public string nombreProducto { get; set; }
        public string imagenProducto { get; set; }
        public float? rendimientoProducto { get; set; }
        public float precioProducto { get; set; }
        public float precioFinal { get; set; }
        public string simboloMoneda { get; set; }
        public int cantidad { get; set; }
    }
}
