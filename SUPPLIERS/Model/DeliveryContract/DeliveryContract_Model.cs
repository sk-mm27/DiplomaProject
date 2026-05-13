using System;
using System.Collections.Generic;
using System.Linq;
using FV = SUPPLIERS.Model.FieldsValidation;

namespace SUPPLIERS.Model
{
    class DeliveryContract_Model
    {
        public DELIVERY_CONTRACT GetDeliveryContract(int Id)
            => new AMBIT().DELIVERY_CONTRACT.Where(dc => dc.DELIVERY_CONTRACT_ID == Id).FirstOrDefault();
        public List<DELIVERY_CONTRACT> GetAllDeliveryContract()
            => new AMBIT().DELIVERY_CONTRACT.ToList();
        public bool CheckNumber(int Id, string Number)
            => new AMBIT().DELIVERY_CONTRACT.Any(pi => pi.NUMBER == Number && pi.DELIVERY_CONTRACT_ID != Id);

        private void NewDeliveryContract(string Number, int Supplier, DateTime Date, DateTime DateFinish, string Comment, 
            int UserId, List<int> Tag)
        {
            AMBIT db = new AMBIT();

            DELIVERY_CONTRACT dc = new DELIVERY_CONTRACT() { 
                NUMBER = Number,
                FK_SUPPLIER_ID = Supplier,
                DATE = Date,
                DATE_FINISH = DateFinish,
                COMMENT = Comment,
                FK_USER_ID_CREATOR = UserId,
                DATE_CREATION = DateTime.Now
            };


            db.DELIVERY_CONTRACT.Add(dc);

            db.SaveChanges();

            int id = db.DELIVERY_CONTRACT.OrderByDescending(s => s.DELIVERY_CONTRACT_ID).First().DELIVERY_CONTRACT_ID;


            new Tag_Model().SaveDeliveryContractTag(id, Tag);
        }

        private void ChangeDeliveryContract(int Id, string Number, int Supplier, DateTime Date, DateTime DateFinish, string Comment,
            int UserId, List<int> Tag)
        {
            AMBIT db = new AMBIT();

            DELIVERY_CONTRACT dc = db.DELIVERY_CONTRACT.Where(p => p.DELIVERY_CONTRACT_ID == Id).First();

            dc.NUMBER = Number;
            dc.FK_SUPPLIER_ID = Supplier;
            dc.DATE = Date;
            dc.DATE_FINISH = DateFinish;
            dc.COMMENT = Comment;

            dc.FK_USER_ID_CHANGED = UserId;
            dc.DATE_CHANGE = DateTime.Now;

            db.SaveChanges();

            new Tag_Model().SaveDeliveryContractTag(Id, Tag);
        }

        public void DeleteDeliveryContract(int Id)
        {
            AMBIT db = new AMBIT();

            db.TAG_PURCHASE_INVOICE.RemoveRange(db.TAG_PURCHASE_INVOICE.Where(dc => dc.PURCHASE_INVOICE.FK_DELIVERY_CONTRACT_ID == Id));
            db.PURCHASE_INVOICE.RemoveRange(db.PURCHASE_INVOICE.Where(dc => dc.FK_DELIVERY_CONTRACT_ID == Id));

            db.TAG_DELIVERY_CONTRACT.RemoveRange(db.TAG_DELIVERY_CONTRACT.Where(s => s.FK_DELIVERY_CONTRACT_ID == Id));
            db.DELIVERY_CONTRACT.Remove(db.DELIVERY_CONTRACT.First(dc => dc.DELIVERY_CONTRACT_ID == Id));

            db.SaveChanges();
        }


        public bool Save(int Id, string Number, object Supplier, DateTime? Date, DateTime? DateFinish, string Comment,
            int UserId, object[] Tag,
            Action<string> Error)
        {
            if (Check(Id, Number, Supplier, Date, DateFinish, Error))
            {
                DeliveryContract_Model dcm = new DeliveryContract_Model();

                int supplier;
                List<int> tag = new List<int>();
                try
                {
                    supplier = ((SUPPLIER)Supplier).SUPPLIER_ID;

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
                    if (dcm.GetDeliveryContract(Id) != null)
                    {
                        dcm.ChangeDeliveryContract(Id, Number, supplier, (DateTime)Date, (DateTime)DateFinish, Comment, UserId, tag);
                        return true;
                    }
                }
                else
                {
                    dcm.NewDeliveryContract(Number, supplier, (DateTime)Date, (DateTime)DateFinish, Comment, UserId, tag);
                    return true;
                }
            }
            return false;
        }

        private bool Check(int Id, string Number, object Supplier, DateTime? Date, DateTime? DateFinish,
            Action<string> Error)
        {
            if (FV.EmptinessField(Number, "Номер пуст!", Error))
            {
                return false;
            }
            if (new DeliveryContract_Model().CheckNumber(Id, Number))
            {
                Error("Номер занят!");
                return false;
            }
            if (FV.SelectedValue<SUPPLIER>(Supplier, "Поставщик не указан!", Error))
            {
                return false;
            }
            if (FV.SelectedDate(Date, "Дата не указана!", Error))
            {
                return false;
            }
            if (FV.SelectedDate(DateFinish, "Крайняя дата не указана!", Error))
            {
                return false;
            }
            else if (DateFinish < Date)
            {
                Error("Крайний срок поставки раньше чем дата оформления договора!");
                return false;
            }
            return true;
        }

        public List<DELIVERY_CONTRACT> SearchName(string Name) => new AMBIT().DELIVERY_CONTRACT.Where(s => s.SUPPLIER.NAME.Contains(Name)).ToList();

        public List<DELIVERY_CONTRACT> SearchTag(int Id)
        {
            var tag = new AMBIT().TAG_DELIVERY_CONTRACT.Where(t => t.FK_TAG_ID == Id).ToList();

            if (tag.Count > 0)
            {
                List<DELIVERY_CONTRACT> su = new List<DELIVERY_CONTRACT>();

                foreach (TAG_DELIVERY_CONTRACT t in tag)
                {
                    su.Add(new AMBIT().DELIVERY_CONTRACT.First(s => s.DELIVERY_CONTRACT_ID == t.FK_DELIVERY_CONTRACT_ID));
                }

                return su;
            }

            return null;
        }
    }
}
