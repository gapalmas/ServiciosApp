using Core.ServiciosApp.Entities;
using System;

namespace Infrastructure.ServiciosApp.DTOs
{
    public class ReporteServiciosPorCliente
    {
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int TotalServicios { get; set; }
        public int ServiciosCompletados { get; set; }
        public int ServiciosPendientes { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal CostoPromedio { get; set; }
    }

    public class ReporteAcumuladoPorTipo
    {
        public TipoServicio Tipo { get; set; }
        public string TipoNombre { get; set; }
        public int TotalServicios { get; set; }
        public int ServiciosCompletados { get; set; }
        public decimal CostoTotal { get; set; }
        public int TotalOperadores { get; set; }
    }

    public class ReporteServiciosPorOperador
    {
        public int OperadorId { get; set; }
        public string OperadorNombre { get; set; }
        public int TotalServicios { get; set; }
        public int ServiciosCompletados { get; set; }
        public int ServiciosEnProceso { get; set; }
        public decimal KmTotales { get; set; }
        public int MinutosTotales { get; set; }
    }

    public class ReporteResumenGeneral
    {
        public int TotalServicios { get; set; }
        public int ServiciosCompletados { get; set; }
        public int ServiciosPendientes { get; set; }
        public int ServiciosAsignados { get; set; }
        public decimal CostoTotal { get; set; }
        public int TotalClientes { get; set; }
        public int TotalOperadores { get; set; }
        public int TotalRutas { get; set; }
    }
}
