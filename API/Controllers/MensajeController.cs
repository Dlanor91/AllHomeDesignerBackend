using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class MensajeController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MensajeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MensajeListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var mensajes = await _unitOfWork.Mensajes
                                                .GetAllAsync();

                return _mapper.Map<List<MensajeListDto>>(mensajes);
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
        public async Task<ActionResult<MensajeListDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var mensaje = await _unitOfWork.Mensajes
                                               .GetByIdAsync(id);

                if (mensaje == null)
                    return NotFound();

                return _mapper.Map<MensajeListDto>(mensaje);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MensajeAddDto>> Post(MensajeAddDto mensajeDto)
        {
            if (mensajeDto == null)
                return BadRequest();

            var mensaje = _mapper.Map<Mensaje>(mensajeDto);

            _unitOfWork.Mensajes.Add(mensaje);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(Post), new { id = mensaje.id }, mensajeDto);
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
                var mensaje = await _unitOfWork.Mensajes
                                               .GetByIdAsync(id);
                if (mensaje == null)
                    return NotFound();

                _unitOfWork.Mensajes.Remove(mensaje);
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
