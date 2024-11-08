using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class PrecioController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrecioController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PrecioListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var precios = await _unitOfWork.Precios
                                                .GetAllAsync();
                return _mapper.Map<List<PrecioListDto>>(precios);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("{codigoProducto}/{rucProveedor}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PrecioDto>> GetUltimoPrecioProducto(string codigoProducto, string rucProveedor)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {

                var codigoProductoExiste = await _unitOfWork.Productos
                                                            .BuscarProductoByCodigoByRucProveedorAsync(codigoProducto, rucProveedor);

                if (codigoProductoExiste == null)
                    return Conflict("Dicho producto ingresado no se encuentra en nuestra base de datos.");

                var ultimoPrecio = await _unitOfWork.Precios
                                                    .UltimoPrecioProductoAsync(codigoProducto, rucProveedor);

                return _mapper.Map<PrecioDto>(ultimoPrecio);
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
        public async Task<ActionResult<PrecioAddDto>> Post(PrecioAddDto precioAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                if (precioAddDto == null)
                    return BadRequest();

                Precio precioMostrar = null;
                if (precioAddDto.precioLista >= 0 && precioAddDto.precioVenta >= 0 && precioAddDto.iva >= 0)
                {
                    precioMostrar = _mapper.Map<Precio>(precioAddDto);
                    precioMostrar.precioFinal = (precioAddDto.precioVenta * precioAddDto.iva / 100) + precioAddDto.precioVenta;
                    precioMostrar.precioFinal = (float)Math.Round(precioMostrar.precioFinal, 2);
                }
                else
                    return BadRequest("Todos los precios y el IVA son mayores q 0.");

                var productoExiste = await _unitOfWork.Productos
                                        .BuscarProductoByCodigoByRucProveedorAsync(precioAddDto.codigoProducto, precioAddDto.rucProveedor);

                var monedaExiste = await _unitOfWork.Monedas
                                                    .GetByIdAsync(precioAddDto.codigoMoneda);

                if (productoExiste == null)
                    return Conflict("El producto no existe.");

                if (monedaExiste == null)
                    return Conflict("El codigo de moneda ingresado no se encuentra en nuestra base de datos.");

                _unitOfWork.Precios.Add(precioMostrar);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(Post), precioAddDto);
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}
