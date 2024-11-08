using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;

namespace Infraestructura.Repositories
{
    public class MensajeRepository : GenericRepository<Mensaje>, IMensajeRepository
    {
        public MensajeRepository(AHDContext context) : base(context)
        {
        }
    }
}
