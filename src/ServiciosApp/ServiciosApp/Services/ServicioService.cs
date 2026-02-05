using Core.ServiciosApp.Entities;
using Core.ServiciosApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiciosApp.Services
{
    public interface IServicioService
    {
        IEnumerable<Servicio> ObtenerTodos();
        Servicio ObtenerPorId(int id);
        IEnumerable<Servicio> ObtenerPorCliente(int clienteId);
        IEnumerable<Servicio> ObtenerPendientes();
        IEnumerable<Servicio> ObtenerAsignados();
        IEnumerable<Servicio> ObtenerCompletados();
        IEnumerable<Servicio> ObtenerPorFecha(DateTime fechaInicio, DateTime fechaFin);
        int CrearServicio(Servicio servicio);
        void ActualizarServicio(Servicio servicio);
        void EliminarServicio(int id);
        void AsignarOperador(int servicioId, int operadorId);
        void AsignarRuta(int servicioId, int rutaId);
        void CambiarEstado(int servicioId, EstadoServicio nuevoEstado);
        void DesasignarOperador(int servicioId);
        void DesasignarRuta(int servicioId);
    }

    public class ServicioService : IServicioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Servicio> ObtenerTodos()
        {
            return _unitOfWork.Servicios.GetAll();
        }

        public Servicio ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            return _unitOfWork.Servicios.GetById(id);
        }

        public IEnumerable<Servicio> ObtenerPorCliente(int clienteId)
        {
            if (clienteId <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor a 0", nameof(clienteId));
            return _unitOfWork.Servicios.Find(s => s.ClienteId == clienteId);
        }

        public IEnumerable<Servicio> ObtenerPendientes()
        {
            return _unitOfWork.Servicios.Find(s => s.Estado == EstadoServicio.Pendiente);
        }

        public IEnumerable<Servicio> ObtenerAsignados()
        {
            return _unitOfWork.Servicios.Find(s => s.OperadorId.HasValue && s.RutaId.HasValue);
        }

        public IEnumerable<Servicio> ObtenerCompletados()
        {
            return _unitOfWork.Servicios.Find(s => s.Estado == EstadoServicio.Completado);
        }

        public IEnumerable<Servicio> ObtenerPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return _unitOfWork.Servicios.Find(s => 
                s.FechaSolicitud >= fechaInicio && 
                s.FechaSolicitud <= fechaFin);
        }

        public int CrearServicio(Servicio servicio)
        {
            ValidarServicio(servicio);
            servicio.FechaSolicitud = DateTime.Now;
            servicio.Estado = EstadoServicio.Pendiente;
            servicio.FechaRegistro = DateTime.Now;
            servicio.Activo = true;

            _unitOfWork.Servicios.Add(servicio);
            return _unitOfWork.SaveChanges();
        }

        public void ActualizarServicio(Servicio servicio)
        {
            ValidarServicio(servicio);
            servicio.FechaModificacion = DateTime.Now;
            _unitOfWork.Servicios.Update(servicio);
            _unitOfWork.SaveChanges();
        }

        public void EliminarServicio(int id)
        {
            var servicio = ObtenerPorId(id);
            if (servicio == null)
                throw new InvalidOperationException($"No se encontró el servicio con ID {id}");

            if (servicio.Estado == EstadoServicio.EnProceso)
                throw new InvalidOperationException("No se pueden eliminar servicios en proceso");

            _unitOfWork.Servicios.Remove(servicio);
            _unitOfWork.SaveChanges();
        }

        public void AsignarOperador(int servicioId, int operadorId)
        {
            var servicio = ObtenerPorId(servicioId);
            if (servicio == null)
                throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");

            var operador = _unitOfWork.Operadores.GetById(operadorId);
            if (operador == null)
                throw new InvalidOperationException($"No se encontró el operador con ID {operadorId}");

            if (!operador.Disponible)
                throw new InvalidOperationException("El operador no está disponible");

            servicio.OperadorId = operadorId;
            servicio.FechaAsignacion = DateTime.Now;
            if (servicio.Estado == EstadoServicio.Pendiente)
                servicio.Estado = EstadoServicio.Asignado;

            ActualizarServicio(servicio);
        }

        public void AsignarRuta(int servicioId, int rutaId)
        {
            var servicio = ObtenerPorId(servicioId);
            if (servicio == null)
                throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");

            var ruta = _unitOfWork.Rutas.GetById(rutaId);
            if (ruta == null)
                throw new InvalidOperationException($"No se encontró la ruta con ID {rutaId}");

            servicio.RutaId = rutaId;
            ActualizarServicio(servicio);
        }

        public void CambiarEstado(int servicioId, EstadoServicio nuevoEstado)
        {
            var servicio = ObtenerPorId(servicioId);
            if (servicio == null)
                throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");

            servicio.Estado = nuevoEstado;

            if (nuevoEstado == EstadoServicio.Completado)
                servicio.FechaCompletado = DateTime.Now;

            ActualizarServicio(servicio);
        }

        public void DesasignarOperador(int servicioId)
        {
            var servicio = ObtenerPorId(servicioId);
            if (servicio == null)
                throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");

            servicio.OperadorId = null;
            ActualizarServicio(servicio);
        }

        public void DesasignarRuta(int servicioId)
        {
            var servicio = ObtenerPorId(servicioId);
            if (servicio == null)
                throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");

            servicio.RutaId = null;
            ActualizarServicio(servicio);
        }

        private void ValidarServicio(Servicio servicio)
        {
            if (servicio == null)
                throw new ArgumentNullException(nameof(servicio));

            if (string.IsNullOrWhiteSpace(servicio.Descripcion))
                throw new ArgumentException("La descripción es obligatoria", nameof(servicio.Descripcion));

            if (servicio.Costo <= 0)
                throw new ArgumentException("El costo debe ser mayor a 0", nameof(servicio.Costo));

            if (servicio.ClienteId <= 0)
                throw new ArgumentException("El cliente es obligatorio", nameof(servicio.ClienteId));

            var clienteExiste = _unitOfWork.Clientes.Any(c => c.Id == servicio.ClienteId && c.Activo);
            if (!clienteExiste)
                throw new InvalidOperationException($"El cliente con ID {servicio.ClienteId} no existe o está inactivo");
        }
    }
}
