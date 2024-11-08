using API.Dtos;
using API.Services;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class EmpresaController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmpresaService _empresaService;

        public EmpresaController(IUnitOfWork unitOfWork, IMapper mapper, IEmpresaService empresaService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _empresaService = empresaService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<EmpresaListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var empresas = await _unitOfWork.Empresas
                .GetAllAsync();
                return _mapper.Map<List<EmpresaListDto>>(empresas);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("{ruc}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpresaListDto>> Get(string ruc)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var empresa = await _unitOfWork.Empresas
                            .GetByIdAsync(ruc);

                if (empresa == null)
                    return NotFound();

                return _mapper.Map<EmpresaListDto>(empresa);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("{rucFilial}/{docProfesional}")]
        public async Task<IActionResult> RegisterEmpresa(EmpresaAddUpdateDto model, string rucFilial, string docProfesional)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var result = await _empresaService.RegisterEmpresaAsync(model, rucFilial, docProfesional);
                return Ok(result);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPut("{ruc}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateEmpresaAsync(string ruc, [FromBody] EmpresaAddUpdateDto model)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                if (model == null)
                    return BadRequest();

                var empresaExiste = await _unitOfWork.Empresas
                                                .GetByIdAsync(ruc.Replace(" ", ""));
                if (empresaExiste == null)
                    return NotFound("La empresa no existe");

                var result = await _empresaService.UpdateEmpresaAsync(empresaExiste, model);
                return Ok(result);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{ruc}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string ruc)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (!privilegio.Contains("Basico"))
            {
                var empresa = await _unitOfWork.Empresas.GetByIdAsync(ruc);
                if (empresa == null)
                    return NotFound();

                var direcciones = empresa.direcciones.ToList();
                var telefonos = empresa.telefonos.ToList();

                if (direcciones.Count() > 0)
                    _unitOfWork.Direcciones.RemoveRange(direcciones);

                if (telefonos.Count() > 0)
                    _unitOfWork.Telefonos.RemoveRange(telefonos);

                var empresasRegistradas = _unitOfWork.ClientesRegistrados
                                                      .Find(cr => cr.rucEmpresa == ruc)
                                                      .ToList();
                var reservasProductos = _unitOfWork.ReservasProductos
                                                   .Find(rp => rp.rucEmpresa == ruc)
                                                   .ToList();
                if (reservasProductos.Count() > 0)
                    return Conflict("Tiene reservas asociadas, eliminelas primero.");

                if (empresasRegistradas.Count() > 0)
                    _unitOfWork.ClientesRegistrados.RemoveRange(empresasRegistradas);

                _unitOfWork.Empresas.Remove(empresa);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}
