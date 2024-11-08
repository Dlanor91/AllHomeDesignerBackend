using API.Dtos;
using API.Services;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    public class PersonaController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PersonaController(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PersonaListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var personas = await _unitOfWork.Personas
                                                .GetAllAsync();
                return _mapper.Map<List<PersonaListDto>>(personas);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("usuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PersonaListDto>>> GetUsuarios()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var personas = await _unitOfWork.Personas
                                                  .GetAllUsuarios();
                return _mapper.Map<List<PersonaListDto>>(personas);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("trabajadoresFilial/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PersonaListDto>>> GetTrabajadoresFilial(string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var filialExiste = await _unitOfWork.Filiales.GetByIdAsync(rucFilial);

                if (filialExiste == null)
                    return NotFound();

                List<Sucursal> sucursalesFilial = filialExiste.sucursales.ToList();

                List<Persona> trabajadores = new List<Persona>();

                foreach (Sucursal unaSucursal in sucursalesFilial)
                {
                    var listaTrabajadores = await _unitOfWork.Personas
                                                                .GetAllTrabajadoresSucursal(unaSucursal.codigo);
                    trabajadores.AddRange(listaTrabajadores);
                }

                return _mapper.Map<List<PersonaListDto>>(trabajadores);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("trabajadoresSucursal/{codigoSucursal}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PersonaListDto>>> GetAllTrabajadores(string codigoSucursal)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var sucursalExiste = await _unitOfWork.Sucursales.GetByIdAsync(codigoSucursal);

                if (sucursalExiste == null)
                    return NotFound();

                var trabajadores = sucursalExiste.trabajadores.ToList();

                if (trabajadores.Count() == 0)
                {
                    return Conflict("No hay trabajadores para mostrar.");
                }

                return _mapper.Map<List<PersonaListDto>>(trabajadores); ;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("{documento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonaPerfilDto>> Get(string documento)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                string direccionPersona;
                string telefonoPersona;

                var persona = await _unitOfWork.Personas
                                                .GetByIdAsync(documento);

                if (persona == null)
                    return NotFound();

                var direccion = persona.direcciones.LastOrDefault();

                if (direccion == null)
                    direccionPersona ="";
                else
                    direccionPersona = direccion.calle + " " + direccion.nroPuerta.ToString() + ", " + direccion.localidad.nombre +  ", " + direccion.departamento;

                var telefono = persona.telefonos.FirstOrDefault();

                if (telefono == null)
                    telefonoPersona = "";
                else
                    telefonoPersona = telefono.numero.ToString();


                PersonaPerfilDto unaPersona = new PersonaPerfilDto()
                {
                    documento = persona.documento,
                    nombre = persona.nombre,
                    apellido = persona.apellido,
                    nombreUsuario = persona.nombreUsuario,
                    password = persona.password,
                    email = persona.email,
                    idTipoUsuario = persona.idTipoUsuario,
                    rol = persona.tipoUsuario.rol,
                    direccion = direccionPersona,
                    telefono = telefonoPersona,
                };

                return unaPersona;
            }
            else
            {
                return StatusCode(403);
            }
        }



        [HttpGet("busquedaNombreUsuario/{nombreUsuario}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonaListDto>> GetPersonaByUserName(string nombreUsuario)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var persona = await _unitOfWork.Personas
                                                .GetByUserNameAsync(nombreUsuario);

                if (persona == null)
                    return NotFound();

                return _mapper.Map<PersonaListDto>(persona);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("registrarUsuarios/{codigoSucursal}")]
        public async Task<ActionResult> RegisterAsync(PersonaRegistroDto model, string codigoSucursal)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin"))
            {
                var result = await _userService.RegisterAsync(model, codigoSucursal);
                return Ok(result);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("registro/trabajadores/{codigoSucursal}")]
        public async Task<ActionResult> RegisterTrabajadorAsync(PersonaRegistroDto model, string codigoSucursal)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var result = await _userService.RegisterTrabajadorAsync(model, codigoSucursal);
                return Ok(result);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("registroCliente/{rucFilial}/{docProfesional}")]
        public async Task<ActionResult> RegisterClienteAsync(PersonaRegistroUpdateClienteDto model, string rucFilial, string docProfesional)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                var result = await _userService.RegisterClienteAsync(model, rucFilial, docProfesional);
                return Ok(result);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLoginToken(PersonaLoginDto model)
        {
            var result = await _userService.GetLoginTokenAsync(model);
            return Ok(result);
        }

        [HttpPut("updatePersona/{documento}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTrabajadorAsync(string documento, [FromBody] PersonaUpdateDto model)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                if (model == null)
                    return BadRequest();

                var personaExiste = await _unitOfWork.Personas
                                                .GetByIdAsync(documento.Replace(" ", "").ToLower());
                if (personaExiste == null)
                    return NotFound("El documento no está registrado en nuestra base de datos.");

                var result = await _userService.UpdateAsync(personaExiste, model);
                return Ok(result);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPut("updateCliente/{documento}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateClienteAsync(string documento, [FromBody] PersonaRegistroUpdateClienteDto model)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null && !privilegio.Contains("Gerente"))
            {
                if (model == null)
                    return BadRequest();

                var clienteExiste = await _unitOfWork.Personas
                                                .GetByIdAsync(documento.Replace(" ", "").ToLower());
                if (clienteExiste == null)
                    return NotFound("El documento no está registrado en nuestra base de datos.");

                var result = await _userService.UpdateClienteAsync(clienteExiste, model);
                return Ok(result);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{documento}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(string documento)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (!privilegio.Contains("Basico"))
            {
                var persona = await _unitOfWork.Personas.GetByIdAsync(documento);
                if (persona == null)
                    return NotFound();

                var direcciones = persona.direcciones.ToList();
                var telefonos = persona.telefonos.ToList();

                if (direcciones.Count() > 0)
                    _unitOfWork.Direcciones.RemoveRange(direcciones);

                if (telefonos.Count() > 0)
                    _unitOfWork.Telefonos.RemoveRange(telefonos);

                var clientesRegistrados = _unitOfWork.ClientesRegistrados
                                                      .Find(cr => cr.documentoCliente == documento || cr.documentoProfesional == documento)
                                                      .ToList();
                var reservasProductos = _unitOfWork.ReservasProductos
                                                   .Find(rp => rp.documentoCliente == documento || rp.documentoProfesional == documento)
                                                   .ToList();
                if (reservasProductos.Count() > 0)
                    return Conflict("Tiene reservas asociadas, eliminelas primero.");

                if (clientesRegistrados.Count() > 0)
                    _unitOfWork.ClientesRegistrados.RemoveRange(clientesRegistrados);                

                _unitOfWork.Personas.Remove(persona);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("borrarTodosTrabajadoresSucursal/{codigoSucursal}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteAllTrabajadores(string codigoSucursal)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var sucursalExiste = await _unitOfWork.Sucursales.GetByIdAsync(codigoSucursal);

                if (sucursalExiste == null)
                    return NotFound();

                var trabajadores = sucursalExiste.trabajadores.ToList();

                if (trabajadores.Count() == 0)
                {
                    return Conflict("No hay trabajadores para eliminar.");
                }

                _unitOfWork.Personas.RemoveRange(trabajadores);
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
