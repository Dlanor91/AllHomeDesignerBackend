using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class ProductoController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string fotoFolder = "/Imagenes";
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoListDto>>> Get()
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                List<ProductoListDto> productosMostrar = new List<ProductoListDto>();
                ProductoListDto unProducto = null;

                var productos = await _unitOfWork.Productos
                                                    .GetAllAsync();
                if (productos != null && productos.Count() > 0)
                {
                    foreach (Producto producto in productos)
                    {


                        if (producto.imagen != null)
                        {
                            string nombreArchivo = producto.imagen;
                            producto.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/{nombreArchivo}";
                        }
                        else
                        {
                            producto.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/noImagen.jpg";
                        }

                        unProducto = _mapper.Map<ProductoListDto>(producto);

                        Precio ultimoPrecio = await _unitOfWork.Precios
                                                    .UltimoPrecioProductoAsync(unProducto.codigo, unProducto.rucProveedor);

                        unProducto.precio = _mapper.Map<PrecioDto>(ultimoPrecio);

                        productosMostrar.Add(unProducto);
                    };
                }

                return productosMostrar;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductoListDto>>> GetAllProductosByRucFilial(string rucFilial)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                List<ProductoListDto> productosMostrar = new List<ProductoListDto>();
                ProductoListDto unProducto = null;

                var existeFilial = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                if (existeFilial == null)
                    return NotFound("No existe esa filial en nuestra bd.");

                var productosFilial = await _unitOfWork.Productos
                                                        .BuscarProductosByRucFilial(rucFilial);

                foreach (Producto producto in productosFilial)
                {
                    if (producto.imagen != null)
                    {
                        string nombreArchivo = producto.imagen;
                        producto.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/{nombreArchivo}";
                    }
                    else
                    {
                        producto.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/noImagen.jpg";
                    }

                    unProducto = _mapper.Map<ProductoListDto>(producto);

                    Precio ultimoPrecio = await _unitOfWork.Precios
                                                     .UltimoPrecioProductoAsync(unProducto.codigo, unProducto.rucProveedor);

                    unProducto.precio = _mapper.Map<PrecioDto>(ultimoPrecio);

                    productosMostrar.Add(unProducto);
                }

                return productosMostrar;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpGet("{codigo}/{rucProveedor}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoListDto>> Get(string codigo, string rucProveedor)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio != null)
            {
                var productoExiste = await _unitOfWork.Productos
                                                        .BuscarProductoByCodigoByRucProveedorAsync(codigo, rucProveedor);
                ProductoListDto unProducto = null;

                if (productoExiste == null)
                    return NotFound();

                if (productoExiste.imagen != null)
                {
                    string nombreArchivo = productoExiste.imagen;
                    productoExiste.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/{nombreArchivo}";
                }
                else
                {
                    productoExiste.imagen = $"{Request.Scheme}://{Request.Host}/Imagenes/noImagen.jpg";
                }

                unProducto = _mapper.Map<ProductoListDto>(productoExiste);

                Precio ultimoPrecio = await _unitOfWork.Precios
                                                 .UltimoPrecioProductoAsync(unProducto.codigo, unProducto.rucProveedor);

                unProducto.precio = _mapper.Map<PrecioDto>(ultimoPrecio);

                return unProducto;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Producto>> Post(string rucFilial, ProductoAddDto productoAddDto)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var ruta = String.Empty;

                var filialExiste = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                var categoria = await _unitOfWork.Categorias
                                        .GetByIdAsync(productoAddDto.idCategoria);

                var proveedor = await _unitOfWork.Proveedores
                                        .GetByIdAsync(productoAddDto.rucProveedor);

                var existeProducto = _unitOfWork.Productos
                                                 .Find(x => x.codigo == productoAddDto.codigo && x.rucProveedor == productoAddDto.rucProveedor)
                                                 .FirstOrDefault();

                if (rucFilial == null)
                    return BadRequest("La filial ingresada no existe en nuestra base de datos.");

                if (proveedor == null)
                    return BadRequest("El proveedor seleccionado no existe en nuestra base de datos.");

                if (categoria == null)
                    return BadRequest("La categoria seleccionada no existe en nuestra base de datos.");

                if (productoAddDto == null)
                    return NotFound();

                if (existeProducto != null)
                    return Conflict("El producto ingresado ya se encuentra en nuestra base de datos.");

                var producto = _mapper.Map<Producto>(productoAddDto);
                producto.rucProveedor = productoAddDto.rucProveedor;
                producto.imagen = null;
                producto.disponibilidad = productoAddDto.stock;
                producto.rucFilial = rucFilial;

                _unitOfWork.Productos.Add(producto);
                await _unitOfWork.SaveAsync();

                productoAddDto.codigo = producto.codigo;
                return CreatedAtAction(nameof(Post), new { codigo = productoAddDto.codigo }, productoAddDto);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPut("{codigo}/{rucProveedor}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProducto(string codigo, string rucProveedor, [FromForm] IFormFile file)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var ruta = String.Empty;
                var nombreImagen = String.Empty;
                int codificacion = 1;

                var productoExiste = await _unitOfWork.Productos
                                                        .BuscarProductoByCodigoByRucProveedorAsync(codigo, rucProveedor);

                if (productoExiste == null)
                    return NotFound();

                if (productoExiste.imagen!= null)
                {
                    var img = productoExiste.imagen;
                    string[] partes = img.Split('_');
                    string valorDespuesDelGuion = partes[1];
                    codificacion = Int32.Parse(valorDespuesDelGuion)+1;
                }

                if (file.Length > 0)
                {
                    nombreImagen = $"{rucProveedor}_{codigo}_{codificacion}.png";
                    ruta = $"wwwroot\\Imagenes\\{nombreImagen}";
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else
                {
                    nombreImagen = null;
                }

                productoExiste.imagen = nombreImagen;

                await _unitOfWork.SaveAsync();

                return Ok();

            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPost("subirFotosProductos/{rucFilial}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostAllPhotos(string rucFilial, [FromForm] IList<IFormFile> fotos)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var ruta = String.Empty;
                var nombreImagen = String.Empty;

                var filialExiste = await _unitOfWork.Filiales
                                                    .GetByIdAsync(rucFilial);

                if (filialExiste == null)
                    return BadRequest("La filial ingresada no existe en nuestra base de datos.");

                foreach (var foto in fotos)
                {
                    nombreImagen = Path.GetFileNameWithoutExtension(foto.FileName);

                    ruta = $"wwwroot\\Imagenes\\{nombreImagen}";

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);

                    }
                }


                return NoContent();
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("{codigo}/{rucProveedor}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string codigo, string rucProveedor)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                Producto productoExiste = await _unitOfWork.Productos
                                                        .BuscarProductoByCodigoByRucProveedorAsync(codigo, rucProveedor);

                if (productoExiste == null)
                    return NotFound();

                string rutaArchivo = $"wwwroot\\Imagenes\\{productoExiste.imagen}";
                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Delete(rutaArchivo);
                }

                var preciosProductos = productoExiste.precios.ToList();

                if (preciosProductos.Count() > 0)
                    _unitOfWork.Precios.RemoveRange(preciosProductos);

                _unitOfWork.Productos.Remove(productoExiste);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("borrarTodosProductosCategoria/{idCategoria}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteAll(int idCategoria)
        {
            var privilegio = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (privilegio.Contains("Superadmin") || privilegio.Contains("Gerente"))
            {
                var categoria = await _unitOfWork.Categorias
                                                    .GetByIdAsync(idCategoria);
                if (categoria == null)
                    return NotFound();

                var productos = categoria.productos.ToList();

                if (productos.Count() == 0)
                    return Conflict("No hay productos para eliminar.");

                foreach (Producto unProducto in productos)
                {
                    List<Precio> preciosProductos = unProducto.precios.ToList();

                    if (preciosProductos.Count() > 0)
                        _unitOfWork.Precios.RemoveRange(preciosProductos);

                }

                _unitOfWork.Productos.RemoveRange(productos);
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
