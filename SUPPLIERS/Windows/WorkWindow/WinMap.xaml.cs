using SUPPLIERS.Controller;
using System.Windows;

namespace SUPPLIERS.Windows
{
    public partial class WinMap : Window
    {
        public WinMap(string Address)
        {
            InitializeComponent();
            Map.Source = WorkWindow_Controller.Map(Address);
        }
    }
}
