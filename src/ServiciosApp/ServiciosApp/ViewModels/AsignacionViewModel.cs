using Core.ServiciosApp.Entities;
using ServiciosApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ServiciosApp.ViewModels
{
    public class AsignacionViewModel : ViewModelBase
    {
        private readonly IServicioService _servicioService;
        private readonly IOperadorService _operadorService;
        private readonly IRutaService _rutaService;

        private Servicio _servicioSeleccionado;
        private ObservableCollection<Servicio> _serviciosPendientes;
        private ObservableCollection<Operador> _operadoresDisponibles;
        private ObservableCollection<Ruta> _rutas;
        private int _operadorSeleccionadoId;
        private int _rutaSeleccionadaId;
        private string _mensajeError;
        private string _mensajeExito;

        public AsignacionViewModel(
            IServicioService servicioService,
            IOperadorService operadorService,
            IRutaService rutaService)
        {
            _servicioService = servicioService ?? throw new ArgumentNullException(nameof(servicioService));
            _operadorService = operadorService ?? throw new ArgumentNullException(nameof(operadorService));
            _rutaService = rutaService ?? throw new ArgumentNullException(nameof(rutaService));

            CargarDatos();
            InitializeCommands();
        }

        #region Properties

        public Servicio ServicioSeleccionado
        {
            get { return _servicioSeleccionado; }
            set { SetProperty(ref _servicioSeleccionado, value); }
        }

        public ObservableCollection<Servicio> ServiciosPendientes
        {
            get { return _serviciosPendientes; }
            set { SetProperty(ref _serviciosPendientes, value); }
        }

        public ObservableCollection<Operador> OperadoresDisponibles
        {
            get { return _operadoresDisponibles; }
            set { SetProperty(ref _operadoresDisponibles, value); }
        }

        public ObservableCollection<Ruta> Rutas
        {
            get { return _rutas; }
            set { SetProperty(ref _rutas, value); }
        }

        public int OperadorSeleccionadoId
        {
            get { return _operadorSeleccionadoId; }
            set { SetProperty(ref _operadorSeleccionadoId, value); }
        }

        public int RutaSeleccionadaId
        {
            get { return _rutaSeleccionadaId; }
            set { SetProperty(ref _rutaSeleccionadaId, value); }
        }

        public string MensajeError
        {
            get { return _mensajeError; }
            set { SetProperty(ref _mensajeError, value); }
        }

        public string MensajeExito
        {
            get { return _mensajeExito; }
            set { SetProperty(ref _mensajeExito, value); }
        }

        #endregion

        #region Commands

        public ICommand AsignarOperadorCommand { get; private set; }
        public ICommand AsignarRutaCommand { get; private set; }
        public ICommand DesasignarOperadorCommand { get; private set; }
        public ICommand DesasignarRutaCommand { get; private set; }
        public ICommand RefrescarCommand { get; private set; }

        #endregion

        #region Methods

        private void CargarDatos()
        {
            try
            {
                ServiciosPendientes = new ObservableCollection<Servicio>(_servicioService.ObtenerPendientes());
                OperadoresDisponibles = new ObservableCollection<Operador>(_operadorService.ObtenerDisponibles());
                Rutas = new ObservableCollection<Ruta>(_rutaService.ObtenerActivas());
                MensajeError = null;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar datos: {ex.Message}";
            }
        }

        private void InitializeCommands()
        {
            AsignarOperadorCommand = new RelayCommand(() => AsignarOperador(), () => CanAsignarOperador());
            AsignarRutaCommand = new RelayCommand(() => AsignarRuta(), () => CanAsignarRuta());
            DesasignarOperadorCommand = new RelayCommand(() => DesasignarOperador(), () => CanDesasignar());
            DesasignarRutaCommand = new RelayCommand(() => DesasignarRuta(), () => CanDesasignar());
            RefrescarCommand = new RelayCommand(() => CargarDatos());
        }

        private void AsignarOperador()
        {
            try
            {
                if (ServicioSeleccionado == null || OperadorSeleccionadoId <= 0)
                {
                    MensajeError = "Debe seleccionar un servicio y un operador";
                    return;
                }

                MensajeError = null;
                MensajeExito = null;

                _servicioService.AsignarOperador(ServicioSeleccionado.Id, OperadorSeleccionadoId);
                MensajeExito = "Operador asignado exitosamente";
                CargarDatos();
                ServicioSeleccionado = null;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al asignar operador: {ex.Message}";
            }
        }

        private void AsignarRuta()
        {
            try
            {
                if (ServicioSeleccionado == null || RutaSeleccionadaId <= 0)
                {
                    MensajeError = "Debe seleccionar un servicio y una ruta";
                    return;
                }

                MensajeError = null;
                MensajeExito = null;

                _servicioService.AsignarRuta(ServicioSeleccionado.Id, RutaSeleccionadaId);
                MensajeExito = "Ruta asignada exitosamente";
                CargarDatos();
                ServicioSeleccionado = null;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al asignar ruta: {ex.Message}";
            }
        }

        private void DesasignarOperador()
        {
            try
            {
                if (ServicioSeleccionado == null)
                {
                    MensajeError = "Debe seleccionar un servicio";
                    return;
                }

                MensajeError = null;
                MensajeExito = null;

                _servicioService.DesasignarOperador(ServicioSeleccionado.Id);
                MensajeExito = "Operador desasignado exitosamente";
                CargarDatos();
                ServicioSeleccionado = null;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al desasignar operador: {ex.Message}";
            }
        }

        private void DesasignarRuta()
        {
            try
            {
                if (ServicioSeleccionado == null)
                {
                    MensajeError = "Debe seleccionar un servicio";
                    return;
                }

                MensajeError = null;
                MensajeExito = null;

                _servicioService.DesasignarRuta(ServicioSeleccionado.Id);
                MensajeExito = "Ruta desasignada exitosamente";
                CargarDatos();
                ServicioSeleccionado = null;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al desasignar ruta: {ex.Message}";
            }
        }

        private bool CanAsignarOperador()
        {
            return ServicioSeleccionado != null && OperadorSeleccionadoId > 0;
        }

        private bool CanAsignarRuta()
        {
            return ServicioSeleccionado != null && RutaSeleccionadaId > 0;
        }

        private bool CanDesasignar()
        {
            return ServicioSeleccionado != null;
        }

        #endregion
    }
}
