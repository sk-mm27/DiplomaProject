using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GC = SUPPLIERS.Controller.GeneralController;
using WWC = SUPPLIERS.Controller.WorkWindow_Controller;

namespace SUPPLIERS.Windows
{
    public partial class WorkWindow : Window
    {
        private readonly int USERID; 

        public WorkWindow(bool Role, int UserId)
        {
            InitializeComponent();

            UpdateDataGrid();
            Rating();            

            if (Role) 
            { 
                this.Role(); 
            }

            USERID = UserId;            
        }

        private void Role()
        {
            Del_Supplier.Visibility = Visibility.Hidden;
            Del_DeliveryContract.Visibility = Visibility.Hidden;
            Del_PurchaseInvoice.Visibility = Visibility.Hidden;
            New_Supplier.Visibility = Visibility.Hidden;
            New_DeliveryContract.Visibility = Visibility.Hidden;
            New_PurchaseInvoice.Visibility = Visibility.Hidden;
            Users.Visibility = Visibility.Hidden;

            DataGrid_Supplier.MouseDoubleClick -= ChangeSupplier;
            DataGrid_DeliveryContract.MouseDoubleClick -= ChangeDeliveryContract;
            DataGrid_PurchaseInvoice.MouseDoubleClick -= ChangePurchaseInvoice;
        }

        #region OpenSupplierData

        private void OpenNowTime(object sender, RoutedEventArgs e) =>
            MessageBox.Show(WWC.GetNowTime(DataGrid_Supplier.SelectedItem));

        private void OpenRegistry(object sender, RoutedEventArgs e) => 
            WWC.OpenRegistry(DataGrid_Supplier.SelectedItem);
        private void OpenOpeningHours(object sender, RoutedEventArgs e) =>
            WWC.OpenOpeningHours(DataGrid_Supplier.SelectedItem);        
        private void OpenMap(object sender, RoutedEventArgs e) =>
            WWC.OpenMap(DataGrid_Supplier.SelectedItem);
        private void OpenСontacts(object sender, RoutedEventArgs e) =>
            WWC.OpenСontacts(DataGrid_Supplier.SelectedItem);

        #endregion OpenSupplierData

        #region NewChangeDelete

        private void NewSupplier(object sender, RoutedEventArgs e) =>
            WWC.OpenNewSupplier(USERID, UpdateDataGrid);
        private void NewDeliveryContract(object sender, RoutedEventArgs e) =>
            WWC.OpenNewDeliveryContract(USERID, UpdateDataGrid);
        private void NewPurchaseInvoice(object sender, RoutedEventArgs e) =>
            WWC.OpenNewPurchaseInvoice(USERID, UpdateDataGrid);

        private void ChangeSupplier(object sender, RoutedEventArgs e) =>
            WWC.OpenChangeSupplier(USERID, DataGrid_Supplier.SelectedItem, UpdateDataGrid);
        private void ChangeDeliveryContract(object sender, RoutedEventArgs e) =>
            WWC.OpenChangeDeliveryContract(USERID, DataGrid_DeliveryContract.SelectedItem, UpdateDataGrid);
        private void ChangePurchaseInvoice(object sender, RoutedEventArgs e) =>
            WWC.OpenChangePurchaseInvoice(USERID, DataGrid_PurchaseInvoice.SelectedItem, UpdateDataGrid);

        private void DeleteSupplier(object sender, RoutedEventArgs e) =>
            WWC.DeleteSupplier(DataGrid_Supplier.SelectedItem, UpdateDataGrid);
        private void DeleteDeliveryContract(object sender, RoutedEventArgs e) =>
            WWC.DeleteDeliveryContract(DataGrid_DeliveryContract.SelectedItem, UpdateDataGrid);
        private void DeletePurchaseInvoice(object sender, RoutedEventArgs e) =>
            WWC.DeletePurchaseInvoice(DataGrid_PurchaseInvoice.SelectedItem, UpdateDataGrid);

        #endregion NewChangeDelete

        #region Search
        
        private void SearcNamehSupplier(object sender, TextChangedEventArgs e)
            => DataGrid_Supplier.ItemsSource = WWC.SearchNameSupplier((sender as TextBox).Text);
        private void SearchNameDeliveryContract(object sender, TextChangedEventArgs e)
            => DataGrid_DeliveryContract.ItemsSource = WWC.SearchNameDeliveryContract((sender as TextBox).Text);
        private void SearchNamePurchaseInvoice(object sender, TextChangedEventArgs e)
            => DataGrid_PurchaseInvoice.ItemsSource = WWC.SearchNamePurchaseInvoice((sender as TextBox).Text);
        
        private void SearcTaghSupplier(object sender, SelectionChangedEventArgs e) 
            => DataGrid_Supplier.ItemsSource = WWC.SearchTagSupplier((sender as ComboBox).SelectedItem);
        private void SearchTagDeliveryContract(object sender, SelectionChangedEventArgs e) 
            => DataGrid_DeliveryContract.ItemsSource = WWC.SearchTagDeliveryContract((sender as ComboBox).SelectedItem);
        private void SearchTagPurchaseInvoice(object sender, SelectionChangedEventArgs e) 
            => DataGrid_PurchaseInvoice.ItemsSource = WWC.SearchTagPurchaseInvoice((sender as ComboBox).SelectedItem);


        private void Clear_SearchTag(object sender, RoutedEventArgs e) =>
            (FindName(GC.GetName((sender as Button).Name)) as ComboBox).SelectedIndex = -1;
        private void Toggle_SearchTag(object sender, MouseButtonEventArgs e) =>
            (FindName("Search_Tag_" + (sender as Path).Name) as DockPanel).Visibility = 
                (FindName("Search_Tag_" + (sender as Path).Name) as DockPanel).Visibility == Visibility.Visible ?
                Visibility.Hidden : Visibility.Visible;
        
        #endregion Search

        private void Rating() => DataGrid_RSupplier.ItemsSource = WWC.Rating();

        private void NotAct(object sender, RoutedEventArgs e) =>
            WWC.ActivateDeactivateUser(DataGrid_User.SelectedItem, false, UpdateDataGrid);
        private void Act(object sender, RoutedEventArgs e) =>
            WWC.ActivateDeactivateUser(DataGrid_User.SelectedItem, true, UpdateDataGrid);


        private void UpdateDataGrid()
        {
            DataGrid_PurchaseInvoice.ItemsSource = WWC.AllPurchaseInvoice();
            DataGrid_DeliveryContract.ItemsSource = WWC.AllDeliveryContract();
            DataGrid_Supplier.ItemsSource = WWC.AllSupplier();
            Rating();
            DataGrid_User.ItemsSource = WWC.AllUser();
        }

        
        private void Window_Closed(object sender, EventArgs e)
        {
            WWC.LastActivity(USERID);
            Application.Current.Shutdown();
        }
    }
}
