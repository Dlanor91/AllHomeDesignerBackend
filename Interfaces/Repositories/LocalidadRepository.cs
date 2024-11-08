using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;

namespace Infraestructura.Repositories
{
    public class LocalidadRepository : GenericRepository<Localidad>, ILocalidadRepository
    {
        public LocalidadRepository(AHDContext context) : base(context)
        {
        }
    }
}
