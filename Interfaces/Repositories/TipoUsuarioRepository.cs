using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class TipoUsuarioRepository : GenericRepository<TipoUsuario>, ITipoUsuarioRepository
    {
        public TipoUsuarioRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<TipoUsuario>> GetAllAsync()
        {
            return await _context.TiposUsuarios
                                .OrderBy(tu => tu.rol)
                                .Include(tu => tu.personas)
                                .Include(tu => tu.empresas)
                                .Include(tup => tup.tiposUsuariosPrivilegios)
                                    .ThenInclude(ptu => ptu.privilegioTipoUsuario)
                                .ToListAsync();
        }
        public override async Task<TipoUsuario> GetByIdAsync(int id)
        {
            return await _context.TiposUsuarios
                                .Include(p => p.personas)
                                .Include(tu => tu.empresas)
                                .Include(tup => tup.tiposUsuariosPrivilegios)
                                    .ThenInclude(ptu => ptu.privilegioTipoUsuario)
                                .FirstOrDefaultAsync(p => p.id == id);
        }
    }
}
