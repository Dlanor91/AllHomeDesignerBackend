namespace API.Dtos
{
    public class MonedaListDto
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public float cotizacion { get; set; }
        public DateTime fecha { get; set; }
        public string simbolo { get; set; }
        public ICollection<PrecioAddDto>? precios { get; set; }

    }
}
