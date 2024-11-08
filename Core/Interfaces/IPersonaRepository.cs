using Core.Entities;

namespace Core.Interfaces
{
    public interface IPersonaRepository : IGenericRepository<Persona>
    {
        Task<Persona> GetByUserNameAsync(string nombreUsuario);
        Task<IEnumerable<Persona>> GetAllUsuarios();
        Task<IEnumerable<Persona>> GetAllTrabajadoresSucursal(string codigoSucursal);
    }
}
