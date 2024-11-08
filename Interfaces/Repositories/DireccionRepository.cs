using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class DireccionRepository : GenericRepository<Direccion>, IDireccionRepository
    {
        public DireccionRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Direccion>> GetAllAsync()
        {
            return await _context.Direcciones
                                .Include(d => d.localidad)
                                .ToListAsync();
        }
        public override async Task<Direccion> GetByIdAsync(int id)
        {
            return await _context.Direcciones
                                .Include(d => d.localidad)
                                .FirstOrDefaultAsync(d => d.id == id);
        }
    }
}
