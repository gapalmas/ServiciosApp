namespace Infrastructure.ServiciosApp.Migrations
{
    using Core.ServiciosApp.Entities;
    using Infrastructure.ServiciosApp.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SqlDbContext>
    {
        private Random _random = new Random();
        //private string[] _zonas = { "Norte", "Sur", "Este", "Oeste", "Centro" };
        private string[] _tiposCliente = { "Corporativo", "PyME", "Gubernamental", "Educativo", "Individual" };
        private string[] _sectores = { "Tecnologíia", "Logistica", "Manufactura", "Salud", "Educacion", "Retail", "Construccion" };

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SqlDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var fechaActual = DateTime.Now;
            var usuarioSistema = "SISTEMA";

            Console.WriteLine("--- INICIANDO SEED DE BASE DE DATOS ---");

            SeedClientes(context, fechaActual, usuarioSistema);

            SeedOperadores(context, fechaActual, usuarioSistema);

            SeedRutas(context, fechaActual, usuarioSistema);

            SeedServicios(context, fechaActual, usuarioSistema);

            MostrarEstadisticas(context);

            Console.WriteLine("--- SEED COMPLETADO EXITOSAMENTE ---");
        }


        #region Seeders

        private void SeedClientes(SqlDbContext context, DateTime fechaRegistro, string usuario)
        {
            if (!context.Clientes.Any())
            {
                Console.WriteLine("Creando clientes...");

                var clientes = new List<Cliente>
                {
                    new Cliente
                    {
                        Nombre = "Microsoft México SA de CV",
                        Direccion = "CDMX",
                        Telefono = "55-1234-5678",
                        Email = "ventas@microsoft.com.mx",
                        RFC = "MMX850101ABC",
                        Activo = true,
                        FechaRegistro = fechaRegistro.AddDays(-365),
                        UsuarioRegistro = usuario
                    }
                };

                // Clientes random
                for (int i = 11; i <= 15; i++)
                {
                    var tipoCliente = _tiposCliente[_random.Next(_tiposCliente.Length)];
                    var sector = _sectores[_random.Next(_sectores.Length)];

                    clientes.Add(new Cliente
                    {
                        Nombre = $"{sector} {tipoCliente} {i}",
                        Direccion = GenerarDireccionAleatoria(),
                        Telefono = $"55-{_random.Next(1000, 9999)}-{_random.Next(1000, 9999)}",
                        Email = $"cliente{i}@{sector.ToLower()}.com.mx",
                        RFC = GenerarRFCAleatorio(),
                        Activo = _random.Next(10) > 1,
                        FechaRegistro = fechaRegistro.AddDays(-_random.Next(1, 365)),
                        UsuarioRegistro = usuario,
                        UsuarioModificacion = _random.Next(3) == 0 ? "ADMIN" : null,
                        FechaModificacion = _random.Next(3) == 0 ? fechaRegistro.AddHours(-_random.Next(1, 24)) : (DateTime?)null
                    });
                }

                context.Clientes.AddOrUpdate(c => c.Email, clientes.ToArray());
                context.SaveChanges();

                Console.WriteLine($"+ {clientes.Count} clientes creados exitosamente.");
            }
            else
            {
                Console.WriteLine("+ Clientes ya existen en la base de datos.");
            }
        }

        private void SeedOperadores(SqlDbContext context, DateTime fechaRegistro, string usuario)
        {
            if (!context.Operadores.Any())
            {
                Console.WriteLine("Creando operadores...");

                var operadores = new List<Operador>
                {
                    new Operador
                    {
                        Nombre = "Pancho López",
                        Licencia = "OP-MX-2024-001",
                        Telefono = "55-1111-2222",
                        Disponible = true,
                        Activo = true,
                        FechaRegistro = fechaRegistro.AddMonths(-24),
                        UsuarioRegistro = usuario
                    }
                };

                for (int i = 8; i <= 10; i++)
                {
                    operadores.Add(new Operador
                    {
                        Nombre = GenerarNombreAleatorio(),
                        Licencia = $"OP-MX-2024-{i.ToString("D3")}",
                        Telefono = $"55-{_random.Next(1000, 9999)}-{_random.Next(1000, 9999)}",
                        Disponible = _random.Next(10) > 2, 
                        Activo = true,
                        FechaRegistro = fechaRegistro.AddDays(-_random.Next(30, 365)),
                        UsuarioRegistro = usuario
                    });
                }

                context.Operadores.AddOrUpdate(o => o.Licencia, operadores.ToArray());
                context.SaveChanges();
                Console.WriteLine($"+ {operadores.Count} operadores creados exitosamente.");
            }
            else
            {
                Console.WriteLine("+ Operadores ya existen en la base de datos.");
            }
        }

        private void SeedRutas(SqlDbContext context, DateTime fechaRegistro, string usuario)
        {
            if (!context.Rutas.Any())
            {
                Console.WriteLine("Creando rutas...");

                var rutas = new List<Ruta>
                {
                    new Ruta
                    {
                        Nombre = "Corredor Centro",
                        Descripcion = "Ruta principal",
                        DistanciaKm = 28.5m,
                        TiempoEstimadoMinutos = 90,
                        Zona = "Poniente-Centro",
                        Activo = true,
                        FechaRegistro = fechaRegistro.AddMonths(-12),
                        UsuarioRegistro = usuario
                    }
                };

                rutas.Add(new Ruta
                {
                    Nombre = "Ruta Universitaria",
                    Descripcion = "Conexion campus universitarios.",
                    DistanciaKm = 35.8m,
                    TiempoEstimadoMinutos = 110,
                    Zona = "Metropolitana",
                    Activo = true,
                    FechaRegistro = fechaRegistro.AddMonths(-1),
                    UsuarioRegistro = usuario
                });

                rutas.Add(new Ruta
                {
                    Nombre = "Juarez Porvernir",
                    Descripcion = "Ruta especial.",
                    DistanciaKm = 40.5m,
                    TiempoEstimadoMinutos = 135,
                    Zona = "Metropolitana",
                    Activo = false, 
                    FechaRegistro = fechaRegistro.AddMonths(-3),
                    FechaModificacion = fechaRegistro.AddDays(-15),
                    UsuarioRegistro = usuario,
                    UsuarioModificacion = "COORDINADOR"
                });

                context.Rutas.AddOrUpdate(r => r.Nombre, rutas.ToArray());
                context.SaveChanges();
                Console.WriteLine($"+ {rutas.Count} rutas creadas exitosamente.");
            }
            else
            {
                Console.WriteLine("+ Rutas ya existen en la base de datos.");
            }
        }

        private void SeedServicios(SqlDbContext context, DateTime fechaActual, string usuario)
        {
            if (!context.Servicios.Any())
            {
                Console.WriteLine("Creando servicios...");

                
                var clientes = context.Clientes.ToList();
                var operadores = context.Operadores.Where(o => o.Disponible && o.Activo).ToList();
                var rutas = context.Rutas.Where(r => r.Activo).ToList();

                if (!clientes.Any() || !operadores.Any() || !rutas.Any())
                {
                    Console.WriteLine("X No se pueden crear servicios. Faltan clientes, operadores o rutas.");
                    return;
                }

                var servicios = new List<Servicio>();
                var estados = Enum.GetValues(typeof(EstadoServicio)).Cast<EstadoServicio>().ToArray();
                var tipos = Enum.GetValues(typeof(TipoServicio)).Cast<TipoServicio>().ToArray();

                servicios.AddRange(CrearServiciosEjemplo(clientes, operadores, rutas, fechaActual, usuario));

                for (int i = 0; i < 40; i++)
                {
                    var cliente = clientes[_random.Next(clientes.Count)];
                    var operador = operadores[_random.Next(operadores.Count)];
                    var ruta = rutas[_random.Next(rutas.Count)];
                    var tipo = tipos[_random.Next(tipos.Length)];
                    var estado = estados[_random.Next(estados.Length)];

                    int prioridad = DeterminarPrioridad(cliente.Nombre, tipo);
                    decimal costo = CalcularCosto(tipo, ruta.DistanciaKm, prioridad);

                    DateTime fechaSolicitud = fechaActual.AddDays(-_random.Next(1, 60));
                    DateTime? fechaAsignacion = null;
                    DateTime? fechaCompletado = null;

                    if (estado == EstadoServicio.Asignado || estado == EstadoServicio.EnProceso ||
                        estado == EstadoServicio.Completado || estado == EstadoServicio.Cancelado)
                    {
                        fechaAsignacion = fechaSolicitud.AddHours(_random.Next(1, 24));
                    }

                    if (estado == EstadoServicio.Completado)
                    {
                        fechaCompletado = fechaAsignacion.Value.AddHours(_random.Next(1, 72));
                    }
                    else if (estado == EstadoServicio.Cancelado)
                    {
                        fechaCompletado = fechaAsignacion.Value.AddHours(_random.Next(1, 12));
                    }

                    servicios.Add(new Servicio
                    {
                        Tipo = tipo,
                        Estado = estado,
                        FechaSolicitud = fechaSolicitud,
                        FechaAsignacion = fechaAsignacion,
                        FechaCompletado = fechaCompletado,
                        Descripcion = GenerarDescripcionServicio(tipo, cliente.Nombre, i + 6),
                        DireccionOrigen = GenerarDireccionAleatoria(),
                        DireccionDestino = GenerarDireccionAleatoria(),
                        Costo = costo,
                        Prioridad = prioridad,
                        Observaciones = GenerarObservaciones(estado, tipo),
                        ClienteId = cliente.Id,
                        OperadorId = operador?.Id,
                        RutaId = ruta?.Id,
                        Activo = estado != EstadoServicio.Cancelado || _random.Next(10) > 2,
                        FechaRegistro = fechaSolicitud,
                        UsuarioRegistro = usuario,
                        UsuarioModificacion = fechaAsignacion.HasValue ? operador?.Nombre : null,
                        FechaModificacion = fechaAsignacion
                    });
                }

                context.Servicios.AddOrUpdate(s => s.Descripcion, servicios.ToArray());
                context.SaveChanges();
                Console.WriteLine($"+ {servicios.Count} servicios creados exitosamente.");
            }
            else
            {
                Console.WriteLine("+ Servicios ya existen en la base de datos.");
            }
        }

        private List<Servicio> CrearServiciosEjemplo(List<Cliente> clientes, List<Operador> operadores,
                                            List<Ruta> rutas, DateTime fechaActual, string usuario)
        {
            if (clientes.Count < 5 || operadores.Count < 5 || rutas.Count < 3)
            {
                Console.WriteLine("X No hay suficientes datos para crear servicios ejemplo");
                return new List<Servicio>();
            }

            return new List<Servicio>
    {
        new Servicio
        {
            Tipo = TipoServicio.Entrega,
            Estado = EstadoServicio.EnProceso,
            FechaSolicitud = fechaActual.AddHours(-2),
            FechaAsignacion = fechaActual.AddHours(-1),
            FechaCompletado = null,
            Descripcion = "Entrega URGENTE Servidores",
            DireccionOrigen = clientes[0].Direccion,
            DireccionDestino = "Data Center X",
            Costo = 25000.00m,
            Prioridad = 1,
            Observaciones = "Equipos 24/7",
            ClienteId = clientes[0].Id,
            OperadorId = operadores[0].Id,
            RutaId = rutas[0].Id,
            Activo = true,
            FechaRegistro = fechaActual.AddHours(-2),
            UsuarioRegistro = usuario
        },
        new Servicio
        {
            Tipo = TipoServicio.Instalacion,
            Estado = EstadoServicio.Asignado,
            FechaSolicitud = fechaActual.AddDays(-1),
            FechaAsignacion = fechaActual.AddHours(-12),
            FechaCompletado = null,
            Descripcion = "Instalacion AWS",
            DireccionOrigen = "Proveedor Industrial",
            DireccionDestino = clientes[1].Direccion,
            Costo = 18500.50m,
            Prioridad = 2,
            Observaciones = "Pre-instalacion pendiente",
            ClienteId = clientes[1].Id,
            OperadorId = operadores[1].Id,
            RutaId = rutas[1].Id,
            Activo = true,
            FechaRegistro = fechaActual.AddDays(-1),
            UsuarioRegistro = usuario
        },
        new Servicio
        {
            Tipo = TipoServicio.Entrega,
            Estado = EstadoServicio.Completado,
            FechaSolicitud = fechaActual.AddDays(-3),
            FechaAsignacion = fechaActual.AddDays(-3).AddHours(2),
            FechaCompletado = fechaActual.AddDays(-2),
            Descripcion = "Entrega de suministros",
            DireccionOrigen = "Distribuidora Central",
            DireccionDestino = clientes[2].Direccion,
            Costo = 8750.25m,
            Prioridad = 1,
            Observaciones = "Material medico",
            ClienteId = clientes[2].Id,
            OperadorId = operadores[2].Id,
            RutaId = rutas[1].Id,
            Activo = true,
            FechaRegistro = fechaActual.AddDays(-3),
            UsuarioRegistro = usuario
        },
        new Servicio
        {
            Tipo = TipoServicio.Recoleccion,
            Estado = EstadoServicio.Completado,
            FechaSolicitud = fechaActual.AddDays(-5),
            FechaAsignacion = fechaActual.AddDays(-5).AddHours(1),
            FechaCompletado = fechaActual.AddDays(-4),
            Descripcion = "Recoleccion de equipo",
            DireccionOrigen = clientes[3].Direccion,
            DireccionDestino = "Centro Tecnico",
            Costo = 3200.00m,
            Prioridad = 3,
            Observaciones = "Equipo embalado",
            ClienteId = clientes[3].Id,
            OperadorId = operadores[3].Id,
            RutaId = rutas[2].Id,
            Activo = true,
            FechaRegistro = fechaActual.AddDays(-5),
            UsuarioRegistro = usuario
        },
        new Servicio
        {
            Tipo = TipoServicio.Entrega,
            Estado = EstadoServicio.Cancelado,
            FechaSolicitud = fechaActual.AddDays(-2),
            FechaAsignacion = fechaActual.AddDays(-2).AddHours(3),
            FechaCompletado = fechaActual.AddDays(-1),
            Descripcion = "Entrega de suministros",
            DireccionOrigen = "Fabrica de Mobiliario",
            DireccionDestino = "Plaza Comercial",
            Costo = 15600.75m,
            Prioridad = 2,
            Observaciones = "Cliente cancelo",
            ClienteId = clientes[4].Id,
            OperadorId = null,
            RutaId = null,
            Activo = false,
            FechaRegistro = fechaActual.AddDays(-2),
            UsuarioRegistro = usuario
        }
    };
        }

        #endregion

        #region Helpers

        private int DeterminarPrioridad(string nombreCliente, TipoServicio tipo)
        {
            if (nombreCliente.Contains("Hospital") || nombreCliente.Contains("Médico"))
                return 1;

            if (tipo == TipoServicio.Mantenimiento && nombreCliente.Contains("Microsoft"))
                return 1;

            if (tipo == TipoServicio.Entrega && nombreCliente.Contains("Amazon"))
                return 2;

            return _random.Next(1, 4);
        }

        private decimal CalcularCosto(TipoServicio tipo, decimal distanciaKm, int prioridad)
        {
            decimal costoBase = _random.Next(500, 5000);
            decimal costoFinal = costoBase + (distanciaKm * 15);

            switch (prioridad)
            {
                case 1: costoFinal *= 1.5m; break;
                case 2: costoFinal *= 1.2m; break;
            }

            switch (tipo)
            {
                case TipoServicio.Instalacion:
                    costoFinal *= 1.3m;
                    break;
                case TipoServicio.Mantenimiento:
                    costoFinal *= 1.25m;
                    break;
            }

            return Math.Round(costoFinal, 2);
        }

        private string GenerarDescripcionServicio(TipoServicio tipo, string cliente, int numero)
        {
            string[] verbosEntrega = { "Entrega de", "Distribucion de", "Envio de", "Transporte de" };
            string[] verbosRecoleccion = { "Recoleccion de", "Retiro de", "Recogida de", "Acopio de" };
            string[] verbosInstalacion = { "Instalacion de", "Montaje de", "Configuracion de", "Puesta en marcha de" };
            string[] verbosMantenimiento = { "Mantenimiento de", "Revision de", "Reparacion de", "Calibracion de" };

            string[] materiales = { "equipo de computo", "documentacion legal", "suministros medicos",
                           "mobiliario de oficina", "equipo industrial", "material educativo" };

            string verbo;

            switch (tipo)
            {
                case TipoServicio.Entrega:
                    verbo = verbosEntrega[_random.Next(verbosEntrega.Length)];
                    break;
                case TipoServicio.Recoleccion:
                    verbo = verbosRecoleccion[_random.Next(verbosRecoleccion.Length)];
                    break;
                case TipoServicio.Instalacion:
                    verbo = verbosInstalacion[_random.Next(verbosInstalacion.Length)];
                    break;
                case TipoServicio.Mantenimiento:
                    verbo = verbosMantenimiento[_random.Next(verbosMantenimiento.Length)];
                    break;
                default:
                    verbo = "Servicio de";
                    break;
            }

            string material = materiales[_random.Next(materiales.Length)];
            string clienteNombre = cliente.Split(' ')[0];

            return $"{verbo} {material} para {clienteNombre} (Servicio #{numero})";
        }

        private string GenerarObservaciones(EstadoServicio estado, TipoServicio tipo)
        {
            switch (estado)
            {
                case EstadoServicio.Completado:
                    return $"Servicio {tipo.ToString().ToLower()} completado.";

                case EstadoServicio.Cancelado:
                    return $"Servicio cancelado.";

                case EstadoServicio.EnProceso:
                    return $"Operador en ruta. Tiempo estimado: {_random.Next(15, 60)} minutos.";

                case EstadoServicio.Asignado:
                    return "Asignado a operador. Preparando documentación.";

                default:
                    return "Servicio registrado. Pendiente de asignacion.";
            }
        }

        private string GenerarNombreAleatorio()
        {
            string[] nombres = { "Juan", "Maria", "Carlos", "Ana", "Jose", "Laura", "Miguel" };
            string[] apellidos = { "Garcia", "Rodriguez", "Martinez", "Hernández", "Lopez", "Gonzalez" };

            return $"{nombres[_random.Next(nombres.Length)]} {apellidos[_random.Next(apellidos.Length)]}";
        }

        private string GenerarDireccionAleatoria()
        {
            string[] calles = { "Av. Reforma", "Paseo Triunfo", "Eje Vial", "Panamericana" };
            string[] colonias = { "Centro", "Praderas", "Horizontes", "Tecnologico", "Campos", "Zaragoza" };

            return $"{calles[_random.Next(calles.Length)]} #{_random.Next(1, 1000)}, Col. {colonias[_random.Next(colonias.Length)]}, JRZ";
        }

        private string GenerarRFCAleatorio()
        {
            string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string rfc = "";

            for (int i = 0; i < 3; i++)
                rfc += letras[_random.Next(letras.Length)];

            rfc += $"{_random.Next(10):D2}{_random.Next(1, 13):D2}{_random.Next(1, 32):D2}";

            for (int i = 0; i < 3; i++)
                rfc += letras[_random.Next(letras.Length)];

            return rfc;
        }

        private void MostrarEstadisticas(SqlDbContext context)
        {
            try
            {
                Console.WriteLine("\n--- ESTADISTICAS DE LA BASE DE DATOS ---");

                var totalClientes = context.Clientes.Count(c => c.Activo);
                var totalOperadores = context.Operadores.Count(o => o.Activo);
                var totalRutas = context.Rutas.Count(r => r.Activo);
                var totalServicios = context.Servicios.Count(s => s.Activo);

                Console.WriteLine($"Clientes activos: {totalClientes}");
                Console.WriteLine($"Operadores activos: {totalOperadores}");
                Console.WriteLine($"Rutas activas: {totalRutas}");
                Console.WriteLine($"Servicios registrados: {totalServicios}");

                var serviciosPorEstado = context.Servicios
                    .Where(s => s.Activo)
                    .GroupBy(s => s.Estado)
                    .Select(g => new { Estado = g.Key, Count = g.Count() })
                    .ToList();

                Console.WriteLine("\nServicios por estado:");
                foreach (var item in serviciosPorEstado)
                {
                    Console.WriteLine($"  {item.Estado}: {item.Count}");
                }

                var ingresosTotales = context.Servicios
                    .Where(s => s.Estado == EstadoServicio.Completado && s.Activo)
                    .Sum(s => (decimal?)s.Costo) ?? 0;

                Console.WriteLine($"\nIngresos totales (servicios completados): ${ingresosTotales:N2}");

                var topClientes = context.Servicios
                    .Where(s => s.Activo)
                    .GroupBy(s => s.Cliente)
                    .Select(g => new {
                        Cliente = g.Key.Nombre,
                        Servicios = g.Count(),
                        Ingresos = g.Where(s => s.Estado == EstadoServicio.Completado).Sum(s => (decimal?)s.Costo) ?? 0
                    })
                    .OrderByDescending(c => c.Servicios)
                    .Take(5)
                    .ToList();

                Console.WriteLine("\nTop 5 clientes:");
                foreach (var cliente in topClientes)
                {
                    Console.WriteLine($"  {cliente.Cliente}: {cliente.Servicios} servicios (${cliente.Ingresos:N2})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error mostrando estadísticas: {ex.Message}");
            }
        }

        #endregion

    }
}
