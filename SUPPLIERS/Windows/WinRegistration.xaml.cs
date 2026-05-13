using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS.Windows
{
    public partial class WinRegistration : Window
    {
        public WinRegistration()
        {
            InitializeComponent();
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            if (WinRegistration_Controller.Registration(FIO.Text, LOGIN.Text, PASSWORD.Password))
            {               
                Close();
            }
        }

        private void ButtonClose(object sender, RoutedEventArgs e) => Close();
    }
}
