using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(AHDContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                                .Include(p => p.categoria)
                                .Include(p => p.filial)
                                .Include(p => p.proveedor)
                                .Include(p => p.precios)
                                    .ThenInclude(pr => pr.moneda)
                                .OrderBy(p => p.categoria)
                                .ToListAsync();
        }
        public async Task<Producto> BuscarProductoByCodigoByRucProveedorAsync(string codigo, string rucProveedor)
        {
            return await _context.Productos
                            .Include(p => p.categoria)
                            .Include(p => p.filial)
                            .Include(p => p.proveedor)
                            .Include(p => p.precios)
                                .ThenInclude(pr => pr.moneda)
                            .Include(p => p.ordenesReservaProducto)
                            .FirstOrDefaultAsync(p => p.codigo == codigo && p.rucProveedor == rucProveedor);
        }

        public async Task<List<Producto>> BuscarProductosByRucFilial(string rucFilial)
        {
            return await _context.Productos
                            .Where(p => p.rucFilial == rucFilial)
                            .Include(p => p.categoria)
                            .Include(p => p.filial)
                            .Include(p => p.proveedor)
                            .OrderBy(p => p.categoria)
                            .ToListAsync();
        }
    }
}
