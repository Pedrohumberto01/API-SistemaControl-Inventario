using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.MODEL;

namespace SistemaVenta.BLL.Servicios
{
    public class DashboardService : IDashboardService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IgenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public DashboardService(IVentaRepository ventaRepository, IgenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        private IQueryable<Venta> RetornarVentas(IQueryable<Venta> tablaVenta, int cantidadDias)
        {
            DateTime ultimaFecha = tablaVenta
                .OrderByDescending(v => v.FechaRegistro)
                .Select(v => v.FechaRegistro)
                .FirstOrDefault() ?? DateTime.Now;

            ultimaFecha = ultimaFecha.AddDays(cantidadDias);

            return tablaVenta.Where(v => v.FechaRegistro.HasValue && v.FechaRegistro.Value.Date >= ultimaFecha.Date);
        }

        private async Task<int> TotalVentaUltimaSemana()
        {
            IQueryable<Venta> ventaQuery = await _ventaRepository.Consultar();

            if (ventaQuery.Any())
            {
                var tablaVenta = RetornarVentas(ventaQuery, -7);
                return tablaVenta.Count();
            }
            return 0;
        }

        private async Task<string> TotalIngresosUltimaSemana()
        {
            IQueryable<Venta> ventaQuery = await _ventaRepository.Consultar();

            if (ventaQuery.Any())
            {
                var tablaVentas = RetornarVentas(ventaQuery, -7);
                return tablaVentas.Sum(v => v.Total ?? 0).ToString("F2");
            }

            return "0.00";
        }

        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> productosQuery = await _productoRepositorio.Consultar();
            return productosQuery.Count();
        }

        private async Task<List<VentaSemanaDTO>> VentasUltimaSemana()
        {
            IQueryable<Venta> ventaQuery = await _ventaRepository.Consultar();

            if (ventaQuery.Any())
            {
                var tablaVenta = RetornarVentas(ventaQuery, -7);

                var ventasAgrupadas = tablaVenta
                    .GroupBy(v => v.FechaRegistro.Value.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new VentaSemanaDTO
                    {
                        Fecha = g.Key.ToString("dd-MM-yyyy"),
                        Total = g.Count()
                    });

                return ventasAgrupadas.ToList();
            }
            return new List<VentaSemanaDTO>();
        }

        public async Task<DashboardDTO> Resumen()
        {
            var dashboardDTO = new DashboardDTO();

            try
            {
                dashboardDTO.TotalVenta = await TotalVentaUltimaSemana();
                dashboardDTO.TotalIngresos = await TotalIngresosUltimaSemana();
                dashboardDTO.TotalProductos = await TotalProductos();
                dashboardDTO.VentaUltimaSemana = await VentasUltimaSemana();
            }
            catch
            {
                throw; // Maneja las excepciones según sea necesario
            }

            return dashboardDTO;
        }
    }
}
