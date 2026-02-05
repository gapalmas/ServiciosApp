using System.Windows;
using ServiciosApp.ViewModels;

namespace ServiciosApp.Views
{
    public partial class OperadorView : Window
    {
        public OperadorView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Instance.Resolve<OperadorViewModel>();
        }
    }
}
