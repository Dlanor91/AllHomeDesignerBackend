using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class ClienteRegistroController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClienteRegistroController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteRegistroListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var clientesRegistrados = await _unitOfWork.ClientesRegistrados
                                                    .GetAllAsync();
                return _mapper.Map<List<ClienteRegistroListDto>>(clientesRegistrados);
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
        public async Task<ActionResult<ClienteRegistroListDto>> Get(int id)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var clienteRegistrado = await _unitOfWork.ClientesRegistrados
                                                            .GetByIdAsync(id);

                if (clienteRegistrado == null)
                    return NotFound();

                return _mapper.Map<ClienteRegistroListDto>(clienteRegistrado);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("verificarCliente/{documentoProfesional}/{rucFilial}/{documentoCliente}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllClientesRegistrados(string documentoProfesional, string rucFilial, string documentoCliente)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                Persona cliente = new Persona();
                cliente = null;
                Persona profesional = new Persona();
                profesional = null;
                Filial afiliado = new Filial();
                afiliado = null;
                ClienteRegistroAddDto crearRegistro = new ClienteRegistroAddDto();
                crearRegistro = null;

                if (documentoCliente.ToLower().Replace(" ", "") != "null")
                {
                    cliente = await _unitOfWork.Personas
                                    .GetByIdAsync(documentoCliente);
                    if (cliente == null)
                        return NotFound();
                }

                if (documentoProfesional.ToLower().Replace(" ", "") != "null")
                {
                    var profesionalRegistrado = await _unitOfWork.Personas
                                                    .GetByIdAsync(documentoProfesional);

                    if (profesionalRegistrado == null)
                        return BadRequest("No existe el documento ingresado en la lista de profesionales.");

                    var clienteRegistradoPorProfesional = await _unitOfWork.ClientesRegistrados
                                                        .verificarRegistroClienteProfesional(documentoProfesional, documentoCliente);
                    if (clienteRegistradoPorProfesional == null)
                    {
                        crearRegistro = new ClienteRegistroAddDto
                        {
                            documentoProfesional = documentoProfesional,
                            documentoCliente = documentoCliente,
                            rucFilial = null,
                            rucEmpresa = null
                        };
                    }
                    else
                        return Ok();
                }

                if (rucFilial.ToLower().Replace(" ", "") != "null")
                {
                    var filialRegistrada = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                    if (filialRegistrada == null)
                        return BadRequest("No existe el ruc ingresado en la lista de afiliados.");

                    var clienteRegistradoPorFilial = await _unitOfWork.ClientesRegistrados
                                                                .verificarRegistroClienteFilial(rucFilial, documentoCliente);
                    if (clienteRegistradoPorFilial == null)
                    {
                        crearRegistro = new ClienteRegistroAddDto
                        { documentoProfesional = null, documentoCliente = documentoCliente, rucFilial = rucFilial, rucEmpresa = null };
                    }
                    else
                        return Ok();
                }

                var clienteRegistrado = _mapper.Map<ClienteRegistro>(crearRegistro);

                _unitOfWork.ClientesRegistrados.Add(clienteRegistrado);
                await _unitOfWork.SaveAsync();
                return StatusCode(201);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("verificarEmpresa/{documentoProfesional}/{rucFilial}/{rucEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllEmpresasRegistrados(string documentoProfesional, string rucFilial, string rucEmpresa)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                Empresa empresa = new Empresa();
                empresa = null;
                Persona profesional = new Persona();
                profesional = null;
                Filial afiliado = new Filial();
                afiliado = null;
                ClienteRegistroAddDto crearRegistro = new ClienteRegistroAddDto();
                crearRegistro = null;

                if (rucEmpresa.ToLower().Replace(" ", "") != "null")
                {
                    empresa = await _unitOfWork.Empresas
                                    .GetByIdAsync(rucEmpresa);
                    if (empresa == null)
                        return NotFound();
                }

                if (documentoProfesional.ToLower().Replace(" ", "") != "null")
                {
                    var profesionalRegistrado = await _unitOfWork.Personas
                                                    .GetByIdAsync(documentoProfesional);

                    if (profesionalRegistrado == null)
                        return BadRequest("No existe el documento ingresado en la lista de profesionales.");

                    var empresaRegistradoPorProfesional = await _unitOfWork.ClientesRegistrados
                                                        .verificarRegistroEmpresaProfesional(documentoProfesional, rucEmpresa);
                    if (empresaRegistradoPorProfesional == null)
                    {
                        crearRegistro = new ClienteRegistroAddDto
                        { documentoProfesional = documentoProfesional, documentoCliente = null, rucFilial = null, rucEmpresa = rucEmpresa };
                    }
                    else
                        return Ok();
                }

                if (rucFilial.ToLower().Replace(" ", "") != "null")
                {
                    var filialRegistrada = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                    if (filialRegistrada == null)
                        return BadRequest("No existe el ruc ingresado en la lista de afiliados.");

                    var empresaRegistradoPorProfesional = await _unitOfWork.ClientesRegistrados
                                                                .verificarRegistroEmpresaFilial(rucFilial, rucEmpresa);
                    if (empresaRegistradoPorProfesional == null)
                    {
                        crearRegistro = new ClienteRegistroAddDto
                        { documentoProfesional = null, documentoCliente = null, rucFilial = rucFilial, rucEmpresa = rucEmpresa };
                    }
                    else
                        return Ok();
                }

                var clienteRegistrado = _mapper.Map<ClienteRegistro>(crearRegistro);

                _unitOfWork.ClientesRegistrados.Add(clienteRegistrado);
                await _unitOfWork.SaveAsync();
                return StatusCode(201);
            }
            else
            {
                return StatusCode(403);
            }
        }


        [HttpGet("clientesListados/{documentoProfesional}/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteRegistroListFiltradaClientesDto>>> GetAllClientesRegistradosFiltradosClientes(string documentoProfesional, string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                IEnumerable<ClienteRegistro> clientesRegistrados = null;

                if (documentoProfesional != "null")
                {
                    var profesionalRegistrado = await _unitOfWork.Personas
                                                                        .GetByIdAsync(documentoProfesional);

                    if (profesionalRegistrado == null)
                        return BadRequest("No existe el documento ingresado en la lista de profesionales.");

                    clientesRegistrados = await _unitOfWork.ClientesRegistrados
                                                    .GetAllClientesProfesionalesAsync(documentoProfesional);
                }
                else
                {
                    var afiliadoRegistrado = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                    if (afiliadoRegistrado == null)
                        return BadRequest("No existe el ruc ingresado en la lista de filiales.");

                    clientesRegistrados = await _unitOfWork.ClientesRegistrados
                                                    .GetAllClientesAfiliadosAsync(rucFilial);
                }

                return _mapper.Map<List<ClienteRegistroListFiltradaClientesDto>>(clientesRegistrados);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("empresasListadas/{documentoProfesional}/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteRegistroListFiltradaEmpresasDto>>> GetAllClientesRegistradosFiltradosEmpresas(string documentoProfesional, string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                IEnumerable<ClienteRegistro> clientesRegistrados = null;

                if (documentoProfesional != "null")
                {
                    var profesionalRegistrado = await _unitOfWork.Personas
                                                                        .GetByIdAsync(documentoProfesional);

                    if (profesionalRegistrado == null)
                        return BadRequest("No existe el documento ingresado en la lista de profesionales.");

                    clientesRegistrados = await _unitOfWork.ClientesRegistrados
                                                    .GetAllEmpresasProfesionalesAsync(documentoProfesional);
                }
                else
                {
                    var afiliadoRegistrado = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                    if (afiliadoRegistrado == null)
                        return BadRequest("No existe el ruc ingresado en la lista de filiales.");

                    clientesRegistrados = await _unitOfWork.ClientesRegistrados
                                                    .GetAllEmpresasAfiliadosAsync(rucFilial);
                }

                return _mapper.Map<List<ClienteRegistroListFiltradaEmpresasDto>>(clientesRegistrados);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteRegistro>> Post(ClienteRegistroAddDto clienteRegistradoAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                Persona existeCliente = new Persona();
                existeCliente = null;
                Persona existeProfesional = new Persona();
                existeProfesional = null;
                Empresa existeEmpresa = new Empresa();
                existeEmpresa = null;
                Filial existeFilial = new Filial();
                existeFilial = null;

                if (clienteRegistradoAddDto == null)
                    return BadRequest();

                if (clienteRegistradoAddDto.documentoCliente == null && clienteRegistradoAddDto.rucEmpresa == null && clienteRegistradoAddDto.documentoProfesional == null && clienteRegistradoAddDto.rucFilial == null)
                    return BadRequest("Debe completar al menos dos campos, un cliente y una empresa o persona que registre los datos");

                if (clienteRegistradoAddDto.documentoCliente != null)
                {
                    existeCliente = await _unitOfWork.Personas
                                         .GetByIdAsync(clienteRegistradoAddDto.documentoCliente);
                    if (existeCliente == null)
                        return BadRequest("El cliente ingresado no está registrado en nuestra base de datos.");

                }

                if (clienteRegistradoAddDto.documentoProfesional != null)
                {
                    existeProfesional = await _unitOfWork.Personas
                                         .GetByIdAsync(clienteRegistradoAddDto.documentoProfesional);
                    if (existeProfesional == null)
                        return BadRequest("El profesional ingresado no está registrado en nuestra base de datos.");
                }

                if (clienteRegistradoAddDto.rucEmpresa != null)
                {
                    existeEmpresa = await _unitOfWork.Empresas
                                         .GetByIdAsync(clienteRegistradoAddDto.rucEmpresa);
                    if (existeEmpresa == null)
                        return BadRequest("La empresa ingresado no está registrada en nuestra base de datos.");
                }
                if (clienteRegistradoAddDto.rucFilial != null)
                {
                    existeFilial = await _unitOfWork.Filiales
                                         .GetByIdAsync(clienteRegistradoAddDto.rucFilial);
                    if (existeFilial == null)
                        return BadRequest("La filial ingresada no está registrada en nuestra base de datos.");
                }

                var clienteRegistrado = _mapper.Map<ClienteRegistro>(clienteRegistradoAddDto);

                _unitOfWork.ClientesRegistrados.Add(clienteRegistrado);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(Post), clienteRegistradoAddDto);
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
            if (privilegio != null)
            {
                var clienteRegistro = await _unitOfWork.ClientesRegistrados.GetByIdAsync(id);
                if (clienteRegistro == null)
                    return NotFound();

                _unitOfWork.ClientesRegistrados.Remove(clienteRegistro);
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
