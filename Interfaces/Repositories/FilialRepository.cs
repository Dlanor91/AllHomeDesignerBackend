using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    internal class FilialRepository : GenericRepository<Filial>, IFilialRepository
    {
        public FilialRepository(AHDContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Filial>> GetAllAsync()
        {
            return await _context.Filial
                                .Include(f => f.clientesFiliales)
                                .Include(f => f.sucursales)
                                .Include(f => f.productos)
                                .ToListAsync();
        }
        public override async Task<Filial> GetByIdAsync(string ruc)
        {
            return await _context.Filial
                                .Include(f => f.clientesFiliales)
                                .Include(f => f.sucursales)
                                .Include(f => f.productos)
                                .FirstOrDefaultAsync(f => f.ruc == ruc);
        }
    }
}
