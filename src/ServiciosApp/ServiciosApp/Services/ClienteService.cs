using Core.ServiciosApp.Entities;
using Core.ServiciosApp.Interfaces;
using System;
using System.Collections.Generic;

namespace ServiciosApp.Services
{
    public interface IClienteService
    {
        IEnumerable<Cliente> ObtenerTodos();
        IEnumerable<Cliente> ObtenerActivos();
        Cliente ObtenerPorId(int id);
        int CrearCliente(Cliente cliente);
        void ActualizarCliente(Cliente cliente);
        void EliminarCliente(int id);
        bool EmailExiste(string email);
        bool RFCExiste(string rfc);
    }

    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Cliente> ObtenerTodos()
        {
            return _unitOfWork.Clientes.GetAll();
        }

        public IEnumerable<Cliente> ObtenerActivos()
        {
            return _unitOfWork.Clientes.Find(c => c.Activo);
        }

        public Cliente ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            return _unitOfWork.Clientes.GetById(id);
        }

        public int CrearCliente(Cliente cliente)
        {
            ValidarCliente(cliente);

            cliente.FechaRegistro = DateTime.Now;
            cliente.Activo = true;

            _unitOfWork.Clientes.Add(cliente);
            return _unitOfWork.SaveChanges();
        }

        public void ActualizarCliente(Cliente cliente)
        {
            ValidarCliente(cliente);
            cliente.FechaModificacion = DateTime.Now;
            _unitOfWork.Clientes.Update(cliente);
            _unitOfWork.SaveChanges();
        }

        public void EliminarCliente(int id)
        {
            var cliente = ObtenerPorId(id);
            if (cliente == null)
                throw new InvalidOperationException($"No se encontró el cliente con ID {id}");

            cliente.Activo = false;
            ActualizarCliente(cliente);
        }

        public bool EmailExiste(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío", nameof(email));

            return _unitOfWork.Clientes.Any(c => c.Email == email && c.Activo);
        }

        public bool RFCExiste(string rfc)
        {
            if (string.IsNullOrWhiteSpace(rfc))
                throw new ArgumentException("El RFC no puede estar vacío", nameof(rfc));

            return _unitOfWork.Clientes.Any(c => c.RFC == rfc && c.Activo);
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ArgumentException("El nombre es obligatorio", nameof(cliente.Nombre));

            // Validar que el email sea único
            if (!string.IsNullOrWhiteSpace(cliente.Email))
            {
                var emailExistente = _unitOfWork.Clientes.Find(c =>
                    c.Email == cliente.Email &&
                    c.Id != cliente.Id &&
                    c.Activo);

                foreach (var item in emailExistente)
                {
                    throw new InvalidOperationException($"Ya existe un cliente con el email '{cliente.Email}'");
                }
            }

            // Validar que el RFC sea único
            if (!string.IsNullOrWhiteSpace(cliente.RFC))
            {
                var rfcExistente = _unitOfWork.Clientes.Find(c =>
                    c.RFC == cliente.RFC &&
                    c.Id != cliente.Id &&
                    c.Activo);

                foreach (var item in rfcExistente)
                {
                    throw new InvalidOperationException($"Ya existe un cliente con el RFC '{cliente.RFC}'");
                }
            }
        }
    }
}
