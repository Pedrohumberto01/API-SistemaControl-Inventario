using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mapster;
using MapsterMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.MODEL;

namespace SistemaVenta.BLL.Servicios
{
    public class RolService : IRolServies
    {
        private readonly IgenericRepository<Rol> _rolRepositorio;
        private readonly IMapper _mapper;

        public RolService(IgenericRepository<Rol> rolRepositorio, IMapper mapper)
        {
            _rolRepositorio = rolRepositorio;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try {
                var ListaDeRoles = await _rolRepositorio.Consultar();
                return _mapper.Map<List<RolDTO>>(ListaDeRoles.ToList()); 
            } catch {
                throw;
            }
        }
    }
}
