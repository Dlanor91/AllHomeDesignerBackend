using Core.Entities;

namespace Core.Interfaces
{
    public interface ITipoUsuarioPrivilegioRepository : IGenericRepository<TipoUsuarioPrivilegio>
    {

        Task<TipoUsuarioPrivilegio> GetByIdTipoUsuario(int idTipoUsuario);
    }
}
