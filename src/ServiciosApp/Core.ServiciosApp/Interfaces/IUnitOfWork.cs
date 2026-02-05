using Core.ServiciosApp.Entities;
using System;

namespace Core.ServiciosApp.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Cliente> Clientes { get; }
        IRepository<Operador> Operadores { get; }
        IRepository<Ruta> Rutas { get; }
        IRepository<Servicio> Servicios { get; }

        int SaveChanges();
    }
}
