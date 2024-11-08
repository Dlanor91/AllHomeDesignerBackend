using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        Task<Producto> BuscarProductoByCodigoByRucProveedorAsync(string codigo, string rucProveedor);
        Task<List<Producto>> BuscarProductosByRucFilial(string rucFilial);
    }
}
