using Core.ServiciosApp.Interfaces;
using Infrastructure.ServiciosApp.Data;
using Infrastructure.ServiciosApp.Repositories;
using ServiciosApp.Services;
using ServiciosApp.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiciosApp
{
    public class ServiceLocator
    {
        private static readonly Lazy<ServiceLocator> _instance = new Lazy<ServiceLocator>(() => new ServiceLocator());
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static ServiceLocator Instance => _instance.Value;

        public void RegisterServices()
        {
            // Registrar DbContext
            var dbContext = new SqlDbContext();
            _services[typeof(SqlDbContext)] = dbContext;

            // Registrar UnitOfWork
            var unitOfWork = new UnitOfWork(dbContext);
            _services[typeof(IUnitOfWork)] = unitOfWork;

            // Registrar Repositories
            var reporteRepository = new ReporteRepository(dbContext);
            _services[typeof(IReporteRepository)] = reporteRepository;

            // Registrar Application Services
            var clienteService = new ClienteService(unitOfWork);
            var operadorService = new OperadorService(unitOfWork);
            var rutaService = new RutaService(unitOfWork);
            var servicioService = new ServicioService(unitOfWork);
            var reporteService = new ReporteService(reporteRepository);

            _services[typeof(IClienteService)] = clienteService;
            _services[typeof(IOperadorService)] = operadorService;
            _services[typeof(IRutaService)] = rutaService;
            _services[typeof(IServicioService)] = servicioService;
            _services[typeof(IReporteService)] = reporteService;

            // Registrar ViewModels
            var servicioViewModel = new ServicioViewModel(servicioService, clienteService, operadorService, rutaService);
            var operadorViewModel = new OperadorViewModel(operadorService);
            var asignacionViewModel = new AsignacionViewModel(servicioService, operadorService, rutaService);
            var reporteViewModel = new ReporteViewModel(reporteService);

            _services[typeof(ServicioViewModel)] = servicioViewModel;
            _services[typeof(OperadorViewModel)] = operadorViewModel;
            _services[typeof(AsignacionViewModel)] = asignacionViewModel;
            _services[typeof(ReporteViewModel)] = reporteViewModel;
        }

        public T Resolve<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }
            throw new InvalidOperationException($"Servicio de tipo {type.Name} no está registrado");
        }

        public void Dispose()
        {
            foreach (var service in _services.Values)
            {
                (service as IDisposable)?.Dispose();
            }
            _services.Clear();
        }
    }
}
