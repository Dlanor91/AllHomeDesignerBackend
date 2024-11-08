using Core.Entities;

namespace Core.Interfaces
{
    public interface IClienteRegistroRepository : IGenericRepository<ClienteRegistro>
    {
        Task<IEnumerable<ClienteRegistro>> GetAllClientesProfesionalesAsync(string documentoProfesional);
        Task<IEnumerable<ClienteRegistro>> GetAllEmpresasProfesionalesAsync(string documentoProfesional);
        Task<IEnumerable<ClienteRegistro>> GetAllClientesAfiliadosAsync(string rucFilial);
        Task<IEnumerable<ClienteRegistro>> GetAllEmpresasAfiliadosAsync(string rucFilial);

        Task<ClienteRegistro> verificarRegistroClienteProfesional(string documentoProfesional, string documentoCliente);
        Task<ClienteRegistro> verificarRegistroClienteFilial(string rucFilial, string documentoCliente);

        Task<ClienteRegistro> verificarRegistroEmpresaProfesional(string documentoProfesional, string rucEmpresa);
        Task<ClienteRegistro> verificarRegistroEmpresaFilial(string rucFilial, string rucEmpresa);
    }
}
