using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class PrivilegioRepository : GenericRepository<Privilegio>, IPrivilegioRepository
    {
        public PrivilegioRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Privilegio>> GetAllAsync()
        {
            return await _context.Privilegios
                                .OrderBy(p => p.tipo)
                                .Include(tup => tup.privilegiosTiposUsuarios)
                                    .ThenInclude(ptu => ptu.tipoUsuarioPrivilegios)
                                .ToListAsync();
        }
        public override async Task<Privilegio> GetByIdAsync(int id)
        {
            return await _context.Privilegios
                            .Include(tup => tup.privilegiosTiposUsuarios)
                                .ThenInclude(ptu => ptu.tipoUsuarioPrivilegios)
                            .FirstOrDefaultAsync(tup => tup.id == id);
        }
    }
}
