using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Infrastructure.ServiciosApp.Migrations;

namespace Infrastructure.ServiciosApp.Data
{
    public class DbInitializer
    {
        public static void Initialize()
        {
            // Establecer la estrategia de inicialización automática
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SqlDbContext, Configuration>());

            using (var context = new SqlDbContext())
            {
                // Si la base de datos no existe, crearla desde el modelo
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                }

                // Intentar inicializar (aplicar migraciones y ejecutar Seed)
                try
                {
                    context.Database.Initialize(true);
                }
                catch (Exception ex)
                {
                    // Si falla debido a que faltan tablas (p. ej. Invalid object name),
                    // generamos y ejecutamos el script DDL basado en el modelo para crear el esquema
                    try
                    {
                        var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                        var createScript = objectContext.CreateDatabaseScript();
                        if (!string.IsNullOrWhiteSpace(createScript))
                        {
                            context.Database.ExecuteSqlCommand(createScript);
                        }

                        // Reintentar la inicialización para ejecutar Seed
                        context.Database.Initialize(true);
                    }
                    catch (Exception inner)
                    {
                        // Si sigue fallando, propagar la excepción original para diagnóstico
                        throw new InvalidOperationException("Error al inicializar la base de datos y crear el esquema.", inner ?? ex);
                    }
                }
            }
        }
    }
}
