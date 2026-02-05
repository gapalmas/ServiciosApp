using Core.ServiciosApp.Entities;
using Core.ServiciosApp.Interfaces;
using Infrastructure.ServiciosApp.Data;
using System;

namespace Infrastructure.ServiciosApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlDbContext _context;

        private IRepository<Cliente> _clienteRepository;
        private IRepository<Operador> _operadorRepository;
        private IRepository<Ruta> _rutaRepository;
        private IRepository<Servicio> _servicioRepository;

        public UnitOfWork(SqlDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<Cliente> Clientes
        {
            get { return _clienteRepository ?? (_clienteRepository = new Repository<Cliente>(_context)); }
        }

        public IRepository<Operador> Operadores
        {
            get { return _operadorRepository ?? (_operadorRepository = new Repository<Operador>(_context)); }
        }

        public IRepository<Ruta> Rutas
        {
            get { return _rutaRepository ?? (_rutaRepository = new Repository<Ruta>(_context)); }
        }

        public IRepository<Servicio> Servicios
        {
            get { return _servicioRepository ?? (_servicioRepository = new Repository<Servicio>(_context)); }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
