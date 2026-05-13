using SUPPLIERS.Model;
using SUPPLIERS.Windows;
using System;
using System.Collections.Generic;

namespace SUPPLIERS.Controller
{
    class WinDeliveryContract_Controller
    {
        public static bool Save(int Id, string Number, object Supplier, DateTime? Date, DateTime? DateFinish, string Comment,
            int UserId, object[] Tag)
        {
            return new DeliveryContract_Model().Save(Id, Number, Supplier, Date, DateFinish, Comment, UserId, Tag, FieldsValidation.ErrorMessage);
        }

        public static void OpenSupplier(int UserId, Action Update)
        {
            new WinSupplier(UserId).ShowDialog();
            Update();
        }
        public static void OpenTag(int UserId, Action Update)
        {
            new WinTag(UserId).ShowDialog();
            Update();
        }

        public static DELIVERY_CONTRACT DeliveryContract(int Id) 
            => new DeliveryContract_Model().GetDeliveryContract(Id);

        public static List<SUPPLIER> AllSupplier() 
            => new Supplier_Model().GetAllSupplier();
        public static List<TAG> AllTag() 
            => new Tag_Model().GetAllTag();

        public static List<TAG_DELIVERY_CONTRACT> DeliveryContractTags(int Id) 
            => new Tag_Model().GetAllDeliveryContractTag(Id);
        public static TAG Tag(int Id) 
            => new Tag_Model().GetTag(Id);
        
    }
}
