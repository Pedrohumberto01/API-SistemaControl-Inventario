using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SistemaVenta.DAL.Repositorios
{
    public class GenericRepository<Tmodelo>:IgenericRepository<Tmodelo> where Tmodelo : class
    {
        private readonly DbventaContext _dbcontext;

        public GenericRepository(DbventaContext dbcontext) { 
            _dbcontext = dbcontext;
        }

        public async Task<Tmodelo> Obtener(Expression<Func<Tmodelo, bool>> filtro)
        {
            try { 
                Tmodelo modelo = await _dbcontext.Set<Tmodelo>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch { throw;  }
        }

        public async Task<Tmodelo> Crear(Tmodelo modelo)
        {
            try { 
                _dbcontext.Set<Tmodelo>().Add(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch { throw; }
        }

        public async Task<bool> Editar(Tmodelo modelo)
        {
            try { 
                _dbcontext.Set<Tmodelo>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }

        public async Task<bool> Delete(Tmodelo modelo)
        {
            try
            {
                _dbcontext.Set<Tmodelo>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }

        public async Task<IQueryable<Tmodelo>> Consultar(Expression<Func<Tmodelo, bool>> filtro = null)
        {
            try { 
                IQueryable<Tmodelo> queryModelo = filtro == null ? _dbcontext.Set<Tmodelo>(): _dbcontext.Set<Tmodelo>().Where(filtro);
                return queryModelo;
            }
            catch { throw; }
        }

    }
}
