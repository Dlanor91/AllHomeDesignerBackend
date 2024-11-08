using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class ReservaProductosRepository : GenericRepository<ReservaProductos>, IReservaProductos
    {
        public ReservaProductosRepository(AHDContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<ReservaProductos>> GetAllAsync()
        {
            return await _context.ReservasProductos
                                .Include(rp => rp.cliente)
                                .Include(rp => rp.empresa)
                                .Include(rp => rp.ordenesReservas)
                                    .ThenInclude(oc => oc.producto)
                                .ToListAsync();
        }
        public override async Task<ReservaProductos> GetByIdAsync(string codigo)
        {
            return await _context.ReservasProductos
                                .Include(rp => rp.cliente)
                                .Include(rp => rp.empresa)
                                .Include(rp => rp.ordenesReservas)
                                    .ThenInclude(oc => oc.producto)
                                .FirstOrDefaultAsync(rp => rp.codigo == codigo);
        }

        public async Task<IEnumerable<ReservaProductos>> GetReservasByDocumentoCliente(string documentoCliente, string documentoProfesional, string rucFilial)
        {
            return await _context.ReservasProductos
                                .Where(rp => rp.documentoCliente == documentoCliente && (rp.rucFilial == rucFilial || rp.documentoProfesional == documentoProfesional))
                                .Include(rp => rp.cliente)
                                .Include(rp => rp.empresa)
                                .Include(rp => rp.ordenesReservas)
                                    .ThenInclude(oc => oc.producto)
                                .ToListAsync();
        }

        public async Task<IEnumerable<ReservaProductos>> GetReservasByRucEmpresa(string rucEmpresa, string documentoProfesional, string rucFilial)
        {
            return await _context.ReservasProductos
                                .Where(rp => rp.rucEmpresa == rucEmpresa && (rp.rucFilial == rucFilial || rp.documentoProfesional == documentoProfesional))
                                .Include(rp => rp.cliente)
                                .Include(rp => rp.empresa)
                                .Include(rp => rp.ordenesReservas)
                                    .ThenInclude(oc => oc.producto)
                                .ToListAsync();
        }

        public async Task<IEnumerable<ReservaProductos>> GetReservasByDocumentoProfesional(string documentoProfesional)
        {
            return await _context.ReservasProductos
                                .Where(rp => rp.documentoProfesional == documentoProfesional)
                                .Include(rp => rp.cliente)
                                .Include(rp => rp.empresa)
                                .Include(rp => rp.ordenesReservas)
                                    .ThenInclude(oc => oc.producto)
                                .ToListAsync();
        }

        public async Task<IEnumerable<ReservaProductos>> GetReservasByRucFilial(string rucFilial)
        {
            return await _context.ReservasProductos
                                .Where(rp => rp.rucFilial == rucFilial)
                                .Include(rp => rp.cliente)
                                .Include(rp => rp.empresa)
                                .Include(rp => rp.ordenesReservas)
                                    .ThenInclude(oc => oc.producto)
                                .ToListAsync();
        }
    }
}
