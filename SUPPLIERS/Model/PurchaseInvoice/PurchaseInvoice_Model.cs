using SUPPLIERS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using FV = SUPPLIERS.Model.FieldsValidation;

namespace SUPPLIERS.Model
{
    class PurchaseInvoice_Model
    {
        public PURCHASE_INVOICE GetPurchaseInvoice(int Id)
        {
            return new AMBIT().PURCHASE_INVOICE.Where(s => s.PURCHASE_INVOICE_ID == Id).FirstOrDefault();
        }

        public List<PURCHASE_INVOICE> GetAllPurchaseInvoice()
        {
            return new AMBIT().PURCHASE_INVOICE.ToList();
        }

        public bool CheckNumber(int Id, string Number)
        {
            return new AMBIT().PURCHASE_INVOICE.Any(pi => pi.NUMBER == Number && pi.PURCHASE_INVOICE_ID != Id);
        }

        private void NewPurchaseInvioce(string Number, int DeliveryContract, DateTime Date, int Quality,
            int Status, string Comment, List<int> Tag, int UserId)
        {

            AMBIT db = new AMBIT();

            PURCHASE_INVOICE pi = new PURCHASE_INVOICE() {
                NUMBER = Number,
                FK_DELIVERY_CONTRACT_ID = DeliveryContract,
                DATE = Date,
                FK_QUALITY_ID = Quality,
                FK_STATUS_ID = Status,
                COMMENT = Comment,
                FK_USER_ID_CREATOR = UserId,
                DATE_CREATION = DateTime.Now
            };                        

            db.PURCHASE_INVOICE.Add(pi);

            db.SaveChanges();

            int id = db.PURCHASE_INVOICE.OrderByDescending(s => s.PURCHASE_INVOICE_ID).First().PURCHASE_INVOICE_ID;

            new Tag_Model().SavePurchaseInvoiceTag(id, Tag);
        }

        private void ChangePurchaseInvoice(int Id, string Number, int DeliveryContract, DateTime Date, int Quality,
            int Status, string Comment, List<int> Tag, int UserId)
        {

            AMBIT db = new AMBIT();

            PURCHASE_INVOICE pi = db.PURCHASE_INVOICE.Where(p => p.PURCHASE_INVOICE_ID == Id).First();
            pi.NUMBER = Number;
            pi.FK_DELIVERY_CONTRACT_ID = DeliveryContract;
            pi.DATE = Date;
            pi.FK_QUALITY_ID = Quality;
            pi.FK_STATUS_ID = Status;
            pi.COMMENT = Comment;

            pi.FK_USER_ID_CHANGED = UserId;
            pi.DATE_CHANGE = DateTime.Now;

            db.SaveChanges();

            new Tag_Model().SavePurchaseInvoiceTag(Id, Tag);
        }

        public void DeletePurchaseInvoice(int Id)
        {
            AMBIT db = new AMBIT();

            db.TAG_PURCHASE_INVOICE.RemoveRange(db.TAG_PURCHASE_INVOICE.Where(pi => pi.FK_PURCHASE_INVOICE_ID == Id));
            db.PURCHASE_INVOICE.Remove(db.PURCHASE_INVOICE.First(pi => pi.PURCHASE_INVOICE_ID == Id));

            db.SaveChanges();
        }

        public bool Save(int Id, string Number, int DeliveryContractId, DateTime? Date, int QualityId,
            int StatusId, string Comment, object[] Tag, int UserId, Action<string> Error)
        {
            if (Check(Id, Number, DeliveryContractId, Date, QualityId, StatusId, Error))
            {
                PurchaseInvoice_Model pim = new PurchaseInvoice_Model();

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
                    if (pim.GetPurchaseInvoice(Id) != null)
                    {
                        pim.ChangePurchaseInvoice(Id, Number, DeliveryContractId, (DateTime)Date, QualityId, StatusId, Comment, tag, UserId);
                        return true;
                    }
                }
                else
                {
                    pim.NewPurchaseInvioce(Number, DeliveryContractId, (DateTime)Date, QualityId, StatusId, Comment, tag, UserId);
                    return true;
                }
            }
            return false;
        }


        private bool Check(int Id, string Number, int DeliveryContractId, DateTime? Date,
            int QualityId, int StatusId, Action<string> Error)
        {
            try
            {
                if (FV.EmptinessField(Number, "Номер пуст!", Error))
                {
                    return false;
                }
                if (new PurchaseInvoice_Model().CheckNumber(Id, Number))
                {
                    Error("Номер занят!");
                    return false;
                }
                if (DeliveryContractId <= 0)
                {
                    Error("Договор не указан!");
                    return false;
                }
                if (FV.SelectedDate(Date, "Дата не указана!", Error))
                {
                    return false;
                }
                if (Date < new DeliveryContract_Model().GetDeliveryContract(DeliveryContractId).DATE)
                {
                    Error("Дата оформления накладной раньше чем у договора поставки!");
                    return false;
                }
                if (QualityId <= 0)
                {
                    Error("Качество не указано!");
                    return false;
                }
                if (StatusId <= 0)
                {
                    Error("Статус не указан!");
                    return false;
                }
                return true;
            }
            catch
            {
                Error("Ошибка конвертации!");
                return false;
            }
        }

        public List<PURCHASE_INVOICE> SearchName(string Name) => 
            new AMBIT().PURCHASE_INVOICE.Where(s => s.DELIVERY_CONTRACT.SUPPLIER.NAME.Contains(Name)).ToList();

        public List<PURCHASE_INVOICE> SearchTag(int Id)
        {
            var tag = new AMBIT().TAG_PURCHASE_INVOICE.Where(t => t.FK_TAG_ID == Id).ToList();

            if (tag.Count > 0)
            {
                List<PURCHASE_INVOICE> su = new List<PURCHASE_INVOICE>();

                foreach (TAG_PURCHASE_INVOICE t in tag)
                {
                    su.Add(new AMBIT().PURCHASE_INVOICE.First(s => s.PURCHASE_INVOICE_ID == t.FK_PURCHASE_INVOICE_ID));
                }

                return su;
            }

            return null;
        }
        public int DeliveryOnTime(int Id)
        {
            return new AMBIT().PURCHASE_INVOICE.Where(
                pi => pi.DELIVERY_CONTRACT.FK_SUPPLIER_ID == Id &&
                (pi.FK_STATUS_ID == 2 || pi.FK_STATUS_ID == 3)).Count();
        }

        public int DeliveryNotOnTime(int Id)
        {
            return new AMBIT().PURCHASE_INVOICE.Where(
                pi => pi.DELIVERY_CONTRACT.FK_SUPPLIER_ID == Id &&
                pi.FK_STATUS_ID == 1).Count();
        }
    }
}
