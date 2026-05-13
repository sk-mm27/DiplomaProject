using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS
{
    public partial class WinStatus : Window
    {
        public WinStatus()
        {
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (WinStatus_Controller.Save(NAME.Text, DESCRIPTION.Text))
                Close();
        }      

        private void ButtonClose(object sender, RoutedEventArgs e) 
            => Close();
    }
}
