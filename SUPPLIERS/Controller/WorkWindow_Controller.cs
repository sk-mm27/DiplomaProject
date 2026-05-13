using SUPPLIERS.Model;
using SUPPLIERS.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace SUPPLIERS.Controller
{
    class WorkWindow_Controller
    {
        #region Open

        private static bool CheckValue<T>(object Value)
            => Value != null && Value is T;


        public static void OpenRegistry(object Supplier)
        {
            if (CheckValue<SUPPLIER>(Supplier))
                new WinRegistry(((SUPPLIER)Supplier).INN).ShowDialog();
        }
        public static void OpenOpeningHours(object Supplier) 
        {
            if (CheckValue<SUPPLIER>(Supplier)) 
                new MessageWinOpeningHours(((SUPPLIER)Supplier).SUPPLIER_ID).ShowDialog(); 
        }
        public static void OpenMap(object Supplier)
        {
            if (CheckValue<SUPPLIER>(Supplier)) 
                new WinMap(((SUPPLIER)Supplier).ADDRESS).ShowDialog();
        }
        public static void OpenСontacts(object Supplier)
        {
            if (CheckValue<SUPPLIER>(Supplier)) 
                new MessageWinСontacts(((SUPPLIER)Supplier).SUPPLIER_ID).ShowDialog();
        }

        #region NewChangeDelete

        private static void NewChange(Func<bool?> Window, Action Update)
        {
            Window();
            Update();
        }        

        public static void OpenNewSupplier(int UserId, Action Update) 
            => NewChange(new WinSupplier(UserId).ShowDialog, Update);
        public static void OpenNewDeliveryContract(int UserId, Action Update) 
            => NewChange(new WinDeliveryContract(UserId).ShowDialog, Update);
        public static void OpenNewPurchaseInvoice(int UserId, Action Update) 
            => NewChange(new WinPurchaseInvoice(UserId).ShowDialog, Update);

        public static void OpenChangeSupplier(int UserId, object Supplier, Action Update) 
        {
            if (CheckValue<SUPPLIER>(Supplier))
                NewChange(new WinSupplier(UserId, ((SUPPLIER)Supplier).SUPPLIER_ID).ShowDialog, Update);
        }
        public static void OpenChangeDeliveryContract(int UserId, object DeliveryContract, Action Update) 
        {
            if (CheckValue<DELIVERY_CONTRACT>(DeliveryContract))
                NewChange(new WinDeliveryContract(UserId, ((DELIVERY_CONTRACT)DeliveryContract).DELIVERY_CONTRACT_ID).ShowDialog, Update);
        }
        public static void OpenChangePurchaseInvoice(int UserId, object PurchaseInvoice, Action Update) 
        {
            if (CheckValue<PURCHASE_INVOICE>(PurchaseInvoice))
                NewChange(new WinPurchaseInvoice(UserId, ((PURCHASE_INVOICE)PurchaseInvoice).PURCHASE_INVOICE_ID).ShowDialog, Update);
        }

        private static bool ConfirmationDelete() => MessageBox.Show("Вы уверены?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

        public static void DeleteSupplier(object Supplier, Action Update)
        {
            if (ConfirmationDelete() && CheckValue<SUPPLIER>(Supplier))
            {
                new Supplier_Model().DeleteSupplier(((SUPPLIER)Supplier).SUPPLIER_ID);
                Update();
            }            
        }
        public static void DeleteDeliveryContract(object DeliveryContract, Action Update)
        {
            if (ConfirmationDelete() && CheckValue<DELIVERY_CONTRACT>(DeliveryContract))
            {
                new DeliveryContract_Model().DeleteDeliveryContract(((DELIVERY_CONTRACT)DeliveryContract).DELIVERY_CONTRACT_ID);
                Update();
            }
        }
        public static void DeletePurchaseInvoice(object PurchaseInvoice, Action Update)
        {
            if (ConfirmationDelete() && CheckValue<PURCHASE_INVOICE>(PurchaseInvoice))
            {
                new PurchaseInvoice_Model().DeletePurchaseInvoice(((PURCHASE_INVOICE)PurchaseInvoice).PURCHASE_INVOICE_ID);
                Update();
            }
        }

        #endregion NewChangeDelete

        #endregion Open


        #region Search

        private static List<T> CheckSearchName<T>(string Name, Func<string, List<T>> Search, Func<List<T>> All)
            => !string.IsNullOrEmpty(Name) ? Search(Name) : All();

        public static List<SUPPLIER> SearchNameSupplier(string Name)
            => CheckSearchName(Name, new Supplier_Model().SearchName, AllSupplier);
        public static List<DELIVERY_CONTRACT> SearchNameDeliveryContract(string Name)
            => CheckSearchName(Name, new DeliveryContract_Model().SearchName, AllDeliveryContract);
        
        public static List<PURCHASE_INVOICE> SearchNamePurchaseInvoice(string Name)
            => CheckSearchName(Name, new PurchaseInvoice_Model().SearchName, AllPurchaseInvoice);
        

        private static List<T> CheckSearchTag<T>(object Tag, Func<int, List<T>> Search, Func<List<T>> All)
            => Tag != null && Tag is TAG tag ? Search(tag.TAG_ID) : All();

        public static List<SUPPLIER> SearchTagSupplier(object Tag)
            => CheckSearchTag(Tag, new Supplier_Model().SearchTag, AllSupplier);
        public static List<DELIVERY_CONTRACT> SearchTagDeliveryContract(object Tag)
            => CheckSearchTag(Tag, new DeliveryContract_Model().SearchTag, AllDeliveryContract);
        public static List<PURCHASE_INVOICE> SearchTagPurchaseInvoice(object Tag)
            => CheckSearchTag(Tag, new PurchaseInvoice_Model().SearchTag, AllPurchaseInvoice);

        #endregion Search

        public static List<SUPPLIER> AllSupplier() 
            => new Supplier_Model().GetAllSupplier();
        public static List<DELIVERY_CONTRACT> AllDeliveryContract() 
            => new DeliveryContract_Model().GetAllDeliveryContract();
        public static List<PURCHASE_INVOICE> AllPurchaseInvoice() 
            => new PurchaseInvoice_Model().GetAllPurchaseInvoice();

        public static List<USER> AllUser() => new User_Model().GetAllUser();

        public static void LastActivity(int Id) => new User_Model().SetLastActivity(Id);

        public static string GetNowTime(object Supplier)
        {
            SUPPLIER s = (SUPPLIER)Supplier;

            string t = DateTime.Now.AddHours(s.TIME_ZONE.VALUE).TimeOfDay.ToString();

            return "Время: " + t.Substring(0, t.LastIndexOf(":"));
        }

        public static List<Rating> Rating() 
            => new WorkWindow_Model().RatingSupliers();

        public static Uri Registry(string Inn)
            => new Uri(new WorkWindow_Model().GetRegistryOnZakupki(Inn));
        public static Uri Map(string Address)
           => new Uri(new WorkWindow_Model().GetAddressOnMap(Address));

        public static List<PHONE> SupplierPhone(int Id) 
            => new Supplier_Model().GetSupplierPhone(Id);        
        public static List<EMAIL> SupplierEmail(int Id) 
            => new Supplier_Model().GetSupplierEmail(Id);

        public static string[] SupplierOpeningHours(int Id)
        {
            List<OPENING_HOURS> openingHours = new OpeningHours_Model().GetOpeningHours(Id);

            string[] result = new string[7];

            for (int i = 0; i < 7; i++)
            {
                var t = openingHours.Where(h => h.DAY == i).FirstOrDefault();
                result[i] = t == null ? "Выходной" :
                    t.START.ToString().Substring(0, t.START.ToString().Length - 3) + "-" +
                    t.END.ToString().Substring(0, t.START.ToString().Length - 3);
            }

            return result;
        }

        public static void ActivateDeactivateUser(object User, bool Status, Action Update)
        {
            if (new User_Model().ActivateDeactivate(User, Status))
                Update();
        }

    }
}
