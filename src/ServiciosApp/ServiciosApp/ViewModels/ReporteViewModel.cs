using Infrastructure.ServiciosApp.DTOs;
using Infrastructure.ServiciosApp.Repositories;
using ServiciosApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ServiciosApp.ViewModels
{
    public class ReporteViewModel : ViewModelBase
    {
        private readonly IReporteService _reporteService;

        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private int _clienteSeleccionadoId;
        private int _operadorSeleccionadoId;
        private ObservableCollection<ReporteServiciosPorCliente> _reportesCliente;
        private ObservableCollection<ReporteAcumuladoPorTipo> _reportesAcumulados;
        private ObservableCollection<ReporteServiciosPorOperador> _reportesOperador;
        private ObservableCollection<ReporteResumenGeneral> _reportesResumen;
        private string _mensajeError;
        private string _tipoReporteSeleccionado;

        public ReporteViewModel(IReporteService reporteService)
        {
            _reporteService = reporteService ?? throw new ArgumentNullException(nameof(reporteService));
            _fechaInicio = DateTime.Now.AddMonths(-1);
            _fechaFin = DateTime.Now;
            InitializeCommands();
        }

        #region Properties

        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { SetProperty(ref _fechaInicio, value); }
        }

        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { SetProperty(ref _fechaFin, value); }
        }

        public int ClienteSeleccionadoId
        {
            get { return _clienteSeleccionadoId; }
            set { SetProperty(ref _clienteSeleccionadoId, value); }
        }

        public int OperadorSeleccionadoId
        {
            get { return _operadorSeleccionadoId; }
            set { SetProperty(ref _operadorSeleccionadoId, value); }
        }

        public ObservableCollection<ReporteServiciosPorCliente> ReportesCliente
        {
            get { return _reportesCliente; }
            set { SetProperty(ref _reportesCliente, value); }
        }

        public ObservableCollection<ReporteAcumuladoPorTipo> ReportesAcumulados
        {
            get { return _reportesAcumulados; }
            set { SetProperty(ref _reportesAcumulados, value); }
        }

        public ObservableCollection<ReporteServiciosPorOperador> ReportesOperador
        {
            get { return _reportesOperador; }
            set { SetProperty(ref _reportesOperador, value); }
        }

        public ObservableCollection<ReporteResumenGeneral> ReportesResumen
        {
            get { return _reportesResumen; }
            set { SetProperty(ref _reportesResumen, value); }
        }

        public string MensajeError
        {
            get { return _mensajeError; }
            set { SetProperty(ref _mensajeError, value); }
        }

        public string TipoReporteSeleccionado
        {
            get { return _tipoReporteSeleccionado; }
            set { SetProperty(ref _tipoReporteSeleccionado, value); }
        }

        #endregion

        #region Commands

        public ICommand GenerarReporteClienteCommand { get; private set; }
        public ICommand GenerarReporteAcumuladoCommand { get; private set; }
        public ICommand GenerarReporteOperadorCommand { get; private set; }
        public ICommand GenerarReporteResumenCommand { get; private set; }

        #endregion

        #region Methods

        private void InitializeCommands()
        {
            GenerarReporteClienteCommand = new RelayCommand(
                () => GenerarReporteCliente(),
                () => FechaInicio <= FechaFin && ClienteSeleccionadoId > 0);
            
            GenerarReporteAcumuladoCommand = new RelayCommand(
                () => GenerarReporteAcumulado(),
                () => FechaInicio <= FechaFin);
            
            GenerarReporteOperadorCommand = new RelayCommand(
                () => GenerarReporteOperador(),
                () => FechaInicio <= FechaFin && OperadorSeleccionadoId > 0);
            
            GenerarReporteResumenCommand = new RelayCommand(
                () => GenerarReporteResumen(),
                () => FechaInicio <= FechaFin);
        }

        private void GenerarReporteCliente()
        {
            try
            {
                MensajeError = null;
                var reportes = _reporteService.ObtenerReporteServiciosPorCliente(
                    ClienteSeleccionadoId, FechaInicio, FechaFin);
                ReportesCliente = new ObservableCollection<ReporteServiciosPorCliente>(reportes);
                TipoReporteSeleccionado = "Servicios por Cliente";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al generar reporte: {ex.Message}";
            }
        }

        private void GenerarReporteAcumulado()
        {
            try
            {
                MensajeError = null;
                var reportes = _reporteService.ObtenerReporteAcumuladoPorTipo(FechaInicio, FechaFin);
                ReportesAcumulados = new ObservableCollection<ReporteAcumuladoPorTipo>(reportes);
                TipoReporteSeleccionado = "Acumulado por Tipo de Servicio";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al generar reporte: {ex.Message}";
            }
        }

        private void GenerarReporteOperador()
        {
            try
            {
                MensajeError = null;
                var reportes = _reporteService.ObtenerReporteServiciosPorOperador(
                    OperadorSeleccionadoId, FechaInicio, FechaFin);
                ReportesOperador = new ObservableCollection<ReporteServiciosPorOperador>(reportes);
                TipoReporteSeleccionado = "Servicios por Operador";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al generar reporte: {ex.Message}";
            }
        }

        private void GenerarReporteResumen()
        {
            try
            {
                MensajeError = null;
                var reportes = _reporteService.ObtenerReporteResumenGeneral(FechaInicio, FechaFin);
                ReportesResumen = new ObservableCollection<ReporteResumenGeneral>(reportes);
                TipoReporteSeleccionado = "Resumen General";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al generar reporte: {ex.Message}";
            }
        }

        #endregion
    }
}
