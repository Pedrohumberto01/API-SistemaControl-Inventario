using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.MODEL;

namespace SistemaVenta.DAL.Repositorios.Contrato
{
    public interface IVentaRepository : IgenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta modelo);
    }
}
