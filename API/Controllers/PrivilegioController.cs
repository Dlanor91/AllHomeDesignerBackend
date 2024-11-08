using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class PrivilegioController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrivilegioController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PrivilegioListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var privilegios = await _unitOfWork.Privilegios
                            .GetAllAsync();
                return _mapper.Map<List<PrivilegioListDto>>(privilegios);
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
        public async Task<ActionResult<PrivilegioListDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var privilegioPorId = await _unitOfWork.Privilegios
                            .GetByIdAsync(id);

                if (privilegioPorId == null)
                    return NotFound();

                return _mapper.Map<PrivilegioListDto>(privilegioPorId);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Privilegio>> Post(PrivilegioAddUpdateDto privilegioAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                if (privilegioAddDto == null)
                    return BadRequest();

                var tipoExiste = _unitOfWork.Privilegios
                                            .Find(p => p.tipo.ToLower() == privilegioAddDto.tipo.ToLower().Replace(" ", ""))
                                            .FirstOrDefault();

                if (tipoExiste != null)
                    return BadRequest("El tipo de privilegio ingresado ya existe.");

                var privilegioIngresar = _mapper.Map<Privilegio>(privilegioAddDto);

                _unitOfWork.Privilegios.Add(privilegioIngresar);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(Post), new { id = privilegioIngresar.id }, privilegioAddDto);
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
        public async Task<ActionResult<PrivilegioAddUpdateDto>> Put(int id, [FromBody] PrivilegioAddUpdateDto privilegioUpdateDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                if (privilegioUpdateDto == null)
                    return BadRequest();

                Privilegio privilegioExiste = await _unitOfWork.Privilegios
                                                        .GetByIdAsync(id);

                if (privilegioExiste == null)
                    return NotFound();

                if (privilegioUpdateDto.tipo == privilegioExiste.tipo)
                {
                    privilegioExiste.descripcion = privilegioUpdateDto.descripcion;

                    await _unitOfWork.SaveAsync();
                }
                else if (privilegioUpdateDto.tipo != privilegioExiste.tipo)
                {
                    var nuevoTipoExiste = _unitOfWork.Privilegios
                                            .Find(tu => tu.tipo == privilegioUpdateDto.tipo)
                                            .FirstOrDefault();
                    if (nuevoTipoExiste == null)
                    {
                        privilegioExiste.tipo = privilegioUpdateDto.tipo;
                        privilegioExiste.descripcion = privilegioUpdateDto.descripcion;
                        await _unitOfWork.SaveAsync();
                        return privilegioUpdateDto;
                    }
                    else
                        return Conflict("El rol ya esta ingresado en nuestra base de datos.");
                }

                return privilegioUpdateDto;
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
                var privilegioBuscar = await _unitOfWork.Privilegios.GetByIdAsync(id);
                if (privilegioBuscar == null)
                    return NotFound();

                var privilegioTipoUsuario = privilegioBuscar.privilegiosTiposUsuarios.ToList();

                if (privilegioTipoUsuario.Count > 0)
                {
                    return Conflict("No se puede eliminar el privilegio, tiene tipos de usuarios asociados");
                }

                _unitOfWork.Privilegios.Remove(privilegioBuscar);
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
