using Core.Entities;
using Interfaces.Data;
using Microsoft.Extensions.Logging;

namespace Infraestructura.Data
{
    public class AHDContextSeed
    {
        public static async Task SeedTiposUsuariosAsync(AHDContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.TiposUsuarios.Count() <= 5)
                {
                    var tiposUsuarios = new List<TipoUsuario>()
                    {
                        new TipoUsuario{rol="Vendedor", descripcionRol="Personal de sucursal." },
                        new TipoUsuario{rol="Administrador", descripcionRol="Todos los permisos de la aplicación web." },
                        new TipoUsuario{rol="Cliente", descripcionRol="Persona o Empresa que compra productos o solicita diseños." },
                        new TipoUsuario{rol="Profesional", descripcionRol="Planifica y diseña soluciones." },
                        new TipoUsuario{rol="Supervisor", descripcionRol="Es el encargado de los vendedores de las sucursales." },
                        new TipoUsuario{rol="Gerencia", descripcionRol="Es el encargado de la gestión de personal en las sucursales." },
                    };
                    context.TiposUsuarios.AddRange(tiposUsuarios);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AHDContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedPrivilegiosAsync(AHDContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Privilegios.Count() <= 3)
                {
                    var privilegios = new List<Privilegio>()
                    {
                        new Privilegio{tipo="Basico", descripcion="Todos los permisos de lectura." },
                        new Privilegio{tipo="Admin", descripcion="Todos los permisos de lectura y algunos de escritura y delete." },
                        new Privilegio{tipo="Gerente", descripcion="Permisos de lectura y escritura en trabajadores de sucursales." },
                        new Privilegio{tipo="Superadmin", descripcion="Todos los permisos de lectura, escritura y delete." },
                    };
                    context.Privilegios.AddRange(privilegios);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AHDContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedTipoUsuariosPrivilegiosAsync(AHDContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Privilegios.Count() <= 4)
                {
                    var tipoUsuariosPrivilegios = new List<TipoUsuarioPrivilegio>()
                    {
                        new TipoUsuarioPrivilegio{idTipoUsuario=1, idPrivilegio=1},
                        new TipoUsuarioPrivilegio{idTipoUsuario=2, idPrivilegio=4},
                        new TipoUsuarioPrivilegio{idTipoUsuario=4, idPrivilegio=2},
                        new TipoUsuarioPrivilegio{idTipoUsuario=5, idPrivilegio=2},
                        new TipoUsuarioPrivilegio{idTipoUsuario=6, idPrivilegio=3}
                    };
                    context.TiposUsuariosPrivilegios.AddRange(tipoUsuariosPrivilegios);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AHDContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedFilialesAsync(AHDContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Filial.Count() <= 2)
                {
                    var filiales = new List<Filial>()
                    {
                        new Filial{ruc="111111111111", nombre="Filial Primera", estado ="activo", email ="filialprimera@example.com", fechaRegistro = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day) },
                        new Filial{ruc="222222222222", nombre="Filial Segunda", estado ="activo", email ="filialsegunda@example.com", fechaRegistro = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day) },
                    };
                    context.Filial.AddRange(filiales);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AHDContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedSucursalesAsync(AHDContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Filial.Count() <= 2)
                {
                    var sucursales = new List<Sucursal>()
                    {
                        new Sucursal{codigo="S1FP1110", nombre="Sucursal Primera", email ="sucursalprimera@example.com", fechaRegistro = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day), rucFilial = "111111111111" },
                        new Sucursal{codigo = "S2FP1210", nombre="Sucursal Segunda", email ="sucursalsegunda@example.com", fechaRegistro = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day), rucFilial = "222222222222" },
                    };
                    context.Sucursal.AddRange(sucursales);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AHDContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedPersonasAsync(AHDContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Personas.Count() <= 14)
                {
                    var personas = new List<Persona>()
                    {
                        new Persona{documento="45558732", nombre="Adrián", apellido="Rodríguez", nombreUsuario="adri", email="adrianrodriguezalvez@gmail.com",
                                    password="AQAAAAEAACcQAAAAENZrVLt0uoTWRwaIBVB1YnJ4hchMo2wb5cQ/QmfOdY0Z4tcShVePJfTO84Yndub/4g==", idTipoUsuario=2},//adri
                        new Persona{documento="47222386", nombre="Alejandro", apellido="Velázquez", nombreUsuario="aleVela", email="alevelazquez9434@gmail.com",
                                    password="AQAAAAEAACcQAAAAEBM2Umc4VXhWl+q8YuiQfhzJ8KpJmxhfnymJhJFqbBe7IURWr3kKz/QB4U7yZo0qbQ==", idTipoUsuario=2},//ale123
                        new Persona{documento="62800080", nombre="Ronald", apellido="Lima", nombreUsuario="dlanor91", email="rl8506@gmail.com",
                                    password="AQAAAAEAACcQAAAAEPkpbKHkNbsIztp8toCYUWrrbCgb8KXLmhY3fAsktxV2fhP7YhI3CMu2pO6kX/AQvg==", idTipoUsuario=2},//dlanor91
                        new Persona{documento="36647892", nombre="Mauro", apellido="Castro", nombreUsuario="mcastro", email="mauro.castro@fi365.ort.edu.uy",
                                    password="AQAAAAEAACcQAAAAEFiY4S55K+okW/wx4l6DGA2KdiTSrJt2SYhRBzU1hJZmki04GzkskPF6KI/5kT2aVQ==", idTipoUsuario=2},//mcastro
                        new Persona{documento="FP111121", nombre="Vendedor", apellido="Prueba1", nombreUsuario="vendedor1", email="vendedor1@example.com",
                                    password="AQAAAAEAACcQAAAAEFawgBXugPCZr5mRN2PLYGjn78AIHfZp0c+R8SqjhCil+sljZl50OgzyZi5FNs+JBw==", idTipoUsuario=1,codigoSucursal ="S1FP1110"},//vendedor1
                        new Persona{documento="FS222221", nombre="Vendedor", apellido="Prueba2", nombreUsuario="vendedor2", email="vendedor2@example.com",
                                    password="AQAAAAEAACcQAAAAEP502IWRz0PuriNdVG+rfP4pnZM72SDKFRBkJudfsDViZEsS4Ed7TiiQdHpLzDAr6Q==", idTipoUsuario=1,codigoSucursal ="S2FP1210"},//vendedor2
                        new Persona{documento="FP111111", nombre="Supervisor", apellido="Prueba1", nombreUsuario="supervisor1", email="supervisor1@example.com",
                                    password="AQAAAAEAACcQAAAAEBISwy/OsZuPURfg5YZLAQGrBFuud1o7QHfhk7sZ0KcTOW11vIEOe75R4m2MNbucMA==", idTipoUsuario=5,codigoSucursal ="S1FP1110"},//supervisor1
                        new Persona{documento="FS222211", nombre="Supervisor", apellido="Prueba2", nombreUsuario="supervisor2", email="supervisor2@example.com",
                                    password="AQAAAAEAACcQAAAAEIqRr/jXruBqIYvS6jSzZkrjT0LWtvRpxIOCzikQbzjH+9y5yERjsF/Gz14QtN9cWg==", idTipoUsuario=5,codigoSucursal ="S2FP1210"},//supervisor2
                        new Persona{documento="FP111101", nombre="Gerencia", apellido="Prueba1", nombreUsuario="gerencia1", email="gerencia1@example.com",
                                    password="AQAAAAEAACcQAAAAEF7a/pYFq3DntDQImU55DpjOFb6S6CjNp5oBosCU5HfqRVv/8ZYpZib0fnXNFOJSqA==", idTipoUsuario=6,codigoSucursal ="S1FP1110"},//gerencia1
                        new Persona{documento="FS222201", nombre="Gerencia", apellido="Prueba2", nombreUsuario="gerencia2", email="gerencia2@example.com",
                                    password="AQAAAAEAACcQAAAAEGwPztqx8BlQOZoMZZfPJoWBOgfmfNYC5X4MaNWlJKbcR0VfDHPSj0TPrSAVU8pI4w==", idTipoUsuario=6,codigoSucursal ="S2FP1210"},//gerencia2
                        new Persona{documento="99887744", nombre="Profesional", apellido="Prueba1", nombreUsuario="profesional1", email="profesional1@example.com",
                                    password="AQAAAAEAACcQAAAAEPJ8jPWjICIQveC9Ld7HXFDS4xr7WKlMUroyeREfqsYE0IZbYp/8K8fcpe477QX2uQ==", idTipoUsuario=4},//profesional1
                        new Persona{documento="99887748", nombre="Profesional", apellido="Prueba2",nombreUsuario="profesional2", email="profesional2@example.com",
                                    password="AQAAAAEAACcQAAAAEE/uBDOeoW2GIwApfJSMWtNsPGC4kN1nj1fNtKqLJhj4sK7hQlDppswi37jlRtjYwg==", idTipoUsuario=4},//profesional2
                        new Persona{documento="99887711", nombre="Cliente", apellido="Prueba1", email="cliente1@example.com", idTipoUsuario=3},//cliente1
                        new Persona{documento="99887712", nombre="Cliente", apellido="Prueba2", email="cliente2@example.com", idTipoUsuario=3},//cliente2
                        new Persona{documento="99887713", nombre="Cliente", apellido="Prueba3", email="cliente3@example.com", idTipoUsuario=3},//cliente2
                        new Persona{documento="99887714", nombre="Cliente", apellido="Prueba4", email="cliente4@example.com", idTipoUsuario=3},//cliente2
                    };
                    context.Personas.AddRange(personas);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AHDContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedEmpresassAsync(AHDContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Empresas.Count() <= 2)
                {
                    var empresas = new List<Empresa>()
                    {
                        new Empresa{ruc="111111111111", nombre="Empresa Prueba1", razonSocial ="Empresa Prueba1", comentarios =null, email ="empresaprueba1@example.com",idTipoUsuario=3},
                        new Empresa{ruc="222222222222", nombre="Empresa Prueba2", razonSocial ="Empresa Prueba2", comentarios =null, email ="empresaprueba2@example.com",idTipoUsuario=3}
                    };
                    context.Empresas.AddRange(empresas);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AHDContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
