using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class PrecioRepository : GenericRepository<Precio>, IPrecioRepository
    {
        public PrecioRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Precio>> GetAllAsync()
        {
            return await _context.Precios
                                .Include(p => p.moneda)
                                .ToListAsync();
        }

        public async Task<Precio> UltimoPrecioProductoAsync(string codigoProducto, string rucProveedor)
        {
            return await _context.Precios
                                .Include(p => p.moneda)
                                .OrderByDescending(p => p.fecha)
                                .Where(p => p.codigoProducto == codigoProducto && p.rucProveedor == rucProveedor)
                                .FirstOrDefaultAsync();
        }
    }
}
