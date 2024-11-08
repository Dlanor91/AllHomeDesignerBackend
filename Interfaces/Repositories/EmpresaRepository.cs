using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class EmpresaRepository : GenericRepository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Empresa>> GetAllAsync()
        {
            return await _context.Empresas
                                .Include(e => e.tipoUsuario)
                                .Include(e => e.telefonos)
                                .Include(p => p.reservaProductos)
                                .Include(e => e.direcciones)
                                    .ThenInclude(e => e.localidad)
                                .ToListAsync();
        }
        public override async Task<Empresa> GetByIdAsync(string ruc)
        {
            return await _context.Empresas
                                .Include(e => e.tipoUsuario)
                                .Include(e => e.telefonos)
                                .Include(p => p.reservaProductos)
                                .Include(e => e.direcciones)
                                    .ThenInclude(e => e.localidad)
                                .FirstOrDefaultAsync(e => e.ruc == ruc);
        }
    }
}
