using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class PersonaRepository : GenericRepository<Persona>, IPersonaRepository
    {
        public PersonaRepository(AHDContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas
                                .Include(p => p.tipoUsuario)
                                .Include(p => p.telefonos)
                                .Include(p => p.direcciones)
                                    .ThenInclude(d => d.localidad)
                                .Include(p => p.clientesProfesionales)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Persona>> GetAllUsuarios()
        {
            return await _context.Personas
                                .Where(p => p.tipoUsuario.rol == "Gerente" || p.tipoUsuario.rol == "Administrador" || p.tipoUsuario.rol == "Profesional")
                                .Include(p => p.tipoUsuario)
                                .Include(p => p.telefonos)
                                .Include(p => p.direcciones)
                                    .ThenInclude(d => d.localidad)
                                .Include(p => p.clientesProfesionales)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Persona>> GetAllTrabajadoresSucursal(string codigoSucursal)
        {
            return await _context.Personas
                                .Where(p => p.codigoSucursal == codigoSucursal)
                                .Include(p => p.tipoUsuario)
                                .Include(p => p.telefonos)
                                .Include(p => p.direcciones)
                                    .ThenInclude(d => d.localidad)
                                .Include(p => p.clientesProfesionales)
                                .ToListAsync();
        }

        public override async Task<Persona> GetByIdAsync(string documento)
        {
            return await _context.Personas
                            .Include(p => p.tipoUsuario)
                            .Include(p => p.telefonos)
                            .Include(p => p.direcciones)
                                .ThenInclude(d => d.localidad)
                            .Include(p => p.clientesProfesionales)
                            .FirstOrDefaultAsync(p => p.documento == documento);
        }

        public async Task<Persona> GetByUserNameAsync(string nombreUsuario)
        {
            return await _context.Personas
                            .Include(p => p.tipoUsuario)
                            .FirstOrDefaultAsync(p => (p.nombreUsuario.ToLower() == nombreUsuario.ToLower()) || (p.email.ToLower() == nombreUsuario.ToLower()));
        }
    }
}
