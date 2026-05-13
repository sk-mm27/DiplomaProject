using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS.Windows
{
    public partial class WinRegistry : Window
    {
        public WinRegistry(string Inn)
        {
            InitializeComponent();
            Registry.Source = WorkWindow_Controller.Registry(Inn);
        }
    }
}
