using Core.Entities;
using Core.Interfaces;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class ClienteRegistroRepository : GenericRepository<ClienteRegistro>, IClienteRegistroRepository
    {
        public ClienteRegistroRepository(AHDContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ClienteRegistro>> GetAllClientesProfesionalesAsync(string documentoProfesional)
        {
            return await _context.ClientesRegistrados
                            .Where(cr => cr.documentoProfesional == documentoProfesional && cr.documentoCliente != null)
                            .Include(cr => cr.cliente)
                            .ToListAsync();
        }

        public async Task<IEnumerable<ClienteRegistro>> GetAllEmpresasProfesionalesAsync(string documentoProfesional)
        {
            return await _context.ClientesRegistrados
                            .Where(cr => cr.documentoProfesional == documentoProfesional && cr.rucEmpresa != null)
                            .Include(cr => cr.empresa)
                            .ToListAsync();
        }

        public async Task<IEnumerable<ClienteRegistro>> GetAllClientesAfiliadosAsync(string rucFilial)
        {
            return await _context.ClientesRegistrados
                            .Where(cr => cr.rucFilial == rucFilial && cr.documentoCliente != null)
                            .Include(cr => cr.cliente)
                            .ToListAsync();
        }

        public async Task<IEnumerable<ClienteRegistro>> GetAllEmpresasAfiliadosAsync(string rucFilial)
        {
            return await _context.ClientesRegistrados
                            .Where(cr => cr.rucFilial == rucFilial && cr.rucEmpresa != null)
                            .Include(cr => cr.empresa)
                            .ToListAsync();
        }

        public async Task<ClienteRegistro> verificarRegistroClienteProfesional(string documentoProfesional, string documentoCliente)
        {
            return await _context.ClientesRegistrados
                            .SingleOrDefaultAsync(cr => cr.documentoProfesional == documentoProfesional && cr.documentoCliente == documentoCliente);
        }

        public async Task<ClienteRegistro> verificarRegistroClienteFilial(string rucFilial, string documentoCliente)
        {
            return await _context.ClientesRegistrados
                             .SingleOrDefaultAsync(cr => cr.rucFilial == rucFilial && cr.documentoCliente == documentoCliente);
        }

        public async Task<ClienteRegistro> verificarRegistroEmpresaProfesional(string documentoProfesional, string rucEmpresa)
        {
            return await _context.ClientesRegistrados
                            .SingleOrDefaultAsync(cr => cr.documentoProfesional == documentoProfesional && cr.rucEmpresa == rucEmpresa);
        }

        public async Task<ClienteRegistro> verificarRegistroEmpresaFilial(string rucFilial, string rucEmpresa)
        {
            return await _context.ClientesRegistrados
                             .SingleOrDefaultAsync(cr => cr.rucFilial == rucFilial && cr.rucEmpresa == rucEmpresa);
        }
    }
}
