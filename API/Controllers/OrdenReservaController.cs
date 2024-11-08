using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class OrdenReservaController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdenReservaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrdenReservaListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var ordenesReservas = await _unitOfWork.OrdenesReservas
                                                        .GetAllAsync();
                if (ordenesReservas == null)
                    return NoContent();

                return _mapper.Map<List<OrdenReservaListDto>>(ordenesReservas);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("detallesCompra/{codigoReservaProducto}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrdenReservaDetalleDto>>> GetAllDetallesCompra(string codigoReservaProducto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var existeCompra = await _unitOfWork.ReservasProductos
                                                    .GetByIdAsync(codigoReservaProducto.ToUpper());

                if (existeCompra == null)
                    return NotFound();

                var ordenesReservas = await _unitOfWork.OrdenesReservas
                                                            .GetAllDetallesCompra(codigoReservaProducto);
                if (ordenesReservas == null)
                    return NoContent();

                List<OrdenReservaDetalleDto> mostrarOrdenes = new List<OrdenReservaDetalleDto>();

                foreach (OrdenReserva unaOrden in ordenesReservas)
                {
                    Filial datosFilial = await _unitOfWork.Filiales
                                                            .GetByIdAsync(unaOrden.producto.rucFilial);

                    if (unaOrden.producto.imagen != null)
                    {
                        string nombreArchivo = unaOrden.producto.imagen;
                        unaOrden.producto.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/{nombreArchivo}";
                    }
                    else
                    {
                        unaOrden.producto.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/noImagen.jpg";
                    }

                    OrdenReservaDetalleDto nueva = new OrdenReservaDetalleDto
                    {
                        cantidad = unaOrden.cantidad,
                        precioFinal = unaOrden.precioFinal,
                        rendimientoProducto = unaOrden.producto.rendimiento,
                        nombreProducto = unaOrden.producto.nombre,
                        imagenProducto = unaOrden.producto.imagen,
                        nombreFilial = datosFilial.nombre,
                        precioProducto = unaOrden.precioProducto,
                        simboloMoneda = unaOrden.simboloMoneda,
                    };
                    mostrarOrdenes.Add(nueva);
                }

                return mostrarOrdenes;
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
        public async Task<ActionResult<OrdenReservaListDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var ordenReserva = await _unitOfWork.OrdenesReservas
                                                    .GetByIdAsync(id);

                if (ordenReserva == null)
                    return NotFound();

                return _mapper.Map<OrdenReservaListDto>(ordenReserva);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("reservaProductosCliente/{documentoCliente}/{rucFilial}/{documentoProfesional}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PostMultipleReservaCliente(string documentoCliente, string rucFilial, string documentoProfesional, List<OrdenReservaAddDto> ordenesReservaAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var existeProducto = new Producto();
                var mensaje = "";
                ReservaProductos nuevaReserva = null;
                if (documentoCliente != null)
                {
                    var existeCliente = await _unitOfWork.Personas
                                                        .GetByIdAsync(documentoCliente);
                    if (existeCliente == null)
                    {
                        return NotFound("No existe el cliente.");
                    }
                }

                if (rucFilial != "null")
                {
                    var existeFilial = await _unitOfWork.Filiales
                                                        .GetByIdAsync(rucFilial);
                    if (existeFilial == null)
                    {
                        return NotFound("No existe la filial ingresada.");
                    }
                }
                else
                    rucFilial = null;

                if (documentoProfesional != "null")
                {
                    var existeProfesional = await _unitOfWork.Personas
                                                        .GetByIdAsync(documentoProfesional);
                    if (existeProfesional == null)
                    {
                        return NotFound("No existe el profesional ingresado.");
                    }
                }
                else
                    documentoProfesional = null;

                var reservaProductos = await _unitOfWork.ReservasProductos
                                                        .GetAllAsync();
                int nuevoId = 1;
                if (reservaProductos.Count() > 0)
                {
                    int ultimoId = reservaProductos.Max(u => u.id);
                    nuevoId += ultimoId;
                }

                nuevaReserva = new ReservaProductos()
                {
                    codigo = "CP" + nuevoId,
                    documentoCliente = documentoCliente,
                    rucEmpresa = null,
                    rucFilial = rucFilial,
                    documentoProfesional = documentoProfesional,
                };
                _unitOfWork.ReservasProductos.Add(nuevaReserva);
                await _unitOfWork.SaveAsync();

                List<OrdenReserva> reservas = new List<OrdenReserva>();

                foreach (var ordenReservaAddDto in ordenesReservaAddDto)
                {
                    existeProducto = await _unitOfWork.Productos
                                                        .BuscarProductoByCodigoByRucProveedorAsync(ordenReservaAddDto.codigoProducto, ordenReservaAddDto.rucProveedor);

                    var ordenReserva = _mapper.Map<OrdenReserva>(ordenReservaAddDto);
                    ordenReserva.codigoReservaProducto = nuevaReserva.codigo;
                    ordenReserva.codigoModelo = null;

                    if (existeProducto.disponibilidad >= ordenReservaAddDto.cantidad)
                    {
                        existeProducto.disponibilidad -= ordenReservaAddDto.cantidad;
                        existeProducto.reserva += ordenReservaAddDto.cantidad;
                        reservas.Add(ordenReserva);
                    }
                    else
                    {
                        mensaje += $"Codigo: {existeProducto.codigo} sin disponibilidad. </br>";
                    }
                }

                _unitOfWork.OrdenesReservas.AddRange(reservas);
                await _unitOfWork.SaveAsync();
                return Ok(mensaje);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("reservaProductos/Empresa/{rucEmpresa}/{rucFilial}/{documentoProfesional}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PostMultipleReservaEmpresa(string rucEmpresa, string rucFilial, string documentoProfesional, List<OrdenReservaAddDto> ordenesReservaAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var existeProducto = new Producto();
                var mensaje = "";
                ReservaProductos nuevaReserva = null;
                if (rucEmpresa != null)
                {
                    var existeCliente = await _unitOfWork.Empresas
                                                        .GetByIdAsync(rucEmpresa);
                    if (existeCliente == null)
                    {
                        return NotFound();
                    }
                }

                if (rucFilial != "null")
                {
                    var existeFilial = await _unitOfWork.Filiales
                                                        .GetByIdAsync(rucFilial);
                    if (existeFilial == null)
                    {
                        return NotFound("No existe la filial ingresada.");
                    }
                }
                else
                    rucFilial = null;

                if (documentoProfesional != "null")
                {
                    var existeProfesional = await _unitOfWork.Personas
                                                        .GetByIdAsync(documentoProfesional);
                    if (existeProfesional == null)
                    {
                        return NotFound("No existe el profesional ingresado.");
                    }
                }
                else
                    documentoProfesional = null;

                var reservaProductos = await _unitOfWork.ReservasProductos
                                                            .GetAllAsync();
                int nuevoId = 1;
                if (reservaProductos.Count() > 0)
                {
                    int ultimoId = reservaProductos.Max(u => u.id);
                    nuevoId += ultimoId;
                }

                nuevaReserva = new ReservaProductos()
                {
                    codigo = "CP" + nuevoId,
                    documentoCliente = null,
                    rucEmpresa = rucEmpresa,
                    rucFilial = rucFilial,
                    documentoProfesional = documentoProfesional,
                };
                _unitOfWork.ReservasProductos.Add(nuevaReserva);
                await _unitOfWork.SaveAsync();

                List<OrdenReserva> reservas = new List<OrdenReserva>();

                foreach (var ordenReservaAddDto in ordenesReservaAddDto)
                {
                    existeProducto = await _unitOfWork.Productos
                                                        .BuscarProductoByCodigoByRucProveedorAsync(ordenReservaAddDto.codigoProducto, ordenReservaAddDto.rucProveedor);

                    var ordenReserva = _mapper.Map<OrdenReserva>(ordenReservaAddDto);
                    ordenReserva.codigoReservaProducto = nuevaReserva.codigo;
                    ordenReserva.codigoModelo = null;

                    if (existeProducto.disponibilidad >= ordenReservaAddDto.cantidad)
                    {
                        existeProducto.disponibilidad -= ordenReservaAddDto.cantidad;
                        existeProducto.reserva += ordenReservaAddDto.cantidad;
                        reservas.Add(ordenReserva);
                    }
                    else
                    {
                        mensaje += $"Codigo: {existeProducto.codigo} sin disponibilidad. </br>";
                    }
                }

                _unitOfWork.OrdenesReservas.AddRange(reservas);
                await _unitOfWork.SaveAsync();
                return Ok(mensaje);
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
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var ordenReserva = await _unitOfWork.OrdenesReservas.GetByIdAsync(id);
                if (ordenReserva == null)
                    return NotFound();

                var existeProducto = await _unitOfWork.Productos
                                                        .BuscarProductoByCodigoByRucProveedorAsync(ordenReserva.codigoProducto, ordenReserva.rucProveedor);
                if (existeProducto != null)
                {
                    existeProducto.reserva -= ordenReserva.cantidad;
                    existeProducto.disponibilidad += ordenReserva.cantidad;
                }

                _unitOfWork.OrdenesReservas.Remove(ordenReserva);
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
