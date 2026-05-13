using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e) 
            => MainWindow_Controller.Login(LOGIN.Text, PASSWORD.Password, Hide);

        private void Registration(object sender, RoutedEventArgs e) 
            => MainWindow_Controller.Registration();
    }
}
