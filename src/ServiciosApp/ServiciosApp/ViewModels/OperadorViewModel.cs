using Core.ServiciosApp.Entities;
using ServiciosApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ServiciosApp.ViewModels
{
    public class OperadorViewModel : ViewModelBase
    {
        private readonly IOperadorService _operadorService;

        private Operador _operadorSeleccionado;
        private ObservableCollection<Operador> _operadores;
        private string _nombre;
        private string _licencia;
        private string _telefono;
        private bool _disponible;
        private string _mensajeError;
        private string _mensajeExito;

        public OperadorViewModel(IOperadorService operadorService)
        {
            _operadorService = operadorService ?? throw new ArgumentNullException(nameof(operadorService));
            CargarDatos();
            InitializeCommands();
        }

        #region Properties

        public Operador OperadorSeleccionado
        {
            get { return _operadorSeleccionado; }
            set 
            { 
                if (SetProperty(ref _operadorSeleccionado, value))
                {
                    CargarFormulario();
                }
            }
        }

        public ObservableCollection<Operador> Operadores
        {
            get { return _operadores; }
            set { SetProperty(ref _operadores, value); }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { SetProperty(ref _nombre, value); }
        }

        public string Licencia
        {
            get { return _licencia; }
            set { SetProperty(ref _licencia, value); }
        }

        public string Telefono
        {
            get { return _telefono; }
            set { SetProperty(ref _telefono, value); }
        }

        public bool Disponible
        {
            get { return _disponible; }
            set { SetProperty(ref _disponible, value); }
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

        public ICommand CrearOperadorCommand { get; private set; }
        public ICommand ActualizarOperadorCommand { get; private set; }
        public ICommand EliminarOperadorCommand { get; private set; }
        public ICommand CambiarDisponibilidadCommand { get; private set; }
        public ICommand RefrescarCommand { get; private set; }
        public ICommand LimpiarFormularioCommand { get; private set; }

        #endregion

        #region Methods

        private void CargarDatos()
        {
            try
            {
                Operadores = new ObservableCollection<Operador>(_operadorService.ObtenerActivos());
                MensajeError = null;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar operadores: {ex.Message}";
            }
        }

        private void InitializeCommands()
        {
            CrearOperadorCommand = new RelayCommand(() => CrearOperador(), () => CanCrearOperador());
            ActualizarOperadorCommand = new RelayCommand(() => ActualizarOperador(), () => CanActualizarOperador());
            EliminarOperadorCommand = new RelayCommand(() => EliminarOperador(), () => CanEliminarOperador());
            CambiarDisponibilidadCommand = new RelayCommand(() => CambiarDisponibilidad(), () => OperadorSeleccionado != null);
            RefrescarCommand = new RelayCommand(() => CargarDatos());
            LimpiarFormularioCommand = new RelayCommand(() => LimpiarFormulario());
        }

        private void CargarFormulario()
        {
            if (OperadorSeleccionado != null)
            {
                Nombre = OperadorSeleccionado.Nombre;
                Licencia = OperadorSeleccionado.Licencia;
                Telefono = OperadorSeleccionado.Telefono;
                Disponible = OperadorSeleccionado.Disponible;
            }
        }

        private void CrearOperador()
        {
            try
            {
                MensajeError = null;
                MensajeExito = null;

                var operador = new Operador
                {
                    Nombre = Nombre,
                    Licencia = Licencia,
                    Telefono = Telefono,
                    Disponible = true
                };

                _operadorService.CrearOperador(operador);
                MensajeExito = "Operador creado exitosamente";
                LimpiarFormulario();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al crear operador: {ex.Message}";
            }
        }

        private void ActualizarOperador()
        {
            try
            {
                if (OperadorSeleccionado == null)
                {
                    MensajeError = "Debe seleccionar un operador";
                    return;
                }

                MensajeError = null;
                MensajeExito = null;

                OperadorSeleccionado.Nombre = Nombre;
                OperadorSeleccionado.Licencia = Licencia;
                OperadorSeleccionado.Telefono = Telefono;
                OperadorSeleccionado.Disponible = Disponible;

                _operadorService.ActualizarOperador(OperadorSeleccionado);
                MensajeExito = "Operador actualizado exitosamente";
                CargarDatos();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al actualizar operador: {ex.Message}";
            }
        }

        private void EliminarOperador()
        {
            try
            {
                if (OperadorSeleccionado == null)
                {
                    MensajeError = "Debe seleccionar un operador";
                    return;
                }

                MensajeError = null;
                MensajeExito = null;

                _operadorService.EliminarOperador(OperadorSeleccionado.Id);
                MensajeExito = "Operador eliminado exitosamente";
                OperadorSeleccionado = null;
                LimpiarFormulario();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar operador: {ex.Message}";
            }
        }

        private void CambiarDisponibilidad()
        {
            try
            {
                if (OperadorSeleccionado == null)
                {
                    MensajeError = "Debe seleccionar un operador";
                    return;
                }

                MensajeError = null;
                MensajeExito = null;

                _operadorService.CambiarDisponibilidad(OperadorSeleccionado.Id, !OperadorSeleccionado.Disponible);
                MensajeExito = $"Operador marcado como {(!OperadorSeleccionado.Disponible ? "disponible" : "no disponible")}";
                CargarDatos();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cambiar disponibilidad: {ex.Message}";
            }
        }

        private bool CanCrearOperador()
        {
            return !string.IsNullOrWhiteSpace(Nombre) &&
                   !string.IsNullOrWhiteSpace(Licencia);
        }

        private bool CanActualizarOperador()
        {
            return OperadorSeleccionado != null &&
                   !string.IsNullOrWhiteSpace(Nombre) &&
                   !string.IsNullOrWhiteSpace(Licencia);
        }

        private bool CanEliminarOperador()
        {
            return OperadorSeleccionado != null;
        }

        private void LimpiarFormulario()
        {
            Nombre = null;
            Licencia = null;
            Telefono = null;
            Disponible = true;
            OperadorSeleccionado = null;
            MensajeError = null;
            MensajeExito = null;
        }

        #endregion
    }
}
