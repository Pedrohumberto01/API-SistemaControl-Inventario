using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;

        public VentaController(IVentaService ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Registrar(venta);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor, string? nVenta, string? fecha_Ini, string? fecha_Fin)
        {
            var rsp = new Response<List<VentaDTO>>();

            nVenta = nVenta is null ? "" : nVenta;
            fecha_Ini = fecha_Ini is null ? "" : fecha_Ini;
            fecha_Fin = fecha_Fin is null ? "" : fecha_Fin;

            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Historial(buscarPor, nVenta, fecha_Ini, fecha_Fin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fecha_Ini, string? fecha_Fin)
        {
            var rsp = new Response<List<ReporteDTO>>();

            fecha_Ini = fecha_Ini is null ? "" : fecha_Ini;
            fecha_Fin = fecha_Fin is null ? "" : fecha_Fin;

            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Reporte(fecha_Ini, fecha_Fin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }

            return Ok(rsp);
        }
    }
}
