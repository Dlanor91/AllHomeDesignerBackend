using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;

namespace Infraestructura.Repositories
{
    public class TelefonoRepository : GenericRepository<Telefono>, ITelefonoRepository
    {
        public TelefonoRepository(AHDContext context) : base(context)
        {
        }
    }
}
