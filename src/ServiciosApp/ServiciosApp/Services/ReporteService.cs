using Infrastructure.ServiciosApp.DTOs;
using Infrastructure.ServiciosApp.Repositories;
using System;
using System.Collections.Generic;

namespace ServiciosApp.Services
{
    public interface IReporteService
    {
        List<ReporteServiciosPorCliente> ObtenerReporteServiciosPorCliente(int clienteId, DateTime fechaInicio, DateTime fechaFin);
        List<ReporteAcumuladoPorTipo> ObtenerReporteAcumuladoPorTipo(DateTime fechaInicio, DateTime fechaFin);
        List<ReporteServiciosPorOperador> ObtenerReporteServiciosPorOperador(int operadorId, DateTime fechaInicio, DateTime fechaFin);
        List<ReporteResumenGeneral> ObtenerReporteResumenGeneral(DateTime fechaInicio, DateTime fechaFin);
    }

    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;

        public ReporteService(IReporteRepository reporteRepository)
        {
            _reporteRepository = reporteRepository ?? throw new ArgumentNullException(nameof(reporteRepository));
        }

        public List<ReporteServiciosPorCliente> ObtenerReporteServiciosPorCliente(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            ValidarFechas(fechaInicio, fechaFin);

            if (clienteId <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor a 0", nameof(clienteId));

            return _reporteRepository.GetReporteServiciosPorCliente(clienteId, fechaInicio, fechaFin);
        }

        public List<ReporteAcumuladoPorTipo> ObtenerReporteAcumuladoPorTipo(DateTime fechaInicio, DateTime fechaFin)
        {
            ValidarFechas(fechaInicio, fechaFin);
            return _reporteRepository.GetReporteAcumuladoPorTipo(fechaInicio, fechaFin);
        }

        public List<ReporteServiciosPorOperador> ObtenerReporteServiciosPorOperador(int operadorId, DateTime fechaInicio, DateTime fechaFin)
        {
            ValidarFechas(fechaInicio, fechaFin);

            if (operadorId <= 0)
                throw new ArgumentException("El ID del operador debe ser mayor a 0", nameof(operadorId));

            return _reporteRepository.GetReporteServiciosPorOperador(operadorId, fechaInicio, fechaFin);
        }

        public List<ReporteResumenGeneral> ObtenerReporteResumenGeneral(DateTime fechaInicio, DateTime fechaFin)
        {
            ValidarFechas(fechaInicio, fechaFin);
            return _reporteRepository.GetReporteResumenGeneral(fechaInicio, fechaFin);
        }

        private void ValidarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor a la fecha final");

            var hoyInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            if (fechaInicio > hoyInicio)
                throw new ArgumentException("La fecha de inicio no puede ser una fecha futura");
        }
    }
}
