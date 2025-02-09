using System;
using System.Collections.Generic;
using System.Linq;
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
    public class UsuarioService : IUsuariosService
    {
        private readonly IgenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public UsuarioService(IgenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try {
                var _usuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));

                if (_usuarioCreado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("Error al crear el usuario");
                }

                var query = await _usuarioRepositorio.Consultar(u => 
                    u.IdUsuario == _usuarioCreado.IdUsuario
                );

                _usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(_usuarioCreado);
            }
            catch {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);

                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if(usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                bool respuesta = await _usuarioRepositorio.Editar(usuarioEncontrado);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se puede editar el usuario");
                }

                return respuesta;
            }
            catch {
                throw;
            }
        }

        public async Task<bool> Eliminar(int IdUsuarioEliminar)
        {
            try {
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario ==  IdUsuarioEliminar);

                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                bool respuesta = await _usuarioRepositorio.Delete(usuarioEncontrado);

                if (!respuesta) 
                { 
                    throw new TaskCanceledException("No se puede eliminar el usuario"); 
                }
                return respuesta;
            }
            catch {
                throw;
            }
        }

        public async Task<List<UsuarioDTO>> ListaUsuarioDTO()
        {
            try {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listaUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();

                return _mapper.Map<List<UsuarioDTO>>(listaUsuario);
            } catch {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try {
                var queryUsuario = await _usuarioRepositorio.Consultar(u => 
                    u.Correo == correo && u.Clave == clave
                );

                if (queryUsuario.FirstOrDefault() == null) {
                    throw new TaskCanceledException("El usuario no existe");
                }

                Usuario _devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<SesionDTO>(_devolverUsuario);

            } catch {
                throw;
            }
        }
    }
}
