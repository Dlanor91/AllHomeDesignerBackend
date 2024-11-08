using Core.Entities;

namespace Core.Interfaces
{
    public interface IReservaProductos : IGenericRepository<ReservaProductos>
    {
        Task<IEnumerable<ReservaProductos>> GetReservasByDocumentoCliente(string documentoCliente, string documentoProfesional, string rucFilial);
        Task<IEnumerable<ReservaProductos>> GetReservasByRucEmpresa(string rucEmpresa, string documentoProfesional, string rucFilial);
        Task<IEnumerable<ReservaProductos>> GetReservasByRucFilial(string rucFilial);
        Task<IEnumerable<ReservaProductos>> GetReservasByDocumentoProfesional(string documentoProfesional);
    }
}
