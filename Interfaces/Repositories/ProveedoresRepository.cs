using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class ProveedoresRepository : GenericRepository<Proveedor>, IProveedorRepository
    {
        public ProveedoresRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            return await _context.Proveedores
                                .Include(p => p.productos)
                                .ToListAsync();
        }
        public override async Task<Proveedor> GetByIdAsync(string ruc)
        {
            return await _context.Proveedores
                                .Include(p => p.productos)
                                .FirstOrDefaultAsync(p => p.ruc == ruc);
        }
    }
}
