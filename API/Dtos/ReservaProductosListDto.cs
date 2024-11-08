namespace API.Dtos
{
    public class ReservaProductosListDto
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public DateTime fechaCompra { get; set; }

        public string documentoCliente { get; set; }
        public string nombreCliente { get; set; }
        public string rucEmpresa { get; set; }
        public string nombreEmpresa { get; set; }

        public ICollection<OrdenReservaListDto> ordenesReservas { get; set; }
    }
}
