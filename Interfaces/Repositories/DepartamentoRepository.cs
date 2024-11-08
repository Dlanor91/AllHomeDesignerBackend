using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class DepartamentoRepository : GenericRepository<Departamento>, IDepartamentoRepository
    {
        public DepartamentoRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            return await _context.Departamentos
                                .Include(d => d.localidades)
                                .ToListAsync();
        }
        public override async Task<Departamento> GetByIdAsync(int id)
        {
            return await _context.Departamentos
                            .Include(d => d.localidades)
                            .FirstOrDefaultAsync(d => d.id == id);
        }
    }
}
