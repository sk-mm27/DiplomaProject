using System.Linq;
using System.Windows;
using GC = SUPPLIERS.Controller.GeneralController;
using WDCC = SUPPLIERS.Controller.WinDeliveryContract_Controller;

namespace SUPPLIERS.Windows
{
    partial class WinDeliveryContract : Window
    {
        private readonly int ID, USERID;

        public WinDeliveryContract(int UserId, int Id = 0)
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
            if (WDCC.Save(ID, NUMBER.Text, SUPPLIER.SelectedValue, DATE.SelectedDate, DATE_FINISH.SelectedDate, COMMENT.Text,
                USERID, DataGrid_Tag.Items.OfType<object>().ToArray()))
            {
                Close();
            }
        }
        private void NewSupplier(object sender, RoutedEventArgs e) =>
            WDCC.OpenSupplier(ID, UpdateComboBox);
        private void NewTag(object sender, RoutedEventArgs e) =>
            WDCC.OpenTag(ID, UpdateComboBox);

        private void Filling()
        {
            DELIVERY_CONTRACT pi = WDCC.DeliveryContract(ID);

            NUMBER.Text = pi.NUMBER;
            SUPPLIER.SelectedValue = pi.FK_SUPPLIER_ID;
            DATE.SelectedDate = pi.DATE;
            DATE_FINISH.SelectedDate = pi.DATE_FINISH;
            COMMENT.Text = pi.COMMENT;

            var tags = WDCC.DeliveryContractTags(ID);

            if (tags == null)
            {
                return;
            }
            foreach (var t in tags)
            {
                DataGrid_Tag.Items.Add(t.TAG);
            }
        }

        private void UpdateComboBox()
        {
            int[,] index = new int[,] 
            { 
                { SUPPLIER.SelectedIndex, SUPPLIER.Items.Count },
                { TAG.SelectedIndex, TAG.Items.Count } 
            };

            SUPPLIER.ItemsSource = WDCC.AllSupplier();
            TAG.ItemsSource = WDCC.AllTag();

            SUPPLIER.SelectedIndex = GC.NewIndexComboBox(SUPPLIER.Items.Count, index[0,1], index[0,0]);
            TAG.SelectedIndex = GC.NewIndexComboBox(TAG.Items.Count, index[0, 1], index[0, 0]);
        }        

        #region Tag

        private void AddTag(object sender, RoutedEventArgs e)
        {
            if (TAG.SelectedIndex > -1)
                DataGrid_Tag.Items.Add(WDCC.Tag(int.Parse(TAG.SelectedValue.ToString())));
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
