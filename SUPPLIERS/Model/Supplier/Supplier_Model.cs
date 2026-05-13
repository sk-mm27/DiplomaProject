using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using FV = SUPPLIERS.Model.FieldsValidation;

namespace SUPPLIERS.Model
{
    class Supplier_Model
    {
        #region Get

        public List<SUPPLIER> GetAllSupplier() 
            => new AMBIT().SUPPLIER.ToList();
        public SUPPLIER GetSupplier(int Id) 
            => new AMBIT().SUPPLIER.Where(s => s.SUPPLIER_ID == Id).FirstOrDefault();        

        public List<PHONE> GetSupplierPhone(int Id) 
            => new AMBIT().PHONE.Where(p => p.FK_SUPPLIER_ID == Id).ToList();
        public List<EMAIL> GetSupplierEmail(int Id) 
            => new AMBIT().EMAIL.Where(e => e.FK_SUPPLIER_ID == Id).ToList();


        #endregion Get

        #region CreateChangeDelete

        private void NewSupplier(string Name, int TimeZone, string INN, int FormOwnershit,
            int TypeActivity, int Profile, string FullAddress, int UserId, string[] Phone, int?[] PhoneFormat, string[] Email,
            string[,] OpeningHours, List<int> Tag)
        {
            AMBIT db = new AMBIT();

            SUPPLIER su = new SUPPLIER() {
                NAME = Name,
                FK_TIME_ZONE_ID = TimeZone,
                INN = INN,
                FORM_OWNERSHIP = FormOwnershit == 0,
                FK_TYPE_ACTIVITY_ID = TypeActivity,
                FK_PROFILE_ID = Profile,
                ADDRESS = FullAddress,
                FK_USER_ID_CREATOR = UserId,
                DATE_CHANGE = DateTime.Now
            };          

            db.SUPPLIER.Add(su);

            db.SaveChanges();

            int id = db.SUPPLIER.OrderByDescending(s => s.SUPPLIER_ID).First().SUPPLIER_ID;
            
            NewContactDetails(id, Phone, Email, PhoneFormat);
            new OpeningHours_Model().NewOpeningHours(id, OpeningHours);
            new Tag_Model().SaveSupplierTag(id, Tag);
        }

        private void NewContactDetails(int ID, string[] Phone, string[] Email, int?[] PhoneFormat)
        {
            AMBIT db = new AMBIT();

            string c = "омд";
            for (int i = 0; i < 3; i++)
            {
                string com = c[i].ToString();

                if (!string.IsNullOrEmpty(Phone[i].Trim('_')))
                {
                    PHONE ph = new PHONE()
                    {
                        FK_SUPPLIER_ID = ID,
                        FK_PHONE_FORMAT_ID = (int)PhoneFormat[i],
                        NUMBER = Phone[i],
                        COMMENT = com
                    };

                    db.PHONE.Add(ph);

                    db.SaveChanges();
                }

                if (!string.IsNullOrEmpty(Email[i]))
                {
                    EMAIL em = new EMAIL()
                    {
                        FK_SUPPLIER_ID = ID,
                        EMAIL1 = Email[i],
                        COMMENT = com
                    };

                    db.EMAIL.Add(em);

                    db.SaveChanges();
                }
            }
        }

        private void ChangeSupplier(int Id, string Name, int TimeZone, string INN, int FormOwnershit,
            int TypeActivity, int Profile, string FullAddress, int UserId, string[] Phone, int?[] PhoneFormat, string[] Email,
            string[,] OpeningHours, List<int> Tag)
        {
            AMBIT db = new AMBIT();

            SUPPLIER su = db.SUPPLIER.First(p => p.SUPPLIER_ID == Id);

            su.NAME = Name;
            su.FK_TIME_ZONE_ID = TimeZone;
            su.INN = INN;
            su.FORM_OWNERSHIP = FormOwnershit == 0;
            su.FK_TYPE_ACTIVITY_ID = TypeActivity;
            su.FK_PROFILE_ID = Profile;
            su.ADDRESS = FullAddress;
            su.FK_USER_ID_CHANGED = UserId;
            su.DATE_CHANGE = DateTime.Now;

            db.SaveChanges();

            ChangeContactDetails(Id, Phone, Email, PhoneFormat);
            new OpeningHours_Model().ChangeOpeningHours(Id, OpeningHours);
            new Tag_Model().SaveSupplierTag(Id, Tag);
        }

