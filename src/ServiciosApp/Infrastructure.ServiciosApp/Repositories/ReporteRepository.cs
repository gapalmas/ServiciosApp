using Core.ServiciosApp.Entities;
using Infrastructure.ServiciosApp.Data;
using Infrastructure.ServiciosApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.ServiciosApp.Repositories
{
    public interface IReporteRepository
    {
        List<ReporteServiciosPorCliente> GetReporteServiciosPorCliente(int clienteId, DateTime fechaInicio, DateTime fechaFin);
        List<ReporteAcumuladoPorTipo> GetReporteAcumuladoPorTipo(DateTime fechaInicio, DateTime fechaFin);
        List<ReporteServiciosPorOperador> GetReporteServiciosPorOperador(int operadorId, DateTime fechaInicio, DateTime fechaFin);
        List<ReporteResumenGeneral> GetReporteResumenGeneral(DateTime fechaInicio, DateTime fechaFin);
    }

    public class ReporteRepository : IReporteRepository
    {
        private readonly SqlDbContext _context;

        public ReporteRepository(SqlDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<ReporteServiciosPorCliente> GetReporteServiciosPorCliente(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var reportes = _context.Servicios
                .Where(s => s.ClienteId == clienteId &&
                            s.FechaSolicitud >= fechaInicio &&
                            s.FechaSolicitud <= fechaFin)
                .GroupBy(s => new { s.ClienteId, s.Cliente.Nombre })
                .Select(g => new ReporteServiciosPorCliente
                {
                    ClienteId = g.Key.ClienteId,
                    ClienteNombre = g.Key.Nombre,
                    TotalServicios = g.Count(),
                    ServiciosCompletados = g.Count(s => s.Estado == EstadoServicio.Completado),
                    ServiciosPendientes = g.Count(s => s.Estado == EstadoServicio.Pendiente),
                    CostoTotal = g.Sum(s => s.Costo),
                    CostoPromedio = g.Average(s => s.Costo)
                })
                .ToList();

            return reportes;
        }

        public List<ReporteAcumuladoPorTipo> GetReporteAcumuladoPorTipo(DateTime fechaInicio, DateTime fechaFin)
        {
            var reportes = _context.Servicios
                .Where(s => s.FechaSolicitud >= fechaInicio &&
                            s.FechaSolicitud <= fechaFin)
                .GroupBy(s => s.Tipo)
                .Select(g => new ReporteAcumuladoPorTipo
                {
                    Tipo = g.Key,
                    TipoNombre = g.Key.ToString(),
                    TotalServicios = g.Count(),
                    ServiciosCompletados = g.Count(s => s.Estado == EstadoServicio.Completado),
                    CostoTotal = g.Sum(s => s.Costo),
                    TotalOperadores = g.Select(s => s.OperadorId).Distinct().Count()
                })
                .ToList();

            return reportes;
        }

        public List<ReporteServiciosPorOperador> GetReporteServiciosPorOperador(int operadorId, DateTime fechaInicio, DateTime fechaFin)
        {
            var reportes = _context.Servicios
                .Where(s => s.OperadorId == operadorId &&
                            s.FechaAsignacion >= fechaInicio &&
                            s.FechaAsignacion <= fechaFin)
                .GroupBy(s => new { s.OperadorId, s.Operador.Nombre })
                .Select(g => new ReporteServiciosPorOperador
                {
                    OperadorId = g.Key.OperadorId.Value,
                    OperadorNombre = g.Key.Nombre,
                    TotalServicios = g.Count(),
                    ServiciosCompletados = g.Count(s => s.Estado == EstadoServicio.Completado),
                    ServiciosEnProceso = g.Count(s => s.Estado == EstadoServicio.EnProceso),
                    KmTotales = (int)g.Sum(s => s.Ruta.DistanciaKm),
                    MinutosTotales = g.Sum(s => s.Ruta.TiempoEstimadoMinutos)
                })
                .ToList();

            return reportes;
        }

        public List<ReporteResumenGeneral> GetReporteResumenGeneral(DateTime fechaInicio, DateTime fechaFin)
        {
            var servicios = _context.Servicios
                .Where(s => s.FechaSolicitud >= fechaInicio &&
                            s.FechaSolicitud <= fechaFin)
                .ToList();

            var reporte = new List<ReporteResumenGeneral>
            {
                new ReporteResumenGeneral
                {
                    TotalServicios = servicios.Count,
                    ServiciosCompletados = servicios.Count(s => s.Estado == EstadoServicio.Completado),
                    ServiciosPendientes = servicios.Count(s => s.Estado == EstadoServicio.Pendiente),
                    ServiciosAsignados = servicios.Count(s => s.OperadorId.HasValue),
                    CostoTotal = servicios.Sum(s => s.Costo),
                    TotalClientes = _context.Clientes.Count(c => c.Activo),
                    TotalOperadores = _context.Operadores.Count(o => o.Activo && o.Disponible),
                    TotalRutas = _context.Rutas.Count(r => r.Activo)
                }
            };

            return reporte;
        }
    }
}
