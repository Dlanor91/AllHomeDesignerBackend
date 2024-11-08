using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class OrdenReservaRepository : GenericRepository<OrdenReserva>, IOrdenReservaRepository
    {
        public OrdenReservaRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<OrdenReserva>> GetAllAsync()
        {
            return await _context.OrdenesReservas
                                .Include(or => or.producto)
                                .ToListAsync();
        }

        public override async Task<OrdenReserva> GetByIdAsync(int id)
        {
            return await _context.OrdenesReservas
                                .Include(or => or.producto)
                                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<IEnumerable<OrdenReserva>> GetAllDetallesCompra(string codigoReservaProducto)
        {
            return await _context.OrdenesReservas
                                .Where(or => or.codigoReservaProducto == codigoReservaProducto)
                                .Include(or => or.producto)
                                    .ThenInclude(p => p.precios)
                                .ToListAsync();
        }
    }
}
