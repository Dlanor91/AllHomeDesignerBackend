using API.Dtos;
using Core.Entities;

namespace API.Services
{
    public interface IUserService
    {
        Task<DatosAddUpdateDto> RegisterAsync(PersonaRegistroDto model, string codigoSucursal);
        Task<DatosAddUpdateDto> RegisterTrabajadorAsync(PersonaRegistroDto model, string codigoSucursal);
        Task<DatosAddUpdateDto> RegisterClienteAsync(PersonaRegistroUpdateClienteDto model, string rucFilial, string docProfesional);
        Task<PersonaDatosLogueadoDto> GetLoginTokenAsync(PersonaLoginDto model);
        Task<DatosAddUpdateDto> UpdateAsync(Persona personaExiste, PersonaUpdateDto model);
        Task<DatosAddUpdateDto> UpdateClienteAsync(Persona clienteExiste, PersonaRegistroUpdateClienteDto model);
    }
}
