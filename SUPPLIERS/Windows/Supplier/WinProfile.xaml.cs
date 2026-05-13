using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS
{
    public partial class WinProfile : Window
    {
        public WinProfile()
        {
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (WinProfile_Controller.Save(NAME.Text))
                Close();
        }

        private void ButtonClose(object sender, RoutedEventArgs e) 
            => Close();
    }
}
