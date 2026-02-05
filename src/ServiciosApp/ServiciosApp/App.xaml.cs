using System;
using System.Windows;

namespace ServiciosApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            try
            {
                // Cargar el proveedor de SQL Server usando reflection
                var sqlServerAsm = System.Reflection.Assembly.Load("EntityFramework.SqlServer");
                var sqlProviderType = sqlServerAsm.GetType("System.Data.Entity.SqlServer.SqlProviderServices");
                var instanceProp = sqlProviderType?.GetProperty("Instance", 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var instance = instanceProp?.GetValue(null);
                
                // Inicializar la base de datos
                Infrastructure.ServiciosApp.Data.DbInitializer.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de inicialización:\n{ex.Message}\n\n{ex.InnerException?.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown(1);
                return;
            }
            
            ServiceLocator.Instance.RegisterServices();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                ServiceLocator.Instance.Dispose();
            }
            catch { }
            
            base.OnExit(e);
        }
    }
}
