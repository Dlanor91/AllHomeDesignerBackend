namespace API.Dtos
{
    public class ReservaProductosVentasDto
    {
        public string codigo { get; set; }
        public DateTime fechaCompra { get; set; }

        public string documentoCliente { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string rucEmpresa { get; set; }
        public string nombreEmpresa { get; set; }
        public string razonSocialEmpresa { get; set; }
        public float totalPesos { get; set; }
        public float totalDolares { get; set; }
    }
}
