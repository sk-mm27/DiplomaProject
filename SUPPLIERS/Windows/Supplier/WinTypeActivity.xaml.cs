using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS.Windows.Supplier
{
    public partial class WinTypeActivity : Window
    {
        public WinTypeActivity()
        {
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (WinTypeActivity_Controller.Save(NAME.Text))
                Close();
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
            => Close();
    }
}
