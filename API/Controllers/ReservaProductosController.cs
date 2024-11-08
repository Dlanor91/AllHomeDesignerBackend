using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class ReservaProductosController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservaProductosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReservaProductosListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var reservasProductos = await _unitOfWork.ReservasProductos
                                                            .GetAllAsync();

                return _mapper.Map<List<ReservaProductosListDto>>(reservasProductos);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("ventasFilial/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservaProductosVentasDto>>> GetAllByRucFilial(string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Profesional"))
            {
                List<ReservaProductosVentasDto> mostrarDatos = new List<ReservaProductosVentasDto>();

                float totalPesos = 0;
                float totalDolares = 0;

                var existeFilial = await _unitOfWork.Filiales
                                                .GetByIdAsync(rucFilial);

                if (existeFilial == null)
                    return NotFound();

                var reservasProductos = await _unitOfWork.ReservasProductos
                                                         .GetReservasByRucFilial(rucFilial);

                if (reservasProductos.Count() > 0)
                {
                    foreach (ReservaProductos unaReserva in reservasProductos)
                    {
                        ReservaProductosVentasDto unaVenta = new ReservaProductosVentasDto();
                        totalPesos = 0;
                        totalDolares = 0;
                        foreach (OrdenReserva unaOrden in unaReserva.ordenesReservas)
                        {
                            if (unaOrden.simboloMoneda == "$")
                                totalPesos += unaOrden.precioFinal;
                            else
                                totalDolares += unaOrden.precioFinal;
                        };

                        unaVenta.fechaCompra = unaReserva.fechaCompra;
                        unaVenta.totalPesos = (float)Math.Round(totalPesos, 2);
                        unaVenta.totalDolares = (float)Math.Round(totalDolares, 2);
                        unaVenta.rucEmpresa = unaReserva.rucEmpresa;
                        unaVenta.documentoCliente = unaReserva.documentoCliente;
                        unaVenta.codigo = unaReserva.codigo;

                        if (unaReserva.rucEmpresa != null)
                        {
                            unaVenta.nombreEmpresa = unaReserva.empresa.nombre;
                            unaVenta.razonSocialEmpresa = unaReserva.empresa.razonSocial;
                        }

                        if (unaReserva.documentoCliente != null)
                        {
                            unaVenta.nombreCliente = unaReserva.cliente.nombre;
                            unaVenta.apellidoCliente = unaReserva.cliente.apellido;
                        }

                        mostrarDatos.Add(unaVenta);
                    };
                }
                else
                    return BadRequest("No existen ventas para mostrar");

                return mostrarDatos;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("ventasProfesional/{documentoProfesional}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservaProductosVentasDto>>> GetAllByDocProfesional(string documentoProfesional)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                List<ReservaProductosVentasDto> mostrarDatos = new List<ReservaProductosVentasDto>();

                float totalPesos = 0;
                float totalDolares = 0;

                var existeProfesional = await _unitOfWork.Personas
                                                            .GetByIdAsync(documentoProfesional);

                if (existeProfesional == null)
                    return NotFound();

                var reservasProductos = await _unitOfWork.ReservasProductos
                                                         .GetReservasByDocumentoProfesional(documentoProfesional);

                if (reservasProductos.Count() > 0)
                {
                    foreach (ReservaProductos unaReserva in reservasProductos)
                    {
                        ReservaProductosVentasDto unaVenta = new ReservaProductosVentasDto();
                        totalPesos = 0;
                        totalDolares = 0;
                        foreach (OrdenReserva unaOrden in unaReserva.ordenesReservas)
                        {
                            if (unaOrden.simboloMoneda == "$")
                                totalPesos += unaOrden.precioFinal;
                            else
                                totalDolares += unaOrden.precioFinal;
                        };

                        unaVenta.fechaCompra = unaReserva.fechaCompra;
                        unaVenta.totalPesos = (float)Math.Round(totalPesos, 2);
                        unaVenta.totalDolares = (float)Math.Round(totalDolares, 2);
                        unaVenta.rucEmpresa = unaReserva.rucEmpresa;
                        unaVenta.documentoCliente = unaReserva.documentoCliente;
                        unaVenta.codigo = unaReserva.codigo;

                        if (unaReserva.rucEmpresa != null)
                        {
                            unaVenta.nombreEmpresa = unaReserva.empresa.nombre;
                            unaVenta.razonSocialEmpresa = unaReserva.empresa.razonSocial;
                        }

                        if (unaReserva.documentoCliente != null)
                        {
                            unaVenta.nombreCliente = unaReserva.cliente.nombre;
                            unaVenta.apellidoCliente = unaReserva.cliente.apellido;
                        }

                        mostrarDatos.Add(unaVenta);
                    };
                }
                else
                    return BadRequest("No existen ventas para mostrar");

                return mostrarDatos;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("comprasCliente/{documentoCliente}/{documentoProfesional}/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservaProductosVentasDto>>> GetAllByDocCliente(string documentoCliente, string documentoProfesional, string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                List<ReservaProductosVentasDto> mostrarDatos = new List<ReservaProductosVentasDto>();

                float totalPesos = 0;
                float totalDolares = 0;

                var existeCliente = await _unitOfWork.Personas
                                                            .GetByIdAsync(documentoCliente);

                if (existeCliente == null)
                    return NotFound("No existe el cliente ingresado.");

                if (documentoProfesional != "null")
                {

                    var existeProfesional = await _unitOfWork.Personas
                                                            .GetByIdAsync(documentoProfesional);

                    if (existeProfesional == null)
                        return NotFound("No existe el profesional ingresado.");
                }

                if (rucFilial != "null")
                {

                    var existeFilial = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                    if (existeFilial == null)
                        return NotFound("No existe la filial ingresada.");
                }

                var reservasProductos = await _unitOfWork.ReservasProductos
                                                         .GetReservasByDocumentoCliente(documentoCliente, documentoProfesional, rucFilial);

                if (reservasProductos.Count() > 0)
                {
                    foreach (ReservaProductos unaReserva in reservasProductos)
                    {
                        ReservaProductosVentasDto unaVenta = new ReservaProductosVentasDto();
                        totalPesos = 0;
                        totalDolares = 0;
                        foreach (OrdenReserva unaOrden in unaReserva.ordenesReservas)
                        {
                            if (unaOrden.simboloMoneda == "$")
                                totalPesos += unaOrden.precioFinal;
                            else
                                totalDolares += unaOrden.precioFinal;
                        };

                        unaVenta.fechaCompra = unaReserva.fechaCompra;
                        unaVenta.totalPesos = (float)Math.Round(totalPesos, 2);
                        unaVenta.totalDolares = (float)Math.Round(totalDolares, 2);
                        unaVenta.rucEmpresa = unaReserva.rucEmpresa;
                        unaVenta.documentoCliente = unaReserva.documentoCliente;
                        unaVenta.codigo = unaReserva.codigo;

                        if (unaReserva.rucEmpresa != null)
                        {
                            unaVenta.nombreEmpresa = unaReserva.empresa.nombre;
                            unaVenta.razonSocialEmpresa = unaReserva.empresa.razonSocial;
                        }

                        if (unaReserva.documentoCliente != null)
                        {
                            unaVenta.nombreCliente = unaReserva.cliente.nombre;
                            unaVenta.apellidoCliente = unaReserva.cliente.apellido;
                        }

                        mostrarDatos.Add(unaVenta);
                    };
                }
                else
                    return BadRequest("No existen ventas para mostrar");

                return mostrarDatos;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("comprasEmpresa/{rucEmpresa}/{documentoProfesional}/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReservaProductosVentasDto>>> GetAllByRucEmpresa(string rucEmpresa, string documentoProfesional, string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                List<ReservaProductosVentasDto> mostrarDatos = new List<ReservaProductosVentasDto>();

                float totalPesos = 0;
                float totalDolares = 0;

                var existeEmpresa = await _unitOfWork.Empresas
                                                     .GetByIdAsync(rucEmpresa);

                if (existeEmpresa == null)
                    return NotFound("No existe la empresa ingresada.");

                if (documentoProfesional != "null")
                {

                    var existeProfesional = await _unitOfWork.Personas
                                                            .GetByIdAsync(documentoProfesional);

                    if (existeProfesional == null)
                        return NotFound("No existe el profesional ingresado.");
                }

                if (rucFilial != "null")
                {

                    var existeFilial = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                    if (existeFilial == null)
                        return NotFound("No existe la filial ingresada.");
                }

                var reservasProductos = await _unitOfWork.ReservasProductos
                                                         .GetReservasByRucEmpresa(rucEmpresa, documentoProfesional, rucFilial);

                if (reservasProductos.Count() > 0)
                {
                    foreach (ReservaProductos unaReserva in reservasProductos)
                    {
                        ReservaProductosVentasDto unaVenta = new ReservaProductosVentasDto();
                        totalPesos = 0;
                        totalDolares = 0;
                        foreach (OrdenReserva unaOrden in unaReserva.ordenesReservas)
                        {
                            if (unaOrden.simboloMoneda == "$")
                                totalPesos += unaOrden.precioFinal;
                            else
                                totalDolares += unaOrden.precioFinal;
                        };

                        unaVenta.fechaCompra = unaReserva.fechaCompra;
                        unaVenta.totalPesos = (float)Math.Round(totalPesos, 2);
                        unaVenta.totalDolares = (float)Math.Round(totalDolares, 2);
                        unaVenta.rucEmpresa = unaReserva.rucEmpresa;
                        unaVenta.documentoCliente = unaReserva.documentoCliente;
                        unaVenta.codigo = unaReserva.codigo;

                        if (unaReserva.rucEmpresa != null)
                        {
                            unaVenta.nombreEmpresa = unaReserva.empresa.nombre;
                            unaVenta.razonSocialEmpresa = unaReserva.empresa.razonSocial;
                        }

                        if (unaReserva.documentoCliente != null)
                        {
                            unaVenta.nombreCliente = unaReserva.cliente.nombre;
                            unaVenta.apellidoCliente = unaReserva.cliente.apellido;
                        }

                        mostrarDatos.Add(unaVenta);
                    };
                }
                else
                    return BadRequest("No existen ventas para mostrar");

                return mostrarDatos;
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
        public async Task<ActionResult<ReservaProductosListDto>> Get(string codigo)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var reservaProductos = await _unitOfWork.ReservasProductos
                            .GetByIdAsync(codigo);

                if (reservaProductos == null)
                    return NotFound();

                return _mapper.Map<ReservaProductosListDto>(reservaProductos);
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
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var reservaProducto = await _unitOfWork.ReservasProductos.GetByIdAsync(codigo);
                if (reservaProducto == null)
                    return NotFound();

                foreach (var orden in reservaProducto.ordenesReservas)
                {
                    var existeProducto = _unitOfWork.Productos
                                                          .Find(p => p.codigo == orden.codigoProducto)
                                                          .FirstOrDefault();
                    if (existeProducto != null)
                    {
                        existeProducto.reserva -= orden.cantidad;
                        existeProducto.disponibilidad += orden.cantidad;
                    }
                    _unitOfWork.OrdenesReservas.Remove(orden);
                    await _unitOfWork.SaveAsync();
                }

                _unitOfWork.ReservasProductos.Remove(reservaProducto);
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
