using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class SucursalRepository : GenericRepository<Sucursal>, ISucursalRepository
    {
        public SucursalRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Sucursal>> GetAllAsync()
        {
            return await _context.Sucursal
                                .Include(s => s.trabajadores)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Sucursal>> GetAllByRucFilialAsync(string rucFilial)
        {
            return await _context.Sucursal
                                .Where(s => s.rucFilial == rucFilial)
                                .Include(s => s.trabajadores)
                                .ToListAsync();
        }

        public override async Task<Sucursal> GetByIdAsync(string codigo)
        {
            return await _context.Sucursal
                                .Include(s => s.trabajadores)
                                .FirstOrDefaultAsync(s => s.codigo == codigo);
        }
    }
}
