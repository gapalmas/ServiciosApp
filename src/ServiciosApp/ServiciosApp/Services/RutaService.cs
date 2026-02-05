using Core.ServiciosApp.Entities;
using Core.ServiciosApp.Interfaces;
using System;
using System.Collections.Generic;

namespace ServiciosApp.Services
{
    public interface IRutaService
    {
        IEnumerable<Ruta> ObtenerTodos();
        IEnumerable<Ruta> ObtenerActivas();
        Ruta ObtenerPorId(int id);
        int CrearRuta(Ruta ruta);
        void ActualizarRuta(Ruta ruta);
        void EliminarRuta(int id);
        IEnumerable<Ruta> ObtenerPorZona(string zona);
        IEnumerable<Ruta> ObtenerPorDistancia(decimal distanciaMin, decimal distanciaMax);
    }

    public class RutaService : IRutaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RutaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Ruta> ObtenerTodos()
        {
            return _unitOfWork.Rutas.GetAll();
        }

        public IEnumerable<Ruta> ObtenerActivas()
        {
            return _unitOfWork.Rutas.Find(r => r.Activo);
        }

        public Ruta ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            return _unitOfWork.Rutas.GetById(id);
        }

        public int CrearRuta(Ruta ruta)
        {
            ValidarRuta(ruta);

            ruta.FechaRegistro = DateTime.Now;
            ruta.Activo = true;

            _unitOfWork.Rutas.Add(ruta);
            return _unitOfWork.SaveChanges();
        }

        public void ActualizarRuta(Ruta ruta)
        {
            ValidarRuta(ruta);
            ruta.FechaModificacion = DateTime.Now;
            _unitOfWork.Rutas.Update(ruta);
            _unitOfWork.SaveChanges();
        }

        public void EliminarRuta(int id)
        {
            var ruta = ObtenerPorId(id);
            if (ruta == null)
                throw new InvalidOperationException($"No se encontró la ruta con ID {id}");

            ruta.Activo = false;
            ActualizarRuta(ruta);
        }

        public IEnumerable<Ruta> ObtenerPorZona(string zona)
        {
            if (string.IsNullOrWhiteSpace(zona))
                throw new ArgumentException("La zona no puede estar vacía", nameof(zona));

            return _unitOfWork.Rutas.Find(r => r.Zona == zona && r.Activo);
        }

        public IEnumerable<Ruta> ObtenerPorDistancia(decimal distanciaMin, decimal distanciaMax)
        {
            if (distanciaMin < 0 || distanciaMax < 0)
                throw new ArgumentException("Las distancias no pueden ser negativas");

            if (distanciaMin > distanciaMax)
                throw new ArgumentException("La distancia mínima no puede ser mayor a la máxima");

            return _unitOfWork.Rutas.Find(r =>
                r.Activo &&
                r.DistanciaKm >= distanciaMin &&
                r.DistanciaKm <= distanciaMax);
        }

        private void ValidarRuta(Ruta ruta)
        {
            if (ruta == null)
                throw new ArgumentNullException(nameof(ruta));

            if (string.IsNullOrWhiteSpace(ruta.Nombre))
                throw new ArgumentException("El nombre es obligatorio", nameof(ruta.Nombre));

            if (ruta.DistanciaKm <= 0)
                throw new ArgumentException("La distancia debe ser mayor a 0", nameof(ruta.DistanciaKm));

            if (ruta.TiempoEstimadoMinutos <= 0)
                throw new ArgumentException("El tiempo estimado debe ser mayor a 0", nameof(ruta.TiempoEstimadoMinutos));
        }
    }
}
