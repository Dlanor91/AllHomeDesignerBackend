using API.Dtos;
using Core.Entities;

namespace API.Services
{
    public interface IEmpresaService
    {
        Task<DatosAddUpdateDto> RegisterEmpresaAsync(EmpresaAddUpdateDto model, string rucFilial, string docProfesional);
        Task<DatosAddUpdateDto> UpdateEmpresaAsync(Empresa empresaExiste, EmpresaAddUpdateDto model);
    }
}
