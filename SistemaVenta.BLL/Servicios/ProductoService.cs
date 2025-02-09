using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.MODEL;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly IgenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(IgenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> ListaProductoDTO()
        {
            try {
                var queryProducto = await _productoRepositorio.Consultar();
                var listaProducto = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();

                return _mapper.Map<List<ProductoDTO>>(listaProducto);
            } catch {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try { 
                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(modelo));

                if (productoCreado != null)
                {
                    throw new TaskCanceledException("No se puede crear el producto");
                }

                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try {
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoEncontrado = await _productoRepositorio.Obtener(p => p.IdProducto == modelo.IdProducto);

                if (productoEncontrado != null) {
                    throw new TaskCanceledException("No existe el producto");
                }

                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.EsActivo = productoModelo.EsActivo;

                bool respuesta = await _productoRepositorio.Editar(productoEncontrado);

                if(!respuesta)
                    throw new TaskCanceledException("No se puede editar el producto");

                return respuesta;
            }
            catch {
                throw;
            }
        }

        public async Task<bool> Eliminar(int IdProductoEliminar)
        {
            try
            {
                var productoEncontrado = await _productoRepositorio.Obtener(p => p.IdProducto == IdProductoEliminar);

                if (productoEncontrado == null)
                {
                    throw new TaskCanceledException("El producto no existe");
                }

                bool respuesta = await _productoRepositorio.Delete(productoEncontrado);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se puede eliminar el producto");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
