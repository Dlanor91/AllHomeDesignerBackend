using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Localidad, LocalidadDto>()
                            .ReverseMap();
            CreateMap<Departamento, DepartamentoDto>()
                            .ReverseMap();
            CreateMap<Telefono, TelefonoAddDto>()
                .ReverseMap();
            CreateMap<Telefono, TelefonoListDto>()
                .ReverseMap();
            CreateMap<TipoUsuario, TipoUsuarioDto>()
                .ReverseMap();
            CreateMap<TipoUsuario, TipoUsuarioListDto>()
                .ForMember(dest => dest.personas, opt => opt.MapFrom(src => src.personas.Select(p => new PersonaListDto
                {
                    documento = p.documento,
                    nombre = p.nombre,
                    apellido = p.apellido
                }).ToList()))
                .ReverseMap();
            CreateMap<Direccion, DireccionAddDto>()
                .ReverseMap();
            CreateMap<Direccion, DireccionUpdateDTo>()
                .ReverseMap()
                .ForMember(origen => origen.persona, dest => dest.Ignore())
                .ForMember(origen => origen.localidad, dest => dest.Ignore());
            CreateMap<Direccion, DireccionListDto>()
                .ForMember(dest => dest.nombreLocalidad, origen => origen.MapFrom(origen => origen.localidad.nombre))
                .ReverseMap()
                .ForMember(origen => origen.localidad, dest => dest.Ignore());
            CreateMap<Direccion, DireccionDto>()
                .ForMember(dest => dest.nombreLocalidad, origen => origen.MapFrom(origen => origen.localidad.nombre))
                .ReverseMap()
                .ForMember(origen => origen.localidad, dest => dest.Ignore());
            CreateMap<Persona, PersonaListDto>()
                .ForMember(dest => dest.rol, origen => origen.MapFrom(origen => origen.tipoUsuario.rol))
                .ReverseMap()
                .ForMember(origen => origen.tipoUsuario, dest => dest.Ignore());
            CreateMap<Persona, PersonaRegistroDto>()
                .ReverseMap()
                .ForMember(origen => origen.tipoUsuario, dest => dest.Ignore());
            CreateMap<Persona, PersonaPerfilDto>()
                .ReverseMap();
            CreateMap<Persona, PersonaUpdateDto>()
                .ReverseMap();
            CreateMap<Proveedor, ProveedorDto>()
                .ReverseMap();
            CreateMap<Proveedor, ProveedorUpdateDto>()
                .ReverseMap();
            CreateMap<Proveedor, ProveedorListDto>()
                .ForMember(dest => dest.productos, opt => opt.MapFrom(src => src.productos.Select(p => new ProductoDto
                {
                    codigo = p.codigo,
                    nombre = p.nombre,
                    stock = p.stock
                }).ToList()))
                .ReverseMap();
            CreateMap<Categoria, CategoriaDto>()
                .ReverseMap();
            CreateMap<Categoria, CategoriaByIdDto>()
                .ReverseMap();
            CreateMap<Producto, ProductoDto>()
                .ReverseMap();
            CreateMap<Producto, ProductoListDto>()
                .ForMember(dest => dest.rucProveedor, origen => origen.MapFrom(origen => origen.proveedor.ruc))
                .ForMember(dest => dest.nombreProveedor, origen => origen.MapFrom(origen => origen.proveedor.nombre))
                .ForMember(dest => dest.nombreCategoria, origen => origen.MapFrom(origen => origen.categoria.nombre))
                .ForMember(dest => dest.rucFilial, origen => origen.MapFrom(origen => origen.filial.ruc))
                .ForMember(dest => dest.nombreFilial, origen => origen.MapFrom(origen => origen.filial.nombre))
                .ReverseMap()
                .ForMember(origen => origen.precios, dest => dest.Ignore())
                .ForMember(origen => origen.proveedor, dest => dest.Ignore())
                .ForMember(origen => origen.categoria, dest => dest.Ignore())
                .ForMember(origen => origen.filial, dest => dest.Ignore());
            CreateMap<Producto, ProductoAddDto>()
                .ReverseMap()
                .ForMember(origen => origen.filial, dest => dest.Ignore())
                .ForMember(origen => origen.proveedor, dest => dest.Ignore())
                .ForMember(origen => origen.categoria, dest => dest.Ignore());
            CreateMap<OrdenReserva, OrdenReservaListDto>()
                .ForMember(dest => dest.nombreProducto, origen => origen.MapFrom(origen => origen.producto.nombre))
                .ReverseMap()
                .ForMember(origen => origen.producto, dest => dest.Ignore());
            CreateMap<OrdenReserva, OrdenReservaDetalleDto>()
                .ForMember(dest => dest.nombreProducto, origen => origen.MapFrom(origen => origen.producto.nombre))
                .ForMember(dest => dest.rendimientoProducto, origen => origen.MapFrom(origen => origen.producto.rendimiento))
                .ReverseMap()
                .ForMember(origen => origen.producto, dest => dest.Ignore());
            CreateMap<OrdenReserva, OrdenReservaAddDto>()
                .ReverseMap()
                .ForMember(origen => origen.producto, dest => dest.Ignore());
            CreateMap<ReservaProductos, ReservaProductosDto>()
                .ReverseMap();
            CreateMap<ReservaProductos, ReservaProductosListDto>()
                .ForMember(dest => dest.nombreCliente, origen => origen.MapFrom(origen => origen.cliente.nombre))
                .ForMember(dest => dest.nombreEmpresa, origen => origen.MapFrom(origen => origen.empresa.nombre))
                .ReverseMap()
                .ForMember(origen => origen.empresa, dest => dest.Ignore())
                .ForMember(origen => origen.cliente, dest => dest.Ignore());
            CreateMap<ReservaProductos, ReservaProductosVentasDto>()
                .ForMember(dest => dest.nombreCliente, origen => origen.MapFrom(origen => origen.cliente.nombre))
                .ForMember(dest => dest.apellidoCliente, origen => origen.MapFrom(origen => origen.cliente.apellido))
                .ForMember(dest => dest.nombreEmpresa, origen => origen.MapFrom(origen => origen.empresa.nombre))
                .ForMember(dest => dest.razonSocialEmpresa, origen => origen.MapFrom(origen => origen.empresa.razonSocial))
                .ReverseMap()
                .ForMember(origen => origen.empresa, dest => dest.Ignore())
                .ForMember(origen => origen.cliente, dest => dest.Ignore());
            CreateMap<Moneda, MonedaDto>()
               .ReverseMap();
            CreateMap<Moneda, MonedaListDto>()
                .ReverseMap();
            CreateMap<Moneda, MonedaAddDto>()
                .ReverseMap();
            CreateMap<Moneda, MonedaUpdateDto>()
                .ReverseMap();
            CreateMap<Precio, PrecioAddDto>()
                .ReverseMap();
            CreateMap<Precio, PrecioListDto>()
                .ReverseMap()
                .ForMember(origen => origen.moneda, dest => dest.Ignore())
                .ForMember(origen => origen.producto, dest => dest.Ignore());
            CreateMap<Precio, PrecioDto>()
                .ForMember(dest => dest.simbolo, origen => origen.MapFrom(origen => origen.moneda.simbolo))
                .ReverseMap()
                .ForMember(origen => origen.moneda, dest => dest.Ignore());
            CreateMap<Empresa, EmpresaListDto>()
                .ForMember(dest => dest.rol, origen => origen.MapFrom(origen => origen.tipoUsuario.rol))
                .ReverseMap()
                .ForMember(origen => origen.tipoUsuario, dest => dest.Ignore())
                .ForMember(origen => origen.direcciones, dest => dest.Ignore())
                .ForMember(origen => origen.telefonos, dest => dest.Ignore());
            CreateMap<Empresa, EmpresaAddUpdateDto>()
                .ReverseMap();
            CreateMap<Privilegio, PrivilegioListDto>()
                .ReverseMap();
            CreateMap<Privilegio, PrivilegioAddUpdateDto>()
               .ReverseMap();
            CreateMap<TipoUsuarioPrivilegio, TipoUsuarioPrivilegioListDto>()
                .ForMember(dest => dest.rol, origen => origen.MapFrom(origen => origen.tipoUsuarioPrivilegios.rol))
                .ForMember(dest => dest.tipo, origen => origen.MapFrom(origen => origen.privilegioTipoUsuario.tipo))
                .ReverseMap()
                .ForMember(origen => origen.privilegioTipoUsuario, dest => dest.Ignore())
                .ForMember(origen => origen.tipoUsuarioPrivilegios, dest => dest.Ignore());
            CreateMap<TipoUsuarioPrivilegio, TipoUsuarioPrivilegioAddUpdateDto>()
                .ReverseMap();
            CreateMap<Filial, FilialListDto>()
                .ReverseMap();
            CreateMap<Filial, FilialAddDto>()
                .ReverseMap();
            CreateMap<Filial, FilialUpdateDto>()
                .ReverseMap();
            CreateMap<Sucursal, SucursalListDto>()
                .ReverseMap();
            CreateMap<Sucursal, SucursalAddDto>()
                .ReverseMap();
            CreateMap<ClienteRegistro, ClienteRegistroListDto>()
                .ReverseMap();
            CreateMap<ClienteRegistro, ClienteRegistroAddDto>()
                .ReverseMap();
            CreateMap<ClienteRegistro, ClienteRegistroListFiltradaClientesDto>()
                .ForMember(dest => dest.documentoCliente, origen => origen.MapFrom(origen => origen.cliente.documento))
                .ForMember(dest => dest.nombreCliente, origen => origen.MapFrom(origen => origen.cliente.nombre))
                .ForMember(dest => dest.apellidoCliente, origen => origen.MapFrom(origen => origen.cliente.apellido))
                .ForMember(dest => dest.emailCliente, origen => origen.MapFrom(origen => origen.cliente.email))
                .ReverseMap()
                .ForMember(origen => origen.cliente, dest => dest.Ignore());
            CreateMap<ClienteRegistro, ClienteRegistroListFiltradaEmpresasDto>()
                .ForMember(dest => dest.rucEmpresaCliente, origen => origen.MapFrom(origen => origen.empresa.ruc))
                .ForMember(dest => dest.emailEmpresa, origen => origen.MapFrom(origen => origen.empresa.email))
                .ForMember(dest => dest.nombreEmpresa, origen => origen.MapFrom(origen => origen.empresa.nombre))
                .ForMember(dest => dest.razonSocialEmpresa, origen => origen.MapFrom(origen => origen.empresa.razonSocial))
                .ReverseMap()
                .ForMember(origen => origen.empresa, dest => dest.Ignore());
            CreateMap<Mensaje, MensajeAddDto>()
                .ReverseMap();
            CreateMap<Mensaje, MensajeListDto>()
                .ReverseMap();
        }
    }
}
