namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IPersonaRepository Personas { get; }
        ITipoUsuarioRepository TiposUsuarios { get; }
        IProveedorRepository Proveedores { get; }
        ICategoriaRepository Categorias { get; }
        ITelefonoRepository Telefonos { get; }
        IProductoRepository Productos { get; }
        IDepartamentoRepository Departamentos { get; }
        ILocalidadRepository Localidades { get; }
        IDireccionRepository Direcciones { get; }
        IOrdenReservaRepository OrdenesReservas { get; }
        IReservaProductos ReservasProductos { get; }
        IMonedaRepository Monedas { get; }
        IPrecioRepository Precios { get; }
        IEmpresaRepository Empresas { get; }
        IPrivilegioRepository Privilegios { get; }
        ITipoUsuarioPrivilegioRepository TiposUsuariosPrivilegios { get; }
        IFilialRepository Filiales { get; }
        ISucursalRepository Sucursales { get; }
        IClienteRegistroRepository ClientesRegistrados { get; }
        IMensajeRepository Mensajes { get; }

        Task<int> SaveAsync();
    }
}
