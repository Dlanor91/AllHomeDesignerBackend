using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class ProveedorController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProveedorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var proveedores = await _unitOfWork.Proveedores
                            .GetAllAsync();
                return _mapper.Map<List<ProveedorDto>>(proveedores);
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
        public async Task<ActionResult<ProveedorListDto>> Get(string ruc)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var proveedor = await _unitOfWork.Proveedores
                            .GetByIdAsync(ruc);

                if (proveedor == null)
                    return NotFound();

                return _mapper.Map<ProveedorListDto>(proveedor);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Proveedor>> Post(ProveedorDto proveedorDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var proveedor = _mapper.Map<Proveedor>(proveedorDto);

                _unitOfWork.Proveedores.Add(proveedor);
                await _unitOfWork.SaveAsync();

                if (proveedor == null)
                {
                    return BadRequest();
                }

                proveedorDto.ruc = proveedor.ruc;
                return CreatedAtAction(nameof(Post), new { ruc = proveedorDto.ruc }, proveedorDto);
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
        public async Task<ActionResult<Proveedor>> Put(string ruc, [FromBody] ProveedorUpdateDto proveedorUpdateDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (proveedorUpdateDto == null)
                    return NotFound();

                var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(ruc);

                if (proveedor == null)
                    return BadRequest();

                proveedor.nombre = proveedorUpdateDto.nombre;

                await _unitOfWork.SaveAsync();

                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{ruc}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string ruc)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var proveedor = await _unitOfWork.Proveedores.GetByIdAsync(ruc);
                if (proveedor == null)
                    return NotFound();

                var existenProductos = proveedor.productos.ToList();

                if (existenProductos.Count() > 0)
                    return Conflict("El Proveedor que desea eliminar tiene productos asociados.");

                _unitOfWork.Proveedores.Remove(proveedor);
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
