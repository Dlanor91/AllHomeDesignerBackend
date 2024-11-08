using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class TipoUsuarioController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoUsuarioController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TipoUsuarioDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var tiposUsuarios = await _unitOfWork.TiposUsuarios
                            .GetAllAsync();
                return _mapper.Map<List<TipoUsuarioDto>>(tiposUsuarios);
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
        public async Task<ActionResult<TipoUsuarioListDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var tipoUsuario = await _unitOfWork.TiposUsuarios
                            .GetByIdAsync(id);

                if (tipoUsuario == null)
                    return NotFound();

                return _mapper.Map<TipoUsuarioListDto>(tipoUsuario);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoUsuario>> Post(TipoUsuarioDto tipoUsuarioDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var tipoUsuario = _mapper.Map<TipoUsuario>(tipoUsuarioDto);

                _unitOfWork.TiposUsuarios.Add(tipoUsuario);
                await _unitOfWork.SaveAsync();

                if (tipoUsuario == null)
                {
                    return BadRequest();
                }

                tipoUsuarioDto.id = tipoUsuario.id;
                return CreatedAtAction(nameof(Post), new { id = tipoUsuarioDto.id }, tipoUsuarioDto);
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
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TipoUsuarioDto>> Put(int id, [FromBody] TipoUsuarioDto tipoUsuarioDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                if (tipoUsuarioDto == null)
                    return BadRequest();

                TipoUsuario existeRol = await _unitOfWork.TiposUsuarios
                                                        .GetByIdAsync(id);
                if (existeRol == null)
                    return NotFound();

                if (tipoUsuarioDto.rol == existeRol.rol)
                {
                    existeRol.descripcionRol = tipoUsuarioDto.descripcionRol;

                    await _unitOfWork.SaveAsync();
                }
                else if (tipoUsuarioDto.rol != existeRol.rol)
                {
                    var nuevoRolExiste = _unitOfWork.TiposUsuarios
                                            .Find(tu => tu.rol == tipoUsuarioDto.rol)
                                            .FirstOrDefault();
                    if (nuevoRolExiste == null)
                    {
                        existeRol.rol = tipoUsuarioDto.rol;
                        existeRol.descripcionRol = tipoUsuarioDto.descripcionRol;
                        await _unitOfWork.SaveAsync();
                        return tipoUsuarioDto;
                    }
                    else
                        return Conflict("El rol ya esta ingresado en nuestra base de datos.");
                }
                return tipoUsuarioDto;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var tipoUsuario = await _unitOfWork.TiposUsuarios.GetByIdAsync(id);
                if (tipoUsuario == null)
                    return NotFound();

                var existenUsuarios = tipoUsuario.personas.ToList();

                if (existenUsuarios.Count() > 0)
                {
                    return Conflict("El tipo de persona que desea eliminar tiene personas asociadas.");
                }
                else
                {
                    var privilegioTipoUsuario = tipoUsuario.tiposUsuariosPrivilegios.ToList();

                    if (privilegioTipoUsuario.Count > 0)
                    {
                        _unitOfWork.TiposUsuariosPrivilegios.RemoveRange(privilegioTipoUsuario);
                    }
                }

                _unitOfWork.TiposUsuarios.Remove(tipoUsuario);
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
