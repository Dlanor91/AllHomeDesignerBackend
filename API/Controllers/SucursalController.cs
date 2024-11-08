using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class SucursalController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SucursalController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<SucursalListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var sucursales = await _unitOfWork.Sucursales
                                                  .GetAllAsync();

                return _mapper.Map<List<SucursalListDto>>(sucursales);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SucursalListDto>> Get(string codigo)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var sucursal = await _unitOfWork.Sucursales
                                                .GetByIdAsync(codigo);

                if (sucursal == null)
                    return NotFound();

                return _mapper.Map<SucursalListDto>(sucursal);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("GetAllByRuc/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SucursalListDto>>> GetAllByRucFilial(string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Gerente"))
            {
                var existeFilial = _unitOfWork.Filiales
                                                 .Find(f => f.ruc == rucFilial.Replace(" ", ""))
                                                 .FirstOrDefault();

                if (existeFilial == null)
                    return NotFound("No existe la filial ingresada.");

                var sucursal = await _unitOfWork.Sucursales
                                                .GetAllByRucFilialAsync(rucFilial);

                if (sucursal == null)
                    return NotFound();

                return _mapper.Map<List<SucursalListDto>>(sucursal);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Sucursal>> Post(string rucFilial, SucursalAddDto sucursalAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (sucursalAddDto == null)
                    return BadRequest();

                if (string.IsNullOrEmpty(rucFilial))
                    return Conflict("El ruc de filial no puede ser ni vacío ni null.");

                var existeFilial = _unitOfWork.Filiales
                                                .Find(f => f.ruc == rucFilial.Replace(" ", ""))
                                                .FirstOrDefault();

                if (existeFilial == null)
                    return NotFound("No existe la filial ingresada.");

                var existeSucursal = _unitOfWork.Sucursales
                                                .Find(s => s.codigo == sucursalAddDto.codigo.Replace(" ", ""))
                                                .FirstOrDefault();

                if (existeSucursal != null)
                    return Conflict("Ese código de sucursal ya existe en nuestra base de datos.");

                var existeEmail = _unitOfWork.Sucursales
                                                .Find(s => s.email.ToUpper() == sucursalAddDto.email.Replace(" ", "").ToUpper())
                                                .FirstOrDefault();
                if (existeEmail != null)
                    return Conflict("Ese email de sucursal ya existe en nuestra base de datos.");

                var sucursal = _mapper.Map<Sucursal>(sucursalAddDto);
                sucursal.rucFilial = rucFilial;
                sucursal.codigo = sucursal.codigo.ToUpper();

                _unitOfWork.Sucursales.Add(sucursal);
                await _unitOfWork.SaveAsync();

                sucursalAddDto.codigo = sucursal.codigo;
                return CreatedAtAction(nameof(Post), new { codigo = sucursalAddDto.codigo }, sucursalAddDto);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPut("{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SucursalUpdateDto>> Put(string codigo, [FromBody] SucursalUpdateDto sucursalUpdateDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (sucursalUpdateDto == null)
                    return BadRequest();

                var sucursalExiste = await _unitOfWork.Sucursales
                                                        .GetByIdAsync(codigo);

                if (sucursalExiste == null)
                    return NotFound();

                if (sucursalExiste.email != sucursalUpdateDto.email)
                {
                    var emailExiste = _unitOfWork.Sucursales
                                                 .Find(s => s.email == sucursalUpdateDto.email)
                                                 .FirstOrDefault();

                    if (emailExiste != null)
                        return BadRequest("Dicho email ya fue ingresado en otra sucursal en nuestra base de datos.");

                    sucursalExiste.email = sucursalUpdateDto.email;
                }

                sucursalExiste.nombre = sucursalUpdateDto.nombre;
                sucursalExiste.detalles = sucursalUpdateDto.detalles;
                sucursalExiste.idDireccion = sucursalUpdateDto.idDireccion;
                sucursalExiste.idTelefono = sucursalUpdateDto.idTelefono;

                await _unitOfWork.SaveAsync();

                return sucursalUpdateDto;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{codigo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string codigo)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var sucursal = await _unitOfWork.Sucursales.GetByIdAsync(codigo);
                if (sucursal == null)
                    return NotFound();

                var trabajadores = sucursal.trabajadores.ToList();

                if (trabajadores.Count() > 0)
                    return Conflict("Esta sucursal contiene trabajadores, no se puede eliminar.");

                if (sucursal.idDireccion > 0 && sucursal.idDireccion != null)
                {
                    Direccion direccionEliminar = await _unitOfWork.Direcciones.GetByIdAsync((int)sucursal.idDireccion);

                    if (direccionEliminar != null)
                    {
                        _unitOfWork.Direcciones.Remove(direccionEliminar);
                        await _unitOfWork.SaveAsync();
                    }
                }

                if (sucursal.idTelefono > 0 && sucursal.idTelefono != null)
                {
                    Telefono telefonoAEliminar = await _unitOfWork.Telefonos.GetByIdAsync((int)sucursal.idTelefono);

                    if (telefonoAEliminar != null)
                    {
                        _unitOfWork.Telefonos.Remove(telefonoAEliminar);
                        await _unitOfWork.SaveAsync();
                    }
                }

                _unitOfWork.Sucursales.Remove(sucursal);
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
