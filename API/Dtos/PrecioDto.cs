namespace API.Dtos
{
    public class PrecioDto
    {
        public float precioLista { get; set; }
        public float precioVenta { get; set; }
        public float precioFinal { get; set; }
        public float iva { get; set; }
        public DateTime fecha { get; set; }
        public string codigoMoneda { get; set; }
        public string simbolo { get; set; }
    }
}
