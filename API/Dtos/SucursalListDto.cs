namespace API.Dtos
{
    public class SucursalListDto
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }

        public string email { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string? detalles { get; set; }

        public int? idTelefono { get; set; }
        public int? idDireccion { get; set; }
        public string rucFilial { get; set; }

        public ICollection<PersonaListDto> trabajadores { get; set; }
    }
}
