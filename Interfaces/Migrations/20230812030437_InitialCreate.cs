using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Filial",
                columns: table => new
                {
                    ruc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filial", x => x.ruc);
                });

            migrationBuilder.CreateTable(
                name: "Mensaje",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mensaje = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensaje", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Moneda",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cotizacion = table.Column<float>(type: "real", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    simbolo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moneda", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "Privilegio",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilegio", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    ruc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.ruc);
                });

            migrationBuilder.CreateTable(
                name: "TipoUsuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcionRol = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUsuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Localidad",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    idDepartamento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidad", x => x.id);
                    table.ForeignKey(
                        name: "FK_Localidad_Departamento_idDepartamento",
                        column: x => x.idDepartamento,
                        principalTable: "Departamento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    rucProveedor = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    largo = table.Column<float>(type: "real", nullable: false),
                    ancho = table.Column<float>(type: "real", nullable: false),
                    profundidad = table.Column<float>(type: "real", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    disponibilidad = table.Column<int>(type: "int", nullable: false),
                    reserva = table.Column<int>(type: "int", nullable: false),
                    imagen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    presentacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    rendimiento = table.Column<float>(type: "real", maxLength: 100, nullable: true),
                    textura = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sugerencias = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
                    rucFilial = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => new { x.codigo, x.rucProveedor });
                    table.ForeignKey(
                        name: "FK_Producto_Categoria_idCategoria",
                        column: x => x.idCategoria,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Filial_rucFilial",
                        column: x => x.rucFilial,
                        principalTable: "Filial",
                        principalColumn: "ruc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor_rucProveedor",
                        column: x => x.rucProveedor,
                        principalTable: "Proveedor",
                        principalColumn: "ruc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    ruc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    razonSocial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    comentarios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    idTipoUsuario = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.ruc);
                    table.ForeignKey(
                        name: "FK_Empresa_TipoUsuario_idTipoUsuario",
                        column: x => x.idTipoUsuario,
                        principalTable: "TipoUsuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoUsuarioPrivilegio",
                columns: table => new
                {
                    idTipoUsuario = table.Column<int>(type: "int", nullable: false),
                    idPrivilegio = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUsuarioPrivilegio", x => new { x.idPrivilegio, x.idTipoUsuario });
                    table.ForeignKey(
                        name: "FK_TipoUsuarioPrivilegio_Privilegio_idPrivilegio",
                        column: x => x.idPrivilegio,
                        principalTable: "Privilegio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TipoUsuarioPrivilegio_TipoUsuario_idTipoUsuario",
                        column: x => x.idTipoUsuario,
                        principalTable: "TipoUsuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Precio",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    precioLista = table.Column<float>(type: "real", nullable: false),
                    precioVenta = table.Column<float>(type: "real", nullable: false),
                    iva = table.Column<float>(type: "real", nullable: false),
                    precioFinal = table.Column<float>(type: "real", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    codigoMoneda = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    rucProveedor = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    codigoProducto = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precio", x => x.id);
                    table.ForeignKey(
                        name: "FK_Precio_Moneda_codigoMoneda",
                        column: x => x.codigoMoneda,
                        principalTable: "Moneda",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Precio_Producto_codigoProducto_rucProveedor",
                        columns: x => new { x.codigoProducto, x.rucProveedor },
                        principalTable: "Producto",
                        principalColumns: new[] { "codigo", "rucProveedor" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClienteRegistro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    documentoCliente = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rucEmpresa = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    documentoProfesional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rucFilial = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteRegistro", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClienteRegistro_Empresa_rucEmpresa",
                        column: x => x.rucEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "ruc");
                    table.ForeignKey(
                        name: "FK_ClienteRegistro_Filial_rucFilial",
                        column: x => x.rucFilial,
                        principalTable: "Filial",
                        principalColumn: "ruc");
                });

            migrationBuilder.CreateTable(
                name: "Direccion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    calle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nroPuerta = table.Column<int>(type: "int", nullable: false),
                    datos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    complemento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    departamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    idLocalidad = table.Column<int>(type: "int", nullable: false),
                    documentoPersona = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rucEmpresa = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    codigoSucursal = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direccion", x => x.id);
                    table.ForeignKey(
                        name: "FK_Direccion_Empresa_rucEmpresa",
                        column: x => x.rucEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "ruc");
                    table.ForeignKey(
                        name: "FK_Direccion_Localidad_idLocalidad",
                        column: x => x.idLocalidad,
                        principalTable: "Localidad",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sucursal",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    detalles = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    idTelefono = table.Column<int>(type: "int", nullable: true),
                    idDireccion = table.Column<int>(type: "int", nullable: true),
                    rucFilial = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sucursal", x => x.codigo);
                    table.ForeignKey(
                        name: "FK_Sucursal_Direccion_idDireccion",
                        column: x => x.idDireccion,
                        principalTable: "Direccion",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Sucursal_Filial_rucFilial",
                        column: x => x.rucFilial,
                        principalTable: "Filial",
                        principalColumn: "ruc");
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    documento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    idTipoUsuario = table.Column<int>(type: "int", nullable: false),
                    codigoSucursal = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.documento);
                    table.ForeignKey(
                        name: "FK_Persona_Sucursal_codigoSucursal",
                        column: x => x.codigoSucursal,
                        principalTable: "Sucursal",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "FK_Persona_TipoUsuario_idTipoUsuario",
                        column: x => x.idTipoUsuario,
                        principalTable: "TipoUsuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modelo",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    personaUsuario = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    personaCliente = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelo", x => x.codigo);
                    table.ForeignKey(
                        name: "FK_Modelo_Persona_personaCliente",
                        column: x => x.personaCliente,
                        principalTable: "Persona",
                        principalColumn: "documento");
                    table.ForeignKey(
                        name: "FK_Modelo_Persona_personaUsuario",
                        column: x => x.personaUsuario,
                        principalTable: "Persona",
                        principalColumn: "documento");
                });

            migrationBuilder.CreateTable(
                name: "ReservaProductos",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    documentoCliente = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    documentoProfesional = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rucEmpresa = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rucFilial = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaProductos", x => x.codigo);
                    table.ForeignKey(
                        name: "FK_ReservaProductos_Empresa_rucEmpresa",
                        column: x => x.rucEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "ruc");
                    table.ForeignKey(
                        name: "FK_ReservaProductos_Filial_rucFilial",
                        column: x => x.rucFilial,
                        principalTable: "Filial",
                        principalColumn: "ruc");
                    table.ForeignKey(
                        name: "FK_ReservaProductos_Persona_documentoCliente",
                        column: x => x.documentoCliente,
                        principalTable: "Persona",
                        principalColumn: "documento");
                    table.ForeignKey(
                        name: "FK_ReservaProductos_Persona_documentoProfesional",
                        column: x => x.documentoProfesional,
                        principalTable: "Persona",
                        principalColumn: "documento");
                });

            migrationBuilder.CreateTable(
                name: "Telefono",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    documentoPersona = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rucEmpresa = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    codigoSucursal = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefono", x => x.id);
                    table.ForeignKey(
                        name: "FK_Telefono_Empresa_rucEmpresa",
                        column: x => x.rucEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "ruc");
                    table.ForeignKey(
                        name: "FK_Telefono_Persona_documentoPersona",
                        column: x => x.documentoPersona,
                        principalTable: "Persona",
                        principalColumn: "documento");
                    table.ForeignKey(
                        name: "FK_Telefono_Sucursal_codigoSucursal",
                        column: x => x.codigoSucursal,
                        principalTable: "Sucursal",
                        principalColumn: "codigo");
                });

            migrationBuilder.CreateTable(
                name: "ModeloCliente",
                columns: table => new
                {
                    documentoCliente = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    codigoModelo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloCliente", x => new { x.documentoCliente, x.codigoModelo });
                    table.ForeignKey(
                        name: "FK_ModeloCliente_Modelo_codigoModelo",
                        column: x => x.codigoModelo,
                        principalTable: "Modelo",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeloCliente_Persona_documentoCliente",
                        column: x => x.documentoCliente,
                        principalTable: "Persona",
                        principalColumn: "documento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdenReserva",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precioFinal = table.Column<float>(type: "real", nullable: false),
                    precioProducto = table.Column<float>(type: "real", nullable: false),
                    simboloMoneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigoReservaProducto = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rucProveedor = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    codigoProducto = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    codigoModelo = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenReserva", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrdenReserva_Modelo_codigoModelo",
                        column: x => x.codigoModelo,
                        principalTable: "Modelo",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "FK_OrdenReserva_Producto_codigoProducto_rucProveedor",
                        columns: x => new { x.codigoProducto, x.rucProveedor },
                        principalTable: "Producto",
                        principalColumns: new[] { "codigo", "rucProveedor" });
                    table.ForeignKey(
                        name: "FK_OrdenReserva_ReservaProductos_codigoReservaProducto",
                        column: x => x.codigoReservaProducto,
                        principalTable: "ReservaProductos",
                        principalColumn: "codigo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteRegistro_documentoCliente",
                table: "ClienteRegistro",
                column: "documentoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteRegistro_rucEmpresa",
                table: "ClienteRegistro",
                column: "rucEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteRegistro_rucFilial",
                table: "ClienteRegistro",
                column: "rucFilial");

            migrationBuilder.CreateIndex(
                name: "IX_Direccion_documentoPersona",
                table: "Direccion",
                column: "documentoPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Direccion_idLocalidad",
                table: "Direccion",
                column: "idLocalidad");

            migrationBuilder.CreateIndex(
                name: "IX_Direccion_rucEmpresa",
                table: "Direccion",
                column: "rucEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_email",
                table: "Empresa",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_idTipoUsuario",
                table: "Empresa",
                column: "idTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_razonSocial",
                table: "Empresa",
                column: "razonSocial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_ruc",
                table: "Empresa",
                column: "ruc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localidad_idDepartamento",
                table: "Localidad",
                column: "idDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_Modelo_personaCliente",
                table: "Modelo",
                column: "personaCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Modelo_personaUsuario",
                table: "Modelo",
                column: "personaUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloCliente_codigoModelo",
                table: "ModeloCliente",
                column: "codigoModelo");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenReserva_codigoModelo",
                table: "OrdenReserva",
                column: "codigoModelo");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenReserva_codigoProducto_rucProveedor",
                table: "OrdenReserva",
                columns: new[] { "codigoProducto", "rucProveedor" });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenReserva_codigoReservaProducto",
                table: "OrdenReserva",
                column: "codigoReservaProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_codigoSucursal",
                table: "Persona",
                column: "codigoSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_email",
                table: "Persona",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persona_idTipoUsuario",
                table: "Persona",
                column: "idTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_nombreUsuario",
                table: "Persona",
                column: "nombreUsuario",
                unique: true,
                filter: "[nombreUsuario] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Precio_codigoMoneda",
                table: "Precio",
                column: "codigoMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_Precio_codigoProducto_rucProveedor",
                table: "Precio",
                columns: new[] { "codigoProducto", "rucProveedor" });

            migrationBuilder.CreateIndex(
                name: "IX_Privilegio_tipo",
                table: "Privilegio",
                column: "tipo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_idCategoria",
                table: "Producto",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_rucFilial",
                table: "Producto",
                column: "rucFilial");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_rucProveedor",
                table: "Producto",
                column: "rucProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaProductos_documentoCliente",
                table: "ReservaProductos",
                column: "documentoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaProductos_documentoProfesional",
                table: "ReservaProductos",
                column: "documentoProfesional");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaProductos_rucEmpresa",
                table: "ReservaProductos",
                column: "rucEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaProductos_rucFilial",
                table: "ReservaProductos",
                column: "rucFilial");

            migrationBuilder.CreateIndex(
                name: "IX_Sucursal_email",
                table: "Sucursal",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sucursal_idDireccion",
                table: "Sucursal",
                column: "idDireccion",
                unique: true,
                filter: "[idDireccion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sucursal_rucFilial",
                table: "Sucursal",
                column: "rucFilial");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_codigoSucursal",
                table: "Telefono",
                column: "codigoSucursal",
                unique: true,
                filter: "[codigoSucursal] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_documentoPersona",
                table: "Telefono",
                column: "documentoPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_rucEmpresa",
                table: "Telefono",
                column: "rucEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_TipoUsuario_rol",
                table: "TipoUsuario",
                column: "rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoUsuarioPrivilegio_idPrivilegio_idTipoUsuario",
                table: "TipoUsuarioPrivilegio",
                columns: new[] { "idPrivilegio", "idTipoUsuario" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoUsuarioPrivilegio_idTipoUsuario",
                table: "TipoUsuarioPrivilegio",
                column: "idTipoUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteRegistro_Persona_documentoCliente",
                table: "ClienteRegistro",
                column: "documentoCliente",
                principalTable: "Persona",
                principalColumn: "documento");

            migrationBuilder.AddForeignKey(
                name: "FK_Direccion_Persona_documentoPersona",
                table: "Direccion",
                column: "documentoPersona",
                principalTable: "Persona",
                principalColumn: "documento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Direccion_Empresa_rucEmpresa",
                table: "Direccion");

            migrationBuilder.DropForeignKey(
                name: "FK_Sucursal_Filial_rucFilial",
                table: "Sucursal");

            migrationBuilder.DropForeignKey(
                name: "FK_Direccion_Persona_documentoPersona",
                table: "Direccion");

            migrationBuilder.DropTable(
                name: "ClienteRegistro");

            migrationBuilder.DropTable(
                name: "Mensaje");

            migrationBuilder.DropTable(
                name: "ModeloCliente");

            migrationBuilder.DropTable(
                name: "OrdenReserva");

            migrationBuilder.DropTable(
                name: "Precio");

            migrationBuilder.DropTable(
                name: "Telefono");

            migrationBuilder.DropTable(
                name: "TipoUsuarioPrivilegio");

            migrationBuilder.DropTable(
                name: "Modelo");

            migrationBuilder.DropTable(
                name: "ReservaProductos");

            migrationBuilder.DropTable(
                name: "Moneda");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Privilegio");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Filial");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Sucursal");

            migrationBuilder.DropTable(
                name: "TipoUsuario");

            migrationBuilder.DropTable(
                name: "Direccion");

            migrationBuilder.DropTable(
                name: "Localidad");

            migrationBuilder.DropTable(
                name: "Departamento");
        }
    }
}