        private void ChangeContactDetails(int Id, string[] Phone, string[] Email, int?[] PhoneFormat)
        {
            AMBIT db = new AMBIT();

            db.PHONE.RemoveRange(db.PHONE.Where(tdc => tdc.FK_SUPPLIER_ID == Id));
            db.EMAIL.RemoveRange(db.EMAIL.Where(tdc => tdc.FK_SUPPLIER_ID == Id));

            string c = "омд";

            for (int i = 0; i < 3; i++)
            {
                string com = c[i].ToString();

                if (!string.IsNullOrEmpty(Phone[i].Trim('_')))
                {
                    PHONE ph = new PHONE() 
                    {
                        FK_SUPPLIER_ID = Id,
                        FK_PHONE_FORMAT_ID = (int)PhoneFormat[i],
                        NUMBER = Phone[i],
                        COMMENT = com
                    };

                    db.PHONE.Add(ph);

                    db.SaveChanges();
                }

                if (!string.IsNullOrEmpty(Email[i]))
                {
                    EMAIL em = new EMAIL() 
                    {
                        FK_SUPPLIER_ID = Id,
                        EMAIL1 = Email[i],
                        COMMENT = com
                    };                    

                    db.EMAIL.Add(em);

                    db.SaveChanges();
                }
            }
        }

        public bool Save(
            int Id, string Name, int TimeZoneId, string Inn, int FormOwnership, int TypeActivityId, int ProfileId, string FullAddress,
            string[] Phone, int?[] PhoneFormat, string[] Email,
            string[,] OpeningHours,
            object[] Tag,
            int UserId,
            Action<string> Error)
        {
            if (Check(Id, Name, (int)TimeZoneId, Inn, FormOwnership, (int)TypeActivityId, (int)ProfileId, FullAddress,
                Phone, Email, OpeningHours,
                Error))
            {
                Supplier_Model sm = new Supplier_Model();

                List<int> tag = new List<int>();
                try
                {                   
                    if (Tag.Length > 0)
                    {
                        foreach (int t in Tag.Cast<TAG>().ToArray().Select(t => t.TAG_ID))
                        {
                            tag.Add(t);
                        }
                    }
                }
                catch
                {
                    Error("Ошибка конвертации!");
                    return false;
                }

                if (Id > 0)
                {
                    if (sm.GetSupplier(Id) != null)
                    {
                        sm.ChangeSupplier(Id, Name, TimeZoneId, Inn, FormOwnership, TypeActivityId, ProfileId, FullAddress, UserId,
                         Phone, PhoneFormat, Email, OpeningHours, tag);
                        return true;
                    }
                }
                else
                {
                    sm.NewSupplier(Name, TimeZoneId, Inn, FormOwnership, TypeActivityId, ProfileId, FullAddress, UserId,
                         Phone, PhoneFormat, Email, OpeningHours, tag);
                    return true;
                }
            }
            return false;
        }


        public void DeleteSupplier(int Id)
        {
            AMBIT db = new AMBIT();

            db.TAG_SUPPLIER.RemoveRange(db.TAG_SUPPLIER.Where(s => s.FK_SUPPLIER_ID == Id));
            db.EMAIL.RemoveRange(db.EMAIL.Where(s => s.FK_SUPPLIER_ID == Id));
            db.PHONE.RemoveRange(db.PHONE.Where(s => s.FK_SUPPLIER_ID == Id));
            db.OPENING_HOURS.RemoveRange(db.OPENING_HOURS.Where(s => s.FK_SUPPLIER_ID == Id));

            db.TAG_PURCHASE_INVOICE.RemoveRange(db.TAG_PURCHASE_INVOICE.Where(s => s.PURCHASE_INVOICE.DELIVERY_CONTRACT.FK_SUPPLIER_ID == Id));
            db.PURCHASE_INVOICE.RemoveRange(db.PURCHASE_INVOICE.Where(s => s.DELIVERY_CONTRACT.FK_SUPPLIER_ID == Id));

            db.TAG_DELIVERY_CONTRACT.RemoveRange(db.TAG_DELIVERY_CONTRACT.Where(s => s.DELIVERY_CONTRACT.FK_SUPPLIER_ID == Id));
            db.DELIVERY_CONTRACT.RemoveRange(db.DELIVERY_CONTRACT.Where(s => s.FK_SUPPLIER_ID == Id));

            db.SUPPLIER.Remove(db.SUPPLIER.First(s => s.SUPPLIER_ID == Id));

            db.SaveChanges();
        }

        #endregion CreateChangeDelete
                
