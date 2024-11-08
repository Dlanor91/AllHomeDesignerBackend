using Core.Entities;

namespace Core.Interfaces
{
    public interface IPrecioRepository : IGenericRepository<Precio>
    {
        Task<Precio> UltimoPrecioProductoAsync(string codigoProducto, string rucProveedor);
    }
}
