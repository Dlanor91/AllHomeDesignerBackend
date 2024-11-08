using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class FilialController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FilialController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FilialListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var filiales = await _unitOfWork.Filiales
                            .GetAllAsync();
                return _mapper.Map<List<FilialListDto>>(filiales);
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
        public async Task<ActionResult<FilialListDto>> Get(string ruc)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var filial = await _unitOfWork.Filiales
                            .GetByIdAsync(ruc);

                if (filial == null)
                    return NotFound();

                return _mapper.Map<FilialListDto>(filial);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Filial>> Post(FilialAddDto filialAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                if (filialAddDto == null)
                    return BadRequest();

                var filialUnica = await _unitOfWork.Filiales
                                        .GetByIdAsync(filialAddDto.ruc);

                if (filialUnica != null)
                    return BadRequest("Filial ya registrado en nuestra base de datos.");

                var emailExiste = _unitOfWork.Filiales
                                                 .Find(f => f.email == filialAddDto.email)
                                                 .FirstOrDefault();

                if (emailExiste != null)
                    return BadRequest("Dicho email ya fue ingresado en otra filial en nuestra base de datos.");

                var filial = _mapper.Map<Filial>(filialAddDto);

                _unitOfWork.Filiales.Add(filial);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(Post), new { id = filial.id }, filialAddDto);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPut("{ruc}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<FilialUpdateDto>> Put(string ruc, [FromBody] FilialUpdateDto filialUpdateDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (filialUpdateDto == null)
                    return BadRequest();

                var filialExiste = await _unitOfWork.Filiales
                                                    .GetByIdAsync(ruc);

                if (filialExiste == null)
                    return NotFound();

                if (filialExiste.email != filialUpdateDto.email)
                {
                    var emailExiste = _unitOfWork.Filiales
                                                 .Find(f => f.email == filialUpdateDto.email)
                                                 .FirstOrDefault();

                    if (emailExiste != null)
                        return Conflict("Dicho email ya fue ingresado en otra filial en nuestra base de datos.");

                    filialExiste.email = filialUpdateDto.email;
                }

                filialExiste.nombre = filialUpdateDto.nombre;
                filialExiste.estado = filialUpdateDto.estado;

                await _unitOfWork.SaveAsync();

                return filialUpdateDto;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{ruc}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(string ruc)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var filial = await _unitOfWork.Filiales.GetByIdAsync(ruc);
                if (filial == null)
                    return NotFound();

                var sucursales = filial.sucursales.ToList();

                if (sucursales.Count() > 0)
                {
                    return Conflict("Esta filial tiene sucursales asociadas no se puede eliminar.");
                }

                _unitOfWork.Filiales.Remove(filial);
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
