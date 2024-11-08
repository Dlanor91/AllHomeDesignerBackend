using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class CategoriaController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var categorias = await _unitOfWork.Categorias
                                                    .GetAllAsync();
                return _mapper.Map<List<CategoriaDto>>(categorias);
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
        public async Task<ActionResult<CategoriaByIdDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var categoria = await _unitOfWork.Categorias
                                                    .GetByIdAsync(id);

                if (categoria == null)
                    return NotFound();

                return _mapper.Map<CategoriaByIdDto>(categoria);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Categoria>> Post(CategoriaDto categoriaDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (categoriaDto == null)
                    return BadRequest();

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _unitOfWork.Categorias.Add(categoria);
                await _unitOfWork.SaveAsync();

                categoriaDto.id = categoria.id;
                return CreatedAtAction(nameof(Post), new { id = categoriaDto.id }, categoriaDto);
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
        public async Task<ActionResult<CategoriaDto>> Put(int id, [FromBody] CategoriaDto categoriaDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (categoriaDto == null)
                    return BadRequest();

                var categoriaExiste = await _unitOfWork.Categorias
                                                        .GetByIdAsync(id);

                if (categoriaExiste == null)
                    return NotFound();

                categoriaExiste.nombre = categoriaDto.nombre;
                categoriaExiste.descripcion = categoriaDto.descripcion;

                await _unitOfWork.SaveAsync();

                return categoriaDto;
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
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var categoria = await _unitOfWork.Categorias
                                                    .GetByIdAsync(id);
                if (categoria == null)
                    return NotFound();

                var productos = categoria.productos.ToList();

                if (productos.Count() > 0)
                    return Conflict("Tiene productos asociados, elimínelos para eliminar la categoría.");

                _unitOfWork.Categorias.Remove(categoria);
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
