using SUPPLIERS.Controller;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SUPPLIERS.Windows
{
    public partial class MessageWinСontacts : Window
    {
        public MessageWinСontacts(int Id)
        {
            InitializeComponent();

            var phons = WorkWindow_Controller.SupplierPhone(Id);
            var emails = WorkWindow_Controller.SupplierEmail(Id);

            for (int i = 0; i < 3; i++)
            {
                var ph = phons.Where(p => p.COMMENT == (i == 0 ? "о" : i == 1 ? "м" : "д")).FirstOrDefault();
                var em = emails.Where(e => e.COMMENT == (i == 0 ? "о" : i == 1 ? "м" : "д")).FirstOrDefault();

                (FindName("Phone_" + i) as TextBox).Text = ph == null ? "" : ph.NUMBER;
                (FindName("Email_" + i) as TextBox).Text = em == null ? "" : em.EMAIL1;
            }
        }
    }
}
