namespace API.Dtos
{
    public class PrecioListDto
    {
        public float precioLista { get; set; }
        public float precioVenta { get; set; }
        public float iva { get; set; }
        public float precioFinal { get; set; }
        public DateTime fecha { get; set; }
        public MonedaDto moneda { get; set; }
    }
}
