using System.Windows;
using System.Windows.Controls;
using ServiciosApp.Views;

namespace ServiciosApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnServicios_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ServiciosDetailView();
            ventana.Show();
        }

        private void BtnOperadores_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new OperadorView();
            ventana.Show();
        }

        private void BtnAsignaciones_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new AsignacionView();
            ventana.Show();
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ReporteView();
            ventana.Show();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button button)
            {
                button.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(66, 66, 66));
            }
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button button)
            {
                button.Background = System.Windows.Media.Brushes.Transparent;
            }
        }
    }
}
