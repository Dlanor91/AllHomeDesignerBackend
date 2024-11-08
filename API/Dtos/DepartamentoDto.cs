namespace API.Dtos
{
    public class DepartamentoDto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public ICollection<LocalidadDto> localidades { get; set; }

    }
}
