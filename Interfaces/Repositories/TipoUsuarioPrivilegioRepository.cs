using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class TipoUsuarioPrivilegioRepository : GenericRepository<TipoUsuarioPrivilegio>, ITipoUsuarioPrivilegioRepository
    {
        public TipoUsuarioPrivilegioRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<TipoUsuarioPrivilegio>> GetAllAsync()
        {
            return await _context.TiposUsuariosPrivilegios
                                .OrderBy(tup => tup.tipoUsuarioPrivilegios.rol)
                                .Include(tup => tup.privilegioTipoUsuario)
                                .Include(tup => tup.tipoUsuarioPrivilegios)
                                .ToListAsync();
        }

        public override async Task<TipoUsuarioPrivilegio> GetByIdAsync(int id)
        {
            return await _context.TiposUsuariosPrivilegios
                                .Include(tup => tup.privilegioTipoUsuario)
                                .Include(tup => tup.tipoUsuarioPrivilegios)
                                .FirstOrDefaultAsync(tup => tup.id == id);
        }

        public async Task<TipoUsuarioPrivilegio> GetByIdTipoUsuario(int idTipoUsuario)
        {
            return await _context.TiposUsuariosPrivilegios
                                    .OrderBy(tup => tup.idPrivilegio)
                                    .LastOrDefaultAsync(tup => tup.idTipoUsuario == idTipoUsuario);
        }
    }
}
