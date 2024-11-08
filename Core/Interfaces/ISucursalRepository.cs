using Core.Entities;

namespace Core.Interfaces
{
    public interface ISucursalRepository : IGenericRepository<Sucursal>
    {
        Task<IEnumerable<Sucursal>> GetAllByRucFilialAsync(string rucFilial);
    }
}
