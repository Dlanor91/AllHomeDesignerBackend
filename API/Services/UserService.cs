using API.Dtos;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<Persona> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt,
            IPasswordHasher<Persona> passwordHasher)
        {
            _jwt = jwt.Value;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<PersonaDatosLogueadoDto> GetLoginTokenAsync(PersonaLoginDto model)
        {
            PersonaDatosLogueadoDto datosPersonaDto = new PersonaDatosLogueadoDto();

            var persona = await _unitOfWork.Personas
                            .GetByUserNameAsync(model.nombreUsuario);

            if (persona == null)
            {
                datosPersonaDto.estaAutenticado = false;
                datosPersonaDto.mensaje = $"No existe ningún usuario con el username {model.nombreUsuario}.";
                return datosPersonaDto;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(persona, persona.password, model.password);

            if (resultado == PasswordVerificationResult.Success)
            {
                await MostrarDatosLogin(datosPersonaDto, persona);

                return datosPersonaDto;
            }
            datosPersonaDto.estaAutenticado = false;
            datosPersonaDto.mensaje = $"Credenciales incorrectas para el usuario {persona.nombreUsuario}.";
            return datosPersonaDto;
        }

        public async Task<DatosAddUpdateDto> RegisterAsync(PersonaRegistroDto personaRegistroDto, string codigoSucursal)
        {
            DatosAddUpdateDto datosMostrar = new DatosAddUpdateDto();
            datosMostrar.mensaje = "";

            var persona = new Persona
            {
                documento = personaRegistroDto.documento.Replace(" ", ""),
                nombre = personaRegistroDto.nombre.Replace(" ", ""),
                apellido = personaRegistroDto.apellido.Replace(" ", ""),
                nombreUsuario = personaRegistroDto.nombreUsuario.Replace(" ", "").Trim(),
                email = personaRegistroDto.email.Replace(" ", ""),
                idTipoUsuario = personaRegistroDto.idTipoUsuario,
            };

            TipoUsuario tipoUsuario = _unitOfWork.TiposUsuarios
                                                .Find(tu => tu.id == personaRegistroDto.idTipoUsuario)
                                                .FirstOrDefault();

            var emailUnico = _unitOfWork.Personas
                                        .Find(p => p.email.ToLower() == personaRegistroDto.email.Replace(" ", "").ToLower())
                                        .FirstOrDefault();

            var nombreUsuarioUnico = _unitOfWork.Personas
                                        .Find(p => p.nombreUsuario.ToLower() == personaRegistroDto.nombreUsuario.Replace(" ", "").ToLower())
                                        .FirstOrDefault();

            var personaExiste = _unitOfWork.Personas
                                        .Find(p => p.documento.ToLower() == personaRegistroDto.documento.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            if (codigoSucursal == "" || codigoSucursal == "persona")
                codigoSucursal = null;

            if (personaExiste == null)
            {
                if (emailUnico == null)
                {
                    if (nombreUsuarioUnico == null)
                    {
                        if (tipoUsuario != null)
                        {
                            persona.codigoSucursal = codigoSucursal;
                            persona.password = _passwordHasher.HashPassword(persona, personaRegistroDto.password);

                            _unitOfWork.Personas.Add(persona);
                            await _unitOfWork.SaveAsync();

                            datosMostrar.error = "";
                            datosMostrar.mensaje = $"El usuario {persona.nombreUsuario} ha sido registrado correctamente.";

                            return datosMostrar;
                        }
                        else
                        {
                            datosMostrar.error = $"No existe ese tipo de usuario en nuestra base de datos.";
                            return datosMostrar;
                        }
                    }
                    else
                    {
                        datosMostrar.error = $"Ya existe un usuario registrado con ese nombre de usuario.";
                        return datosMostrar;
                    }
                }
                else
                {
                    datosMostrar.error = $"Ya existe un usuario registrado con ese email.";
                    return datosMostrar;
                }
            }
            else
            {
                datosMostrar.error = $"Ya existe un usuario registrado con ese documento.";
                return datosMostrar;
            }
        }

        public async Task<DatosAddUpdateDto> RegisterTrabajadorAsync(PersonaRegistroDto personaRegistroDto, string codigoSucursal)
        {
            DatosAddUpdateDto datosMostrar = new DatosAddUpdateDto();
            datosMostrar.mensaje = "";

            var sucursalExiste = new Sucursal();
            sucursalExiste = null;

            var persona = new Persona
            {
                documento = personaRegistroDto.documento.Replace(" ", ""),
                nombre = personaRegistroDto.nombre.Replace(" ", ""),
                apellido = personaRegistroDto.apellido.Replace(" ", ""),
                nombreUsuario = personaRegistroDto.nombreUsuario.Replace(" ", "").Trim(),
                email = personaRegistroDto.email.Replace(" ", ""),
                idTipoUsuario = personaRegistroDto.idTipoUsuario,
            };

            TipoUsuario tipoUsuario = _unitOfWork.TiposUsuarios
                                                .Find(tu => tu.id == personaRegistroDto.idTipoUsuario)
                                                .FirstOrDefault();

            var emailUnico = _unitOfWork.Personas
                                        .Find(p => p.email.ToLower() == personaRegistroDto.email.Replace(" ", "").ToLower())
                                        .FirstOrDefault();

            var nombreUsuarioUnico = _unitOfWork.Personas
                                        .Find(p => p.nombreUsuario.ToLower() == personaRegistroDto.nombreUsuario.Replace(" ", "").ToLower())
                                        .FirstOrDefault();

            var personaExiste = _unitOfWork.Personas
                                        .Find(p => p.documento.ToLower() == personaRegistroDto.documento.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            if (codigoSucursal != null)
            {
                sucursalExiste = _unitOfWork.Sucursales
                                        .Find(p => p.codigo.ToLower() == codigoSucursal.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            }

            var registrarPersona = false;
            if (sucursalExiste != null && (tipoUsuario.rol != "Cliente" || tipoUsuario.rol != "Profesional" || tipoUsuario.rol != "Administrador" ))
              registrarPersona = true;            
            else 
                registrarPersona = false;
            

            if (personaExiste == null)
            {
                if (emailUnico == null)
                {
                    if (nombreUsuarioUnico == null)
                    {
                        if (tipoUsuario != null)
                        {
                            if (sucursalExiste != null)
                            {
                                if (registrarPersona)
                                {
                                    persona.codigoSucursal = codigoSucursal;
                                    persona.password = _passwordHasher.HashPassword(persona, personaRegistroDto.password);

                                    _unitOfWork.Personas.Add(persona);
                                    await _unitOfWork.SaveAsync();

                                    datosMostrar.error = "";
                                    datosMostrar.mensaje = $"El usuario {persona.nombreUsuario} ha sido registrado correctamente.";

                                    return datosMostrar;
                                }
                                else
                                {
                                    datosMostrar.error = $"En las filiales solo se pueden registrar personal de tipo gerente, comercial, recursos humanos, supervisor o vendedor.";
                                    return datosMostrar;
                                }
                            }
                            else
                            {
                                datosMostrar.error = $"No existe esa sucursal en nuestra base de datos.";
                                return datosMostrar;
                            }
                        }
                        else
                        {
                            datosMostrar.error = $"No existe ese tipo de usuario en nuestra base de datos.";
                            return datosMostrar;
                        }
                    }
                    else
                    {
                        datosMostrar.error = $"Ya existe un usuario registrado con ese nombre de usuario.";
                        return datosMostrar;
                    }
                }
                else
                {
                    datosMostrar.error = $"Ya existe un usuario registrado con ese email.";
                    return datosMostrar;
                }
            }
            else
            {
                datosMostrar.error = $"Ya existe un usuario registrado con ese documento.";
                return datosMostrar;
            }
        }

        public async Task<DatosAddUpdateDto> RegisterClienteAsync(PersonaRegistroUpdateClienteDto personaRegistroClienteDto, string rucFilial, string docProfesional)
        {
            DatosAddUpdateDto datosMostrar = new DatosAddUpdateDto();
            datosMostrar.mensaje = "";

            var persona = new Persona
            {
                documento = personaRegistroClienteDto.documento.Replace(" ", ""),
                nombre = personaRegistroClienteDto.nombre.Trim(),
                apellido = personaRegistroClienteDto.apellido.Replace(" ", ""),
                email = personaRegistroClienteDto.email.Replace(" ", ""),
            };
            persona.idTipoUsuario = _unitOfWork.TiposUsuarios
                                    .Find(tu => tu.rol == "Cliente")
                                    .FirstOrDefault()?.id ?? 0;

            var emailUnico = _unitOfWork.Personas
                                        .Find(p => p.email.ToLower() == personaRegistroClienteDto.email.Replace(" ", "").ToLower())
                                        .FirstOrDefault();

            var personaExiste = _unitOfWork.Personas
                                        .Find(p => p.documento.ToLower() == personaRegistroClienteDto.documento.Replace(" ", "").ToLower())
                                        .FirstOrDefault();

            if (rucFilial =="null")
                rucFilial = null;
            if (docProfesional=="null")
                docProfesional = null;

            if (personaExiste == null)
            {
                if (emailUnico == null)
                {
                    ClienteRegistro registroClientePrimeraVez = new ClienteRegistro
                    {
                        documentoProfesional = docProfesional,
                        rucFilial = rucFilial,
                        documentoCliente = persona.documento,
                        rucEmpresa = null,
                    };

                    _unitOfWork.Personas.Add(persona);
                    _unitOfWork.ClientesRegistrados.Add(registroClientePrimeraVez);

                    await _unitOfWork.SaveAsync();

                    datosMostrar.error = "";
                    datosMostrar.mensaje = $"El cliente {persona.nombre} ha sido registrado correctamente.";

                    return datosMostrar;
                }
                else
                {
                    datosMostrar.error = $"Ya existe un cliente registrado con ese email.";
                    return datosMostrar;
                }
            }
            else
            {
                datosMostrar.error = $"El cliente ya se encuentra registrado.";
                return datosMostrar;
            }
        }

        public async Task<DatosAddUpdateDto> UpdateAsync(Persona personasExiste, [FromBody] PersonaUpdateDto personaUpdateDto)
        {
            DatosAddUpdateDto datosMostrar = new DatosAddUpdateDto();
            datosMostrar.mensaje = "";

            personasExiste.nombre = personaUpdateDto.nombre.Trim();
            personasExiste.apellido = personaUpdateDto.apellido.Replace(" ", "");
            personasExiste.idTipoUsuario = personaUpdateDto.idTipoUsuario;

            Persona emailUnico = null;
            Persona nombreUsuarioUnico = null;

            if (personasExiste.tipoUsuario.rol == "Cliente")
            {
                personasExiste.password = null;
                personasExiste.nombreUsuario = null;
            }
            else
            {
                if (personasExiste.nombreUsuario != personaUpdateDto.nombreUsuario.Replace(" ", ""))
                {
                    personasExiste.nombreUsuario = personaUpdateDto.nombreUsuario.Trim();
                    nombreUsuarioUnico = _unitOfWork.Personas
                                            .Find(p => p.nombreUsuario.ToLower() == personaUpdateDto.nombreUsuario.Replace(" ", "").ToLower())
                                            .FirstOrDefault();
                }
                personasExiste.password = _passwordHasher.HashPassword(personasExiste, personaUpdateDto.password);
            }

            if (personasExiste.email != personaUpdateDto.email.Replace(" ", ""))
            {
                personasExiste.email = personaUpdateDto.email.Replace(" ", "").ToLower();
                emailUnico = _unitOfWork.Personas
                                        .Find(p => p.email.ToLower() == personaUpdateDto.email.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            }

            if (emailUnico == null)
            {
                if (nombreUsuarioUnico == null)
                {
                    await _unitOfWork.SaveAsync();

                    datosMostrar.error = "";
                    datosMostrar.mensaje = $"El usuario {personasExiste.nombreUsuario} ha sido actualizado correctamente.";

                    return datosMostrar;
                }
                else
                {
                    datosMostrar.error = $"Ya existe un usuario registrado con ese nombre de usuario.";
                    return datosMostrar;
                }
            }
            else
            {
                datosMostrar.error = $"Ya existe un usuario registrado con ese email.";
                return datosMostrar;
            }
        }

        public async Task<DatosAddUpdateDto> UpdateClienteAsync(Persona clienteExiste, [FromBody] PersonaRegistroUpdateClienteDto personaRegistroClienteDto)
        {
            DatosAddUpdateDto datosMostrar = new DatosAddUpdateDto();
            datosMostrar.mensaje = "";

            Persona emailUnico = null;
            clienteExiste.nombre = personaRegistroClienteDto.nombre.Trim();
            clienteExiste.apellido = personaRegistroClienteDto.apellido.Replace(" ", "");
            clienteExiste.password = null;

            if (clienteExiste.email != personaRegistroClienteDto.email.Replace(" ", ""))
            {
                clienteExiste.email = personaRegistroClienteDto.email.Replace(" ", "").ToLower();
                emailUnico = _unitOfWork.Personas
                                        .Find(p => p.email.ToLower() == personaRegistroClienteDto.email.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            }

            if (emailUnico == null)
            {
                await _unitOfWork.SaveAsync();
                datosMostrar.error = "";
                datosMostrar.mensaje = $"El cliente {clienteExiste.nombre} ha sido actualizado correctamente.";

                return datosMostrar;
            }
            else
            {
                datosMostrar.error = $"Ya existe un cliente registrado con ese email.";
                return datosMostrar;
            }
        }

        private async Task<JwtSecurityToken> CreateJwtToken(Persona persona)
        {
            var idTipoUsuario = persona.tipoUsuario.id;
            TipoUsuarioPrivilegio tipoUsuarioPrivilegioObtenerPrivilegio = await _unitOfWork.TiposUsuariosPrivilegios
                                                                    .GetByIdTipoUsuario(idTipoUsuario);

            Privilegio privilegio = await _unitOfWork.Privilegios
                                        .GetByIdAsync(tipoUsuarioPrivilegioObtenerPrivilegio.idPrivilegio);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, persona.nombreUsuario),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim("tipoUsuario", persona.tipoUsuario.rol),
                 new Claim("role", privilegio.tipo),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private async Task MostrarDatosLogin(PersonaDatosLogueadoDto datosPersonaLogueadoDto, Persona persona)
        {
            datosPersonaLogueadoDto.estaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(persona);
            datosPersonaLogueadoDto.token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosPersonaLogueadoDto.nombreCompleto = persona.nombre + " " + persona.apellido;
            datosPersonaLogueadoDto.nombreUsuario = persona.nombreUsuario;

            datosPersonaLogueadoDto.mensaje = "Ok";



            var relacionTipoUsuarioPrivilegio = _unitOfWork.TiposUsuariosPrivilegios
                                                            .Find(tup => tup.idTipoUsuario == persona.idTipoUsuario)
                                                            .FirstOrDefault();
            if (relacionTipoUsuarioPrivilegio != null)
            {
                datosPersonaLogueadoDto.rol = relacionTipoUsuarioPrivilegio.privilegioTipoUsuario.tipo;
            }

            var idTipoUsuarioAdmin = _unitOfWork.TiposUsuarios
                                    .Find(tu => tu.rol == "Administrador")
                                    .FirstOrDefault()?.id ?? 0;

            if (persona.codigoSucursal == null && idTipoUsuarioAdmin != persona.idTipoUsuario)
                datosPersonaLogueadoDto.documentoProfesional = persona.documento;
            else
            {
                datosPersonaLogueadoDto.codigoSucursal = persona.codigoSucursal;
                if (datosPersonaLogueadoDto.codigoSucursal != null)
                {
                    Sucursal buscarSucursal = _unitOfWork.Sucursales
                                                            .Find(s => s.codigo == persona.codigoSucursal)
                                                            .FirstOrDefault();
                    datosPersonaLogueadoDto.rucFilial = buscarSucursal.rucFilial;
                }
            }
        }
    }
}
