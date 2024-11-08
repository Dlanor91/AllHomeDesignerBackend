using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class DireccionController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DireccionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DireccionListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var direcciones = await _unitOfWork.Direcciones
                            .GetAllAsync();

                if (direcciones.Count() == 0)
                    return NoContent();

                return _mapper.Map<List<DireccionListDto>>(direcciones);
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
        public async Task<ActionResult<DireccionListDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var direccion = await _unitOfWork.Direcciones
                            .GetByIdAsync(id);

                if (direccion == null)
                    return NotFound();

                return _mapper.Map<DireccionListDto>(direccion);
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
        public async Task<ActionResult<Direccion>> Post(DireccionAddDto direccionAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                Sucursal sucursalExiste = null;

                if (direccionAddDto.documentoPersona != null)
                {
                    var existePersona = await _unitOfWork.Personas
                                       .GetByIdAsync(direccionAddDto.documentoPersona);

                    if (existePersona == null)
                        return NotFound();
                }

                if (direccionAddDto.rucEmpresa != null)
                {
                    var existeEmpresa = await _unitOfWork.Empresas
                                       .GetByIdAsync(direccionAddDto.rucEmpresa);

                    if (existeEmpresa == null)
                        return NotFound();
                }

                var localidadExiste = await _unitOfWork.Localidades
                                        .GetByIdAsync(direccionAddDto.idLocalidad);

                if (localidadExiste == null)
                    return NotFound();

                var departamento = await _unitOfWork.Departamentos
                                    .GetByIdAsync(localidadExiste.idDepartamento);

                var direccion = _mapper.Map<Direccion>(direccionAddDto);
                direccion.departamento = departamento.nombre;

                if (direccionAddDto.codigoSucursal != null)
                {
                    sucursalExiste = await _unitOfWork.Sucursales
                                                        .GetByIdAsync(direccionAddDto.codigoSucursal);
                    if (sucursalExiste == null)
                        return NotFound("El codigo de sucursal ingresado no se encuentra en nuestra base de datos.");
                }

                _unitOfWork.Direcciones.Add(direccion);
                await _unitOfWork.SaveAsync();

                if (direccion == null)
                {
                    return BadRequest();
                }

                if (sucursalExiste != null)
                    sucursalExiste.idDireccion = direccion.id;

                direccionAddDto.id = direccion.id;

                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(Post), new { id = direccionAddDto.id }, direccionAddDto);
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
        public async Task<ActionResult> Put(int id, [FromBody] DireccionUpdateDTo direccionUpdateDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                if (direccionUpdateDto == null)
                    return NotFound();

                var direccionExiste = await _unitOfWork.Direcciones
                                .GetByIdAsync(id);
                if (direccionExiste == null)
                    return BadRequest();

                var localidadExiste = await _unitOfWork.Localidades
                                        .GetByIdAsync(direccionUpdateDto.idLocalidad);
                if (localidadExiste == null)
                    return BadRequest();
                var departamento = await _unitOfWork.Departamentos
                                    .GetByIdAsync(localidadExiste.idDepartamento);

                direccionExiste.calle = direccionUpdateDto.calle;
                direccionExiste.nroPuerta = direccionUpdateDto.nroPuerta;
                direccionExiste.datos = direccionUpdateDto.datos;
                direccionExiste.complemento = direccionUpdateDto.complemento;
                direccionExiste.idLocalidad = direccionUpdateDto.idLocalidad;
                direccionExiste.departamento = departamento.nombre;

                _unitOfWork.Direcciones.Update(direccionExiste);
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
                var direccion = await _unitOfWork.Direcciones.GetByIdAsync(id);
                if (direccion == null)
                    return NotFound();

                _unitOfWork.Direcciones.Remove(direccion);
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
