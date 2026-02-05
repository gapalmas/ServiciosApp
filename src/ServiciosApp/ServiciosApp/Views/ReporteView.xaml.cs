using System.Windows;
using ServiciosApp.ViewModels;

namespace ServiciosApp.Views
{
    public partial class ReporteView : Window
    {
        public ReporteView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Instance.Resolve<ReporteViewModel>();
        }
    }
}
