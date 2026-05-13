using SUPPLIERS.Model;
using SUPPLIERS.Windows.PurchaseInvoice;
using SUPPLIERS.Windows;
using System;
using System.Collections.Generic;
using SUPPLIERS.Windows.Supplier;

namespace SUPPLIERS.Controller
{
    class WinSupplier_Controller
    {
        public static bool Save(int Id, string Name, int TimeZoneId, string Inn, 
            int FormOwnership, int TypeActivityId, int ProfileId, string FullAddress,
            string[] Phone, int?[] PhoneFormat, string[] Email, string[,] OpeningHours, object[] Tag,
            int UserId)
        {
            return new Supplier_Model().Save(Id, Name, TimeZoneId, Inn, FormOwnership, TypeActivityId, ProfileId, 
                FullAddress, Phone, PhoneFormat, Email, OpeningHours, Tag, UserId, FieldsValidation.ErrorMessage);
        }

        public static void OpenTypeActivity(Action Update)
        {
            new WinTypeActivity().ShowDialog();
            Update();
        }
        public static void OpenProfile(Action Update)
        {
            new WinProfile().ShowDialog();
            Update();
        }
        public static void OpenTag(int UserId, Action Update)
        {
            new WinTag(UserId).ShowDialog();
            Update();
        }

        public static SUPPLIER Supplier(int Id) 
            => new Supplier_Model().GetSupplier(Id);

        public static List<TYPE_ACTIVITY> AllTypeActivity() 
            => new TypeActivity_Model().GetAllTypeActivity();
        public static List<PROFILE> AllProfile() 
            => new Profile_Model().GetAllProfile();

        public static List<TIME_ZONE> AllTimeZone()
           => new TimeZone_Model().GetAllTimeZone();
        public static List<PHONE_FORMAT> AllPhoneFormat()
           => new PhoneFormat_Model().GetAllPhoneFormat();

        public static List<PHONE> SupplierPhone(int Id) 
            => new Supplier_Model().GetSupplierPhone(Id);
        public static List<EMAIL> SupplierEmail(int Id) 
            => new Supplier_Model().GetSupplierEmail(Id);

        public static List<OPENING_HOURS> OpeningHours(int Id) 
            => new OpeningHours_Model().GetOpeningHours(Id);
        
        public static List<TAG> AllTag() 
            => new Tag_Model().GetAllTag();
        public static List<TAG_SUPPLIER> SupplierTags(int Id) 
            => new Tag_Model().GetAllSupplierTag(Id);
        public static TAG Tag(int Id) 
            => new Tag_Model().GetTag(Id);

        public static string PhoneFormat(int Id)
            => new PhoneFormat_Model().GetPhoneFormat(Id).FORMAT;

    }
}
