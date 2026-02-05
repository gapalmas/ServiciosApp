using Core.ServiciosApp.Entities;
using Infrastructure.ServiciosApp.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SqlDbContext _context;

        public ObservableCollection<Cliente> Clientes { get; set; }
        public ObservableCollection<Servicio> Servicios { get; set; }
        public ObservableCollection<Operador> Operadores { get; set; }

        public MainViewModel()
        {
            _context = new SqlDbContext();
            Clientes = new ObservableCollection<Cliente>();
            Servicios = new ObservableCollection<Servicio>();
            Operadores = new ObservableCollection<Operador>();

            CargarDatos();
        }

        private void CargarDatos()
        {
            
            var clientes = _context.Clientes.Where(c => c.Activo).ToList();
            Clientes.Clear();
            foreach (var cliente in clientes)
            {
                Clientes.Add(cliente);
            }

            var servicios = _context.Servicios
                .Include("Cliente")
                .Where(s => s.Estado == EstadoServicio.Pendiente)
                .ToList();

            Servicios.Clear();
            foreach (var servicio in servicios)
            {
                Servicios.Add(servicio);
            }

            // Cargar operadores disponibles
            var operadores = _context.Operadores
                .Where(o => o.Disponible && o.Activo)
                .ToList();

            Operadores.Clear();
            foreach (var operador in operadores)
            {
                Operadores.Add(operador);
            }
        }

        public void GuardarCambios()
        {
            _context.SaveChanges();
        }
    }
}
