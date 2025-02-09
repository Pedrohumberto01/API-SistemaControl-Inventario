using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Identity.Client;
using SistemaVenta.DAL.DBContext;

using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.Repositorios;

using SistemaVenta.UTILITY;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.BLL.Servicios;
using Mapster;
using MapsterMapper;



namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            //Agregar la dependencia del contexto al proyecto
            services.AddDbContext<DbventaContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("CadenaSql"));
            });

            //Agregar la referencia de la implementacion de interfaces genericas obligatoria y su implementacion
            services.AddTransient(typeof(IgenericRepository<>),typeof(GenericRepository<>));

            //Agregar dependencia de interfaz exclusiva a venta
            services.AddScoped<IVentaRepository, VentaRepository>();

            //mapster
            // Configurar Mapster
            MappingConfig.RegisterMappings();
            var config = TypeAdapterConfig.GlobalSettings;
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            //Agregar servicios 
            services.AddScoped<IRolServies, RolService>();
            services.AddScoped<IUsuariosService, UsuarioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IMenuService, MenuService>();
        }
    }
}
