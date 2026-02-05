using System.Windows;
using ServiciosApp.ViewModels;

namespace ServiciosApp.Views
{
    public partial class AsignacionView : Window
    {
        public AsignacionView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Instance.Resolve<AsignacionViewModel>();
        }
    }
}
