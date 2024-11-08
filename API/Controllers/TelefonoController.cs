using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class TelefonoController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TelefonoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TelefonoListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var telefonos = await _unitOfWork.Telefonos
                            .GetAllAsync();
                return _mapper.Map<List<TelefonoListDto>>(telefonos);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TelefonoAddDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var telefono = await _unitOfWork.Telefonos
                            .GetByIdAsync(id);

                if (telefono == null)
                    return NotFound();

                return _mapper.Map<TelefonoAddDto>(telefono);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Telefono>> Post(TelefonoAddDto telefonoAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                Sucursal sucursalExiste = null;

                if (telefonoAddDto.documentoPersona != "")
                {
                    var existePersona = await _unitOfWork.Personas
                                       .GetByIdAsync(telefonoAddDto.documentoPersona);

                    if (existePersona == null)
                        return NotFound("El documento ingresado no se encuentra en nuestra base de datos.");
                }
                else
                    telefonoAddDto.documentoPersona = null;

                if (telefonoAddDto.rucEmpresa != "")
                {
                    var existeEmpresa = await _unitOfWork.Empresas
                                       .GetByIdAsync(telefonoAddDto.rucEmpresa);

                    if (existeEmpresa == null)
                        return NotFound("El ruc de empresa ingresado no se encuentra en nuestra base de datos.");
                }
                else
                    telefonoAddDto.rucEmpresa = null;

                if (telefonoAddDto.codigoSucursal != "")
                {
                    sucursalExiste = await _unitOfWork.Sucursales
                                                        .GetByIdAsync(telefonoAddDto.codigoSucursal);
                    if (sucursalExiste == null)
                        return NotFound("El codigo de sucursal ingresado no se encuentra en nuestra base de datos.");
                }
                else
                    telefonoAddDto.codigoSucursal = null;

                var telefono = _mapper.Map<Telefono>(telefonoAddDto);

                _unitOfWork.Telefonos.Add(telefono);
                await _unitOfWork.SaveAsync();

                if (telefono == null)
                {
                    return BadRequest();
                }

                if (sucursalExiste != null)
                {
                    sucursalExiste.idTelefono = telefono.id;
                    await _unitOfWork.SaveAsync();
                }

                telefonoAddDto.id = telefono.id;  
                return CreatedAtAction(nameof(Post), new { id = telefonoAddDto.id }, telefonoAddDto);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] TelefonoAddDto telefonoUpdateDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                if (telefonoUpdateDto == null)
                    return NotFound();

                var telefonoExiste = await _unitOfWork.Telefonos
                                .GetByIdAsync(id);
                if (telefonoExiste == null)
                    return BadRequest();

                telefonoExiste.numero = telefonoUpdateDto.numero;

                var telefono = _mapper.Map<Telefono>(telefonoExiste);

                _unitOfWork.Telefonos.Update(telefono);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var telefono = await _unitOfWork.Telefonos.GetByIdAsync(id);
                if (telefono == null)
                    return NotFound();

                _unitOfWork.Telefonos.Remove(telefono);
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
