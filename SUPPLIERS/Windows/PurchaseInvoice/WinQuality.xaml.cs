using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS.Windows.PurchaseInvoice
{
    public partial class WinQuality : Window
    {
        public WinQuality()
        {
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (WinQuality_Controller.Save(NAME.Text, DESCRIPTION.Text))
                Close();
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
            => Close();
    }
}
