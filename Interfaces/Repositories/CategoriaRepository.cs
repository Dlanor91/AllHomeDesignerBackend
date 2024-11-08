using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AHDContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _context.Categorias
                                .OrderBy(c => c.nombre)
                                .ToListAsync();
        }

        public override async Task<Categoria> GetByIdAsync(int id)
        {
            return await _context.Categorias
                            .Include(c => c.productos)
                                    .ThenInclude(p => p.precios)
                            .FirstOrDefaultAsync(c => c.id == id);
        }
    }
}
