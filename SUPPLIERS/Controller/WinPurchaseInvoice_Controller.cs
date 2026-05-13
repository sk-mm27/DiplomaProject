using SUPPLIERS.Model;
using SUPPLIERS.Windows;
using SUPPLIERS.Windows.PurchaseInvoice;
using System;
using System.Collections.Generic;

namespace SUPPLIERS.Controller
{
    class WinPurchaseInvoice_Controller
    {
        public static void OpenDeliveryContract(int UserId, Action Update)
        {
            new WinDeliveryContract(UserId).ShowDialog();
            Update();
        }        
        public static void OpenQuality(Action Update)
        {
            new WinQuality().ShowDialog();
            Update();
        }
        public static void OpenStatus(Action Update)
        {
            new WinStatus().ShowDialog();
            Update();
        }
        public static void OpenTag(int UserId, Action Update)
        {
            new WinTag(UserId).ShowDialog();
            Update();
        }


        public static bool Save(int Id, string Number, int DeliveryContractId, DateTime? Date, int QualityId,
            int StatusId, string Comment, object[] Tag, int UserId)
        {
            return new PurchaseInvoice_Model().Save(Id, Number, DeliveryContractId, Date, QualityId,
            StatusId, Comment, Tag, UserId, FieldsValidation.ErrorMessage);
        }

        public static PURCHASE_INVOICE PurchaseInvoce(int Id) => new PurchaseInvoice_Model().GetPurchaseInvoice(Id);

        public static List<DELIVERY_CONTRACT> AllDeliveryContract() => new DeliveryContract_Model().GetAllDeliveryContract();

        public static List<QUALITY> AllQuality() => new Quality_Model().GetAllQuality();
        public static List<STATUS> AllStatus() => new Status_Model().GetAllStatus();

        public static TAG Tag(int Id) => new Tag_Model().GetTag(Id);
        public static List<TAG> AllTag() => new Tag_Model().GetAllTag();
        public static List<TAG_PURCHASE_INVOICE> AllPurchaseInvoiceTag(int Id) => new Tag_Model().GetAllPurchaseInvoiceTag(Id);
        
    }
}