        #region Check
        public bool Check(
            int Id, string Name, int TimeZoneId, string Inn, int FormOwnership, int TypeActivityId, int ProfileId, string FullAddress,
            string[] Phone, string[] Email,
            string[,] OpeningHours,
            Action<string> Error)
        {
            if (FV.EmptinessField(Name, "Название пусто!", Error))
                return false;

            if (TimeZoneId <= 0)
            {
                Error("Часовой пояс не указан!");
                return false;
            }

            if (Inn.Trim('_').Length != 10 && FormOwnership == 0 ||
                Inn.Trim('_').Length != 12 && FormOwnership == 1)
            {
                Error("ИНН не заполнен!");
                return false;
            }
            else if (CheckInn(Inn, Id)) 
            {
                Error("ИНН занят!");
                return false;
            }                

            if (FormOwnership != 0 & FormOwnership != 1)
            {
                Error("Вид собственности не указан!");
                return false;
            }

            if (TypeActivityId <= 0)
            {
                Error("Вид деятельности не указан!");
                return false;
            }

            if (ProfileId <= 0)
            {
                Error("Профиль не указан!");
                return false;
            }

            if (string.IsNullOrEmpty(FullAddress.Trim('^')))
            {
                Error("Адрес пуст!");
                return false;
            }
            else if (NotFullAddress(FullAddress))
            {
                if (FV.ConfirmationRequest("Адрес не полный!"))
                    return false;
            }


            //Phone and Email
            if (CheckPhone(Phone[0], "Телефон организации", Error, true))
                return false;
            if (CheckEmail(Email[0], "Email организации", Error, true))
                return false;

            if (CheckPhone(Phone[1], "Телефон менеджера", Error))
                return false;
            if (CheckEmail(Email[1], "Email менеджера", Error))
                return false;

            if (CheckPhone(Phone[2], "Дополнительный телефон", Error))
                return false;       
            if (CheckEmail(Email[2], "Дополнительный Email", Error))
                return false;


            for (int i = 0; i < 7; i++)
            {
                string day = i == 6 ? CultureInfo.CurrentCulture.DateTimeFormat.DayNames[0] :
                    CultureInfo.CurrentCulture.DateTimeFormat.DayNames[i + 1];

                if (CheckHours(OpeningHours[i, 0], OpeningHours[i, 1], day, Error))
                    return false;
            }
            return true;
        }

        private bool CheckPhone(string Phone, string Message, Action<string> Error, bool Required = false)
        {
            if (string.IsNullOrEmpty(Phone.Trim('_')))
            {
                if (Required)
                {
                    Error(Message + " не заполнен!");
                    return true;
                }
                else if (FV.ConfirmationRequest(Message + " не заполнен!"))
                {
                    return true;
                }
            }
            else if (Phone.Length > Phone.Trim('_').Length)
            {
                Error(Message + " заполненн не правильно!");
                return true;
            }
            return false;
        }

        private bool CheckEmail(string Email, string Message, Action<string> Error, bool Required = false)
        {
            if (string.IsNullOrEmpty(Email.Trim(' ')))
            {
                if (Required)
                {
                    Error(Message + " не заполнен!");
                    return true;
                }
                else if(FV.ConfirmationRequest(Message + " не заполнен!"))
                {
                    return true;
                }
            }
            else if (Regex.IsMatch(Email, @"([а-я])+") || Regex.IsMatch(Email, @"([А-Я])+"))
            {
                Error(Message + "\n\nEmail не должен содержать в себе кириллицу!");
                return true;
            }
            else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Error("\n\nEmail введён неправильно!");
                return true;
            }
            return false;
        }

        private bool CheckHours(string Hours1, string Hours2, string Message, Action<string> Error)
        {
            if (string.IsNullOrEmpty(Hours1) && string.IsNullOrEmpty(Hours2))
            {
                if(!FV.ConfirmationRequest(Message + "Рабочее время не заполнено!"))
                {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(Hours1) || string.IsNullOrEmpty(Hours2))
            {
                Error(Message + "\n\nРабочее время неполное!");
                return true;
            }
            if (GetTimeSpan(DateTime.Parse(Hours1)) >= GetTimeSpan(DateTime.Parse(Hours2)))
            {
                Error(Message + "\n\nНачало рабочего дня позже его конца!");
                return true;
            }

            return false;
        }

        private bool CheckInn(string Inn, int Id)
            => new AMBIT().SUPPLIER.Any(pi => pi.INN == Inn & pi.SUPPLIER_ID != Id);

        private TimeSpan GetTimeSpan(DateTime dt) => TimeSpan.Parse("" + dt.Hour + ":" + dt.Minute + ":00");

        private bool NotFullAddress(string FullAddress)
        {
            foreach (string fa in FullAddress.Split('^'))
            {
                if (string.IsNullOrEmpty(fa.Trim(' ')))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion Check

        #region Search

        public List<SUPPLIER> SearchName(string Name) => new AMBIT().SUPPLIER.Where(s => s.NAME.Contains(Name)).ToList();

        public List<SUPPLIER> SearchTag(int Id)
        {
            var tag = new AMBIT().TAG_SUPPLIER.Where(t => t.FK_TAG_ID == Id).ToList();

            if (tag.Count > 0)
            {
                List<SUPPLIER> su = new List<SUPPLIER>();

                foreach (TAG_SUPPLIER t in tag)
                {
                    su.Add(new AMBIT().SUPPLIER.First(s => s.SUPPLIER_ID == t.FK_SUPPLIER_ID));
                }

                return su;
            }

            return null;
        }

        #endregion Search
    }
}
