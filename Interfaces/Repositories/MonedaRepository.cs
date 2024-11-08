using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class MonedaRepository : GenericRepository<Moneda>, IMonedaRepository
    {
        public MonedaRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Moneda>> GetAllAsync()
        {
            return await _context.Monedas
                                .Include(m => m.precios)
                                .ToListAsync();
        }
        public override async Task<Moneda> GetByIdAsync(string codigo)
        {
            return await _context.Monedas
                            .Include(m => m.precios)
                            .FirstOrDefaultAsync(cp => cp.codigo == codigo);
        }
    }
}
