namespace API.Dtos
{
    public class DireccionUpdateDTo
    {
        public int id { get; set; }
        public string calle { get; set; }
        public int nroPuerta { get; set; }
        public string? datos { get; set; }
        public string? complemento { get; set; }

        public int idLocalidad { get; set; }

    }
}
