using Core.Interfaces;
using Infraestructura.Repositories;
using Interfaces.Data;

namespace Infraestructura.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AHDContext _context;
        private IPersonaRepository _personas;
        private ITipoUsuarioRepository _tiposUsuarios;
        private IProveedorRepository _proveedores;
        private ICategoriaRepository _categorias;
        private IProductoRepository _productos;
        private ITelefonoRepository _telefonos;
        private IDepartamentoRepository _departamentos;
        private ILocalidadRepository _localidades;
        private IDireccionRepository _direcciones;
        private IOrdenReservaRepository _ordenesReservas;
        private IReservaProductos _reservasProductos;
        private IMonedaRepository _monedas;
        private IPrecioRepository _precios;
        private IEmpresaRepository _empresas;
        private IPrivilegioRepository _privilegios;
        private ITipoUsuarioPrivilegioRepository _tipoUsuarioPrivilegios;
        private IFilialRepository _filiales;
        private ISucursalRepository _sucursales;
        private IClienteRegistroRepository _clientesRegistrados;
        private IMensajeRepository _mensajes;

        public UnitOfWork(AHDContext context)
        {
            _context = context;
        }

        public IPersonaRepository Personas
        {
            get
            {
                if (_personas == null)
                {
                    _personas = new PersonaRepository(_context);
                }
                return _personas;
            }
        }

        public ITipoUsuarioRepository TiposUsuarios
        {
            get
            {
                if (_tiposUsuarios == null)
                {
                    _tiposUsuarios = new TipoUsuarioRepository(_context);
                }
                return _tiposUsuarios;
            }
        }

        public IProveedorRepository Proveedores
        {
            get
            {
                if (_proveedores == null)
                {
                    _proveedores = new ProveedoresRepository(_context);
                }
                return _proveedores;
            }
        }

        public ICategoriaRepository Categorias
        {
            get
            {
                if (_categorias == null)
                {
                    _categorias = new CategoriaRepository(_context);
                }
                return _categorias;
            }
        }

        public IProductoRepository Productos
        {
            get
            {
                if (_productos == null)
                {
                    _productos = new ProductoRepository(_context);
                }
                return _productos;
            }
        }

        public ITelefonoRepository Telefonos
        {
            get
            {
                if (_telefonos == null)
                {
                    _telefonos = new TelefonoRepository(_context);
                }
                return _telefonos;
            }
        }
        public IDepartamentoRepository Departamentos
        {
            get
            {
                if (_departamentos == null)
                {
                    _departamentos = new DepartamentoRepository(_context);
                }
                return _departamentos;
            }
        }

        public ILocalidadRepository Localidades
        {
            get
            {
                if (_localidades == null)
                {
                    _localidades = new LocalidadRepository(_context);
                }
                return _localidades;
            }
        }

        public IDireccionRepository Direcciones
        {
            get
            {
                if (_direcciones == null)
                {
                    _direcciones = new DireccionRepository(_context);
                }
                return _direcciones;
            }
        }

        public IOrdenReservaRepository OrdenesReservas
        {
            get
            {
                if (_ordenesReservas == null)
                {
                    _ordenesReservas = new OrdenReservaRepository(_context);
                }
                return _ordenesReservas;
            }
        }

        public IReservaProductos ReservasProductos
        {
            get
            {
                if (_reservasProductos == null)
                {
                    _reservasProductos = new ReservaProductosRepository(_context);
                }
                return _reservasProductos;
            }
        }

        public IMonedaRepository Monedas
        {
            get
            {
                if (_monedas == null)
                {
                    _monedas = new MonedaRepository(_context);
                }
                return _monedas;
            }
        }

        public IPrecioRepository Precios
        {
            get
            {
                if (_precios == null)
                {
                    _precios = new PrecioRepository(_context);
                }
                return _precios;
            }
        }

        public IEmpresaRepository Empresas
        {
            get
            {
                if (_empresas == null)
                {
                    _empresas = new EmpresaRepository(_context);
                }
                return _empresas;
            }
        }

        public IPrivilegioRepository Privilegios
        {
            get
            {
                if (_privilegios == null)
                {
                    _privilegios = new PrivilegioRepository(_context);
                }
                return _privilegios;
            }
        }

        public ITipoUsuarioPrivilegioRepository TiposUsuariosPrivilegios
        {
            get
            {
                if (_tipoUsuarioPrivilegios == null)
                {
                    _tipoUsuarioPrivilegios = new TipoUsuarioPrivilegioRepository(_context);
                }
                return _tipoUsuarioPrivilegios;
            }
        }

        public IFilialRepository Filiales
        {
            get
            {
                if (_filiales == null)
                {
                    _filiales = new FilialRepository(_context);
                }
                return _filiales;
            }
        }

        public ISucursalRepository Sucursales
        {
            get
            {
                if (_sucursales == null)
                {
                    _sucursales = new SucursalRepository(_context);
                }
                return _sucursales;
            }
        }

        public IClienteRegistroRepository ClientesRegistrados
        {
            get
            {
                if (_clientesRegistrados == null)
                {
                    _clientesRegistrados = new ClienteRegistroRepository(_context);
                }
                return _clientesRegistrados;
            }
        }

        public IMensajeRepository Mensajes
        {
            get
            {
                if (_mensajes == null)
                {
                    _mensajes = new MensajeRepository(_context);
                }
                return _mensajes;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
