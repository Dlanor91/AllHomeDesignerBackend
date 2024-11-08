using Core.Entities;

namespace Core.Interfaces
{
    public interface IOrdenReservaRepository : IGenericRepository<OrdenReserva>
    {
        Task<IEnumerable<OrdenReserva>> GetAllDetallesCompra(string codigoReservaProducto);
    }
}
