using System.Windows;
using ServiciosApp.ViewModels;

namespace ServiciosApp.Views
{
    public partial class ServiciosDetailView : Window
    {
        public ServiciosDetailView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Instance.Resolve<ServicioViewModel>();
        }
    }
}
