using Core.ServiciosApp.Entities;
using Core.ServiciosApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiciosApp.Services
{
    public interface IOperadorService
    {
        IEnumerable<Operador> ObtenerTodos();
        IEnumerable<Operador> ObtenerActivos();
        IEnumerable<Operador> ObtenerDisponibles();
        Operador ObtenerPorId(int id);
        int CrearOperador(Operador operador);
        void ActualizarOperador(Operador operador);
        void EliminarOperador(int id);
        void CambiarDisponibilidad(int id, bool disponible);
        bool LicenciaExiste(string licencia);
        IEnumerable<Servicio> ObtenerServiciosAsignados(int operadorId);
    }

    public class OperadorService : IOperadorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OperadorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Operador> ObtenerTodos()
        {
            return _unitOfWork.Operadores.GetAll();
        }

        public IEnumerable<Operador> ObtenerActivos()
        {
            return _unitOfWork.Operadores.Find(o => o.Activo);
        }

        public IEnumerable<Operador> ObtenerDisponibles()
        {
            return _unitOfWork.Operadores.Find(o => o.Activo && o.Disponible);
        }

        public Operador ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            return _unitOfWork.Operadores.GetById(id);
        }

        public int CrearOperador(Operador operador)
        {
            ValidarOperador(operador);

            operador.FechaRegistro = DateTime.Now;
            operador.Activo = true;
            operador.Disponible = true;

            _unitOfWork.Operadores.Add(operador);
            return _unitOfWork.SaveChanges();
        }

        public void ActualizarOperador(Operador operador)
        {
            ValidarOperador(operador);
            operador.FechaModificacion = DateTime.Now;
            _unitOfWork.Operadores.Update(operador);
            _unitOfWork.SaveChanges();
        }

        public void EliminarOperador(int id)
        {
            var operador = ObtenerPorId(id);
            if (operador == null)
                throw new InvalidOperationException($"No se encontró el operador con ID {id}");

            var serviciosAsignados = _unitOfWork.Servicios.Find(s =>
                s.OperadorId == id &&
                (s.Estado == EstadoServicio.EnProceso || s.Estado == EstadoServicio.Asignado))
                .ToList();

            if (serviciosAsignados.Any())
                throw new InvalidOperationException(
                    $"No se puede eliminar el operador, tiene {serviciosAsignados.Count} servicios asignados en proceso");

            operador.Activo = false;
            ActualizarOperador(operador);
        }

        public void CambiarDisponibilidad(int id, bool disponible)
        {
            var operador = ObtenerPorId(id);
            if (operador == null)
                throw new InvalidOperationException($"No se encontró el operador con ID {id}");

            operador.Disponible = disponible;
            ActualizarOperador(operador);
        }

        public bool LicenciaExiste(string licencia)
        {
            if (string.IsNullOrWhiteSpace(licencia))
                throw new ArgumentException("La licencia no puede estar vacía", nameof(licencia));

            return _unitOfWork.Operadores.Any(o => o.Licencia == licencia && o.Activo);
        }

        public IEnumerable<Servicio> ObtenerServiciosAsignados(int operadorId)
        {
            if (operadorId <= 0)
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(operadorId));

            var operador = ObtenerPorId(operadorId);
            if (operador == null)
                throw new InvalidOperationException($"No se encontró el operador con ID {operadorId}");

            return _unitOfWork.Servicios.Find(s => s.OperadorId == operadorId);
        }

        private void ValidarOperador(Operador operador)
        {
            if (operador == null)
                throw new ArgumentNullException(nameof(operador));

            if (string.IsNullOrWhiteSpace(operador.Nombre))
                throw new ArgumentException("El nombre es obligatorio", nameof(operador.Nombre));

            if (string.IsNullOrWhiteSpace(operador.Licencia))
                throw new ArgumentException("La licencia es obligatoria", nameof(operador.Licencia));

            // Validar que la licencia sea única (exceptuando el mismo operador)
            var licenciaExistente = _unitOfWork.Operadores.Find(o =>
                o.Licencia == operador.Licencia &&
                o.Id != operador.Id &&
                o.Activo).FirstOrDefault();

            if (licenciaExistente != null)
                throw new InvalidOperationException($"Ya existe un operador con la licencia '{operador.Licencia}'");
        }
    }
}
