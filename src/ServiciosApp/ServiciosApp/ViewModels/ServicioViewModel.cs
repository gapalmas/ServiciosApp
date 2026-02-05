using Core.ServiciosApp.Entities;
using ServiciosApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ServiciosApp.ViewModels
{
    public class ServicioViewModel : ViewModelBase
    {
        private readonly IServicioService _servicioService;
        private readonly IClienteService _clienteService;
        private readonly IOperadorService _operadorService;
        private readonly IRutaService _rutaService;

        private Servicio _servicioSeleccionado;
        private ObservableCollection<Servicio> _servicios;
        private ObservableCollection<Cliente> _clientes;
        private ObservableCollection<Operador> _operadoresDisponibles;
        private ObservableCollection<Ruta> _rutas;
        private string _descripcion;
        private string _direccionOrigen;
        private string _direccionDestino;
        private decimal _costo;
        private TipoServicio _tipo;
        private int _clienteSeleccionadoId;
        private string _mensajeError;
        private string _mensajeExito;

        public ServicioViewModel(
            IServicioService servicioService,
            IClienteService clienteService,
            IOperadorService operadorService,
            IRutaService rutaService)
        {
            _servicioService = servicioService ?? throw new ArgumentNullException(nameof(servicioService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
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

        public ObservableCollection<Servicio> Servicios
        {
            get { return _servicios; }
            set { SetProperty(ref _servicios, value); }
        }

        public ObservableCollection<Cliente> Clientes
        {
            get { return _clientes; }
            set { SetProperty(ref _clientes, value); }
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

        public string Descripcion
        {
            get { return _descripcion; }
            set { SetProperty(ref _descripcion, value); }
        }

        public string DireccionOrigen
        {
            get { return _direccionOrigen; }
            set { SetProperty(ref _direccionOrigen, value); }
        }

        public string DireccionDestino
        {
            get { return _direccionDestino; }
            set { SetProperty(ref _direccionDestino, value); }
        }

        public decimal Costo
        {
            get { return _costo; }
            set { SetProperty(ref _costo, value); }
        }

        public TipoServicio Tipo
        {
            get { return _tipo; }
            set { SetProperty(ref _tipo, value); }
        }

        public int ClienteSeleccionadoId
        {
            get { return _clienteSeleccionadoId; }
            set { SetProperty(ref _clienteSeleccionadoId, value); }
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

        public ICommand CrearServicioCommand { get; private set; }
        public ICommand ActualizarServicioCommand { get; private set; }
        public ICommand EliminarServicioCommand { get; private set; }
        public ICommand RefrescarCommand { get; private set; }

        #endregion

        #region Methods

        private void CargarDatos()
        {
            try
            {
                Servicios = new ObservableCollection<Servicio>(_servicioService.ObtenerTodos());
                Clientes = new ObservableCollection<Cliente>(_clienteService.ObtenerActivos());
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
            CrearServicioCommand = new RelayCommand(() => CrearServicio(), () => CanCrearServicio());
            ActualizarServicioCommand = new RelayCommand(() => ActualizarServicio(), () => CanActualizarServicio());
            EliminarServicioCommand = new RelayCommand(() => EliminarServicio(), () => CanEliminarServicio());
            RefrescarCommand = new RelayCommand(() => CargarDatos());
        }

        private void CrearServicio()
        {
            try
            {
                MensajeError = null;
                MensajeExito = null;

                var servicio = new Servicio
                {
                    Descripcion = Descripcion,
                    DireccionOrigen = DireccionOrigen,
                    DireccionDestino = DireccionDestino,
                    Costo = Costo,
                    Tipo = Tipo,
                    ClienteId = ClienteSeleccionadoId,
                    Estado = EstadoServicio.Pendiente
                };

                _servicioService.CrearServicio(servicio);
                MensajeExito = "Servicio creado exitosamente";
                LimpiarFormulario();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al crear servicio: {ex.Message}";
            }
        }

        private void ActualizarServicio()
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

                _servicioService.ActualizarServicio(ServicioSeleccionado);
                MensajeExito = "Servicio actualizado exitosamente";
                CargarDatos();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al actualizar servicio: {ex.Message}";
            }
        }

        private void EliminarServicio()
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

                _servicioService.EliminarServicio(ServicioSeleccionado.Id);
                MensajeExito = "Servicio eliminado exitosamente";
                ServicioSeleccionado = null;
                CargarDatos();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar servicio: {ex.Message}";
            }
        }

        private bool CanCrearServicio()
        {
            return !string.IsNullOrWhiteSpace(Descripcion) &&
                   Costo > 0 &&
                   ClienteSeleccionadoId > 0;
        }

        private bool CanActualizarServicio()
        {
            return ServicioSeleccionado != null;
        }

        private bool CanEliminarServicio()
        {
            return ServicioSeleccionado != null &&
                   ServicioSeleccionado.Estado != EstadoServicio.EnProceso;
        }

        private void LimpiarFormulario()
        {
            Descripcion = null;
            DireccionOrigen = null;
            DireccionDestino = null;
            Costo = 0;
            ClienteSeleccionadoId = 0;
        }

        #endregion
    }
}
