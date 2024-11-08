using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class TipoUsuarioPrivilegioController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoUsuarioPrivilegioController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TipoUsuarioPrivilegioListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var tiposUsuariosPrivilegios = await _unitOfWork.TiposUsuariosPrivilegios
                                                                .GetAllAsync();
                return _mapper.Map<List<TipoUsuarioPrivilegioListDto>>(tiposUsuariosPrivilegios);
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
        public async Task<ActionResult<TipoUsuarioPrivilegioListDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var tipoUsuarioPrivilegio = await _unitOfWork.TiposUsuariosPrivilegios
                                                                .GetByIdAsync(id);

                if (tipoUsuarioPrivilegio == null)
                    return NotFound();

                return _mapper.Map<TipoUsuarioPrivilegioListDto>(tipoUsuarioPrivilegio);
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
        public async Task<ActionResult<TipoUsuarioPrivilegio>> Post(TipoUsuarioPrivilegioAddUpdateDto tipoUsuarioPrivilegioAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                if (tipoUsuarioPrivilegioAddDto == null)
                    return BadRequest();

                var existeTipoUsuario = _unitOfWork.TiposUsuarios
                                                    .Find(tu => tu.id == tipoUsuarioPrivilegioAddDto.idTipoUsuario)
                                                    .FirstOrDefault();

                if (existeTipoUsuario == null)
                    return NotFound("No existe ese tipo de Usuario en nuestra base de datos");

                var existePrivilegio = _unitOfWork.Privilegios
                                                    .Find(p => p.id == tipoUsuarioPrivilegioAddDto.idPrivilegio)
                                                    .FirstOrDefault();

                if (existePrivilegio == null)
                    return NotFound("No existe ese privilegio en nuestra base de datos");

                var existeRelacion = _unitOfWork.TiposUsuariosPrivilegios
                                                .Find(tup => tup.idTipoUsuario == tipoUsuarioPrivilegioAddDto.idTipoUsuario && tup.idPrivilegio == tipoUsuarioPrivilegioAddDto.idPrivilegio)
                                                .FirstOrDefault();

                if (existeRelacion != null)
                    return BadRequest("Ya este privilegio y este tipo de usuario están relacionados");

                var yaIngresado = _unitOfWork.TiposUsuariosPrivilegios
                                              .Find(tup => tup.idTipoUsuario == tipoUsuarioPrivilegioAddDto.idTipoUsuario)
                                              .FirstOrDefault();

                if (yaIngresado != null)
                    return Conflict("Ya este usuario posee un privilegio asociado.");

                var tipoUsuarioPrivilegioAdd = _mapper.Map<TipoUsuarioPrivilegio>(tipoUsuarioPrivilegioAddDto);

                _unitOfWork.TiposUsuariosPrivilegios.Add(tipoUsuarioPrivilegioAdd);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(Post), new { id = tipoUsuarioPrivilegioAdd.id }, tipoUsuarioPrivilegioAddDto);
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
            if (privilegio.Contains("Superadmin"))
            {
                var tipoUsuarioPrivilegio = await _unitOfWork.TiposUsuariosPrivilegios.GetByIdAsync(id);

                if (tipoUsuarioPrivilegio == null)
                    return NotFound();

                _unitOfWork.TiposUsuariosPrivilegios.Remove(tipoUsuarioPrivilegio);
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
