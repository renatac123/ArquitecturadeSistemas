﻿using SistemaVenta.BLL.Servicios.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace SistemaVenta.BLL.Servicios
{
    public class DashBoardService: IDashBoardService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IMapper _mapper;

        public DashBoardService(IGenericRepository<Producto> productoRepositorio, IVentaRepository ventaRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _ventaRepositorio = ventaRepositorio;
            _mapper = mapper;
        }

        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();

            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }

        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if(_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);
                total = tablaVenta.Count();
            }
            return total;

        }

        private async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);

            }
            return Convert.ToString(resultado, new CultureInfo("es-PE"));

        }

        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> _productoQuery = await _productoRepositorio.Consultar();

            int total = _productoQuery.Count();
            return total;
        }

        private async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if(_ventaQuery.Count() > 0) {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado = tablaVenta
                            .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key)
                            .Select(dv => new {fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count()})
                            .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
            }

            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashBoard = new DashBoardDTO();

            try
            {
                vmDashBoard.TotalVentas = await TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await TotalProductos();

                List<VentasSemanaDTO> listaVentaSemana = new List<VentasSemanaDTO>();

                foreach(KeyValuePair<string, int> item in await VentasUltimaSemana())
                {
                    listaVentaSemana.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                vmDashBoard.VentasUltimaSemana = listaVentaSemana;

            }
            catch
            {
                throw;
            }

            return vmDashBoard;
        }
    }
}
