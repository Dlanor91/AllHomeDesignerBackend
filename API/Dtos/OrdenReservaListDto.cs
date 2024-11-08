namespace API.Dtos
{
    public class OrdenReservaListDto
    {
        public int id { get; set; }
        public int cantidad { get; set; }
        public float precioFinal { get; set; }
        public float precioProducto { get; set; }
        public string simboloMoneda { get; set; }
        public string? codigoReservaProductos { get; set; }

        public string productoCodigo { get; set; }
        public string nombreProducto { get; set; }
        public string? codigoModelo { get; set; }
    }
}
