using System.Linq;
using System.Windows;
using GC = SUPPLIERS.Controller.GeneralController;
using WPIC = SUPPLIERS.Controller.WinPurchaseInvoice_Controller;

namespace SUPPLIERS.Windows
{
    public partial class WinPurchaseInvoice : Window
    {
        private readonly int ID, USERID;

        public WinPurchaseInvoice(int UserId, int Id = 0)
        {
            InitializeComponent();

            UpdateComboBox();

            if (Id > 0) 
            { 
                ID = Id; 
                Filling(); 
            }

            USERID = UserId;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (WPIC.Save(ID, NUMBER.Text, (int)DELIVERY_CONTRACT.SelectedValue, DATE.SelectedDate, (int)QUALITY.SelectedValue,
                (int)STATUS.SelectedValue, COMMENT.Text, DataGrid_Tag.Items.OfType<object>().ToArray(),
                USERID))
            {               
                Close();
            }
        }        

        private void NewDeliveryContract(object sender, RoutedEventArgs e) =>
            WPIC.OpenDeliveryContract(ID, UpdateComboBox);
        private void NewQuality(object sender, RoutedEventArgs e) =>
            WPIC.OpenStatus(UpdateComboBox);
        private void NewStatus(object sender, RoutedEventArgs e) =>
            WPIC.OpenStatus(UpdateComboBox);
        private void NewTag(object sender, RoutedEventArgs e) =>
            WPIC.OpenTag(ID, UpdateComboBox);

        private void UpdateComboBox()
        {
            int[,] index = new int[,] 
            {
                { DELIVERY_CONTRACT.SelectedIndex, DELIVERY_CONTRACT.Items.Count },
                { QUALITY.SelectedIndex, QUALITY.Items.Count },
                { STATUS.SelectedIndex, STATUS.Items.Count },
                { TAG.SelectedIndex, TAG.Items.Count }
            };

            DELIVERY_CONTRACT.ItemsSource = WPIC.AllDeliveryContract();
            QUALITY.ItemsSource = WPIC.AllQuality();
            STATUS.ItemsSource = WPIC.AllStatus();
            TAG.ItemsSource = WPIC.AllTag();

            DELIVERY_CONTRACT.SelectedIndex = GC.NewIndexComboBox(DELIVERY_CONTRACT.Items.Count, index[0, 1], index[0, 0]);
            QUALITY.SelectedIndex = GC.NewIndexComboBox(QUALITY.Items.Count, index[1, 1], index[1, 0]);
            STATUS.SelectedIndex = GC.NewIndexComboBox(STATUS.Items.Count, index[2, 1], index[2, 0]);
            TAG.SelectedIndex = GC.NewIndexComboBox(TAG.Items.Count, index[3, 1], index[3, 0]);
        }        
        
        private void Filling()
        {
            PURCHASE_INVOICE pi = WPIC.PurchaseInvoce(ID);

            NUMBER.Text = pi.NUMBER;
            DELIVERY_CONTRACT.SelectedValue = pi.FK_DELIVERY_CONTRACT_ID;
            DATE.SelectedDate = pi.DATE;
            QUALITY.SelectedValue = pi.FK_QUALITY_ID;
            STATUS.SelectedValue = pi.FK_STATUS_ID;
            COMMENT.Text = pi.COMMENT;

            var tags = WPIC.AllPurchaseInvoiceTag(ID);

            if (tags == null) 
            {
                return; 
            }
            foreach (var t in tags) 
            { 
                DataGrid_Tag.Items.Add(t.TAG); 
            }
        }

        #region Tag

        private void AddTag(object sender, RoutedEventArgs e)
        {
            if (TAG.SelectedIndex > -1)
                DataGrid_Tag.Items.Add(WPIC.Tag(int.Parse(TAG.SelectedValue.ToString())));
        }

        private void DelTag(object sender, RoutedEventArgs e)
        {
            if (DataGrid_Tag.SelectedIndex > -1)
                DataGrid_Tag.Items.Remove(DataGrid_Tag.SelectedItem);
        }
        #endregion Tag

        private void ButtonClose(object sender, RoutedEventArgs e) => Close();
    }
}
