using SUPPLIERS.Controller;
using System.Windows;
using System.Windows.Controls;
namespace SUPPLIERS
{
    public partial class MessageWinOpeningHours : Window
    {
        public MessageWinOpeningHours(int Id)
        {
            InitializeComponent();

            string[] openingHours = WorkWindow_Controller.SupplierOpeningHours(Id);

            for (int i = 0; i < 7; i++) 
                (FindName("Time_" + i) as TextBox).Text = openingHours[i];
        }
    }
}
