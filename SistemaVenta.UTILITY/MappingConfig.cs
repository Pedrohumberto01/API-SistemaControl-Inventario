using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Mapster;
using SistemaVenta.DTO;
using SistemaVenta.MODEL;

namespace SistemaVenta.UTILITY
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            //Mapeo de los modelos a interfaces y viceversa

            #region Rol

            TypeAdapterConfig<Rol, RolDTO>.NewConfig();
            TypeAdapterConfig<RolDTO, Rol>.NewConfig();

            #endregion Rol

            #region Menu

            TypeAdapterConfig<Menu, MenuDTO>.NewConfig();
            TypeAdapterConfig<MenuDTO, Menu>.NewConfig();

            #endregion Menu

            #region Usuario

            TypeAdapterConfig<Usuario, UsuarioDTO>
                .NewConfig()
                .Map(dest => dest.RolDescripcion, src => src.IdRolNavigation != null ? src.IdRolNavigation.Nombre : string.Empty)
                .Map(dest => dest.EsActivo, src => src.EsActivo == true ? 1 : 0);

            TypeAdapterConfig<Usuario, SesionDTO>
                .NewConfig()
                .Map(dest => dest.RolDescripcion, src => src.IdRolNavigation.Nombre);

            TypeAdapterConfig<UsuarioDTO, Usuario>
                .NewConfig()
                .Ignore(dest => dest.IdRolNavigation)
                .Map(dest => dest.EsActivo, src => src.EsActivo == 1 ? true : false);


            #endregion Usuario

            #region Categoria

            TypeAdapterConfig<Categoria, CategoriaDTO>.NewConfig();
            TypeAdapterConfig<CategoriaDTO, Categoria>.NewConfig();

            #endregion Categoria

            #region Producto


            TypeAdapterConfig<Producto, ProductoDTO>
                .NewConfig()
                .Map(dest => dest.DescripcionCategoria, src => src.IdCategoriaNavigation.Nombre)
                .Map(dest => dest.Precio, src => Convert.ToString(src.Precio.Value))
                .Map(dest => dest.EsActivo, src => src.EsActivo == true ? 1 : 0);

            TypeAdapterConfig<ProductoDTO, Producto>
                .NewConfig()
                .Ignore(dest => dest.IdCategoriaNavigation)
                .Map(dest => dest.Precio, src => Convert.ToDecimal(src.Precio))
                .Map(dest => dest.EsActivo, src => src.EsActivo == 1 ? true : false);

            #endregion Producto

            #region venta

            TypeAdapterConfig<Venta, VentaDTO>
                .NewConfig()
                .Map(dest => dest.TotalTexto, src => Convert.ToString(src.Total))
                .Map(dest => dest.FechaRegistro, src => src.FechaRegistro.Value.ToString("dd,MM,yyyy"));

            TypeAdapterConfig<VentaDTO, Venta>
                .NewConfig()
                .Map(dest => dest.Total, src => Convert.ToDecimal(src.TotalTexto));

            #endregion Venta

            #region DetalleVenta

            TypeAdapterConfig<DetalleVenta, DetalleVentaDTO>
                .NewConfig()
                .Map(dest => dest.DescripcionProducto, src => src.IdProductoNavigation.Nombre)
                .Map(dest => dest.PrecioTexto, src => Convert.ToString(src.Precio.Value))
                .Map(dest => dest.TotalTexto, src => Convert.ToString(src.Total.Value));

            TypeAdapterConfig<DetalleVentaDTO, DetalleVenta>
                .NewConfig()
                .Map(dest => dest.Precio, src => Convert.ToDecimal(src.PrecioTexto))
                .Map(dest => dest.Total, src => Convert.ToDecimal(src.TotalTexto));

            #endregion DetalleVenta

            #region Reporte

            TypeAdapterConfig<DetalleVenta, ReporteDTO>
                .NewConfig()
                .Map(dest => dest.FechaRegistro, src => src.IdVentaNavigation.FechaRegistro.Value.ToString("dd,MM,yyyy"))
                .Map(dest => dest.NumeroDocumento, src => src.IdVentaNavigation.NumeroDocumento)
                .Map(dest => dest.TipoPago, src => src.IdVentaNavigation.TipoPago)
                .Map(dest => dest.TotalVenta, src => Convert.ToString(src.IdVentaNavigation.Total.Value))
                .Map(dest => dest.Producto, src => src.IdProductoNavigation.Nombre)
                .Map(dest => dest.Precio, src => Convert.ToString(src.IdProductoNavigation.Precio.Value))
                .Map(dest => dest.Total, src => Convert.ToString(src.Total.Value));

            //TypeAdapterConfig<CategoriaDTO, Categoria>.NewConfig();

            #endregion Reporte
        }
    }
}
