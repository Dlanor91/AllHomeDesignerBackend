using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class MonedaController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MonedaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MonedaDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var monedas = await _unitOfWork.Monedas
                            .GetAllAsync();
                return _mapper.Map<List<MonedaDto>>(monedas);
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
        public async Task<ActionResult<MonedaListDto>> Get(string codigo)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var codigoValido = codigo.ToUpper().Replace(" ", "");

                var moneda = await _unitOfWork.Monedas
                                            .GetByIdAsync(codigoValido);

                if (moneda == null)
                    return NotFound();

                return _mapper.Map<MonedaListDto>(moneda);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Moneda>> Post(MonedaAddDto monedaAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (monedaAddDto == null)
                    return BadRequest();

                var monedaExiste = await _unitOfWork.Monedas
                                        .GetByIdAsync(monedaAddDto.codigo);
                if (monedaExiste != null)
                    return Conflict("Ese código de moneda ya existe en nuestra base de datos.");

                monedaAddDto.codigo = monedaAddDto.codigo.ToUpper().Replace(" ", "");

                var moneda = _mapper.Map<Moneda>(monedaAddDto);

                _unitOfWork.Monedas.Add(moneda);
                await _unitOfWork.SaveAsync();

                return moneda;
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
        public async Task<ActionResult<MonedaUpdateDto>> Put(string codigo, [FromBody] MonedaUpdateDto monedaUpdDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (monedaUpdDto == null)
                    return BadRequest();

                var codigoValido = codigo.ToUpper().Replace(" ", "");

                var monedaExiste = await _unitOfWork.Monedas
                                            .GetByIdAsync(codigoValido);

                if (monedaExiste == null)
                    return NotFound();

                monedaExiste.cotizacion = monedaUpdDto.cotizacion;
                monedaExiste.fecha = monedaUpdDto.fecha;

                await _unitOfWork.SaveAsync();

                return monedaUpdDto;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{codigo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(string codigo)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                codigo = codigo.ToUpper().Replace(" ", "");
                var moneda = await _unitOfWork.Monedas.GetByIdAsync(codigo);

                if (moneda == null)
                    return NotFound();

                var preciosMoneda = moneda.precios.ToList();

                if (preciosMoneda.Count() > 0)
                {
                    return Conflict("Tiene precios asociados, no se puede eliminar esta moneda.");
                }

                _unitOfWork.Monedas.Remove(moneda);
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
