using System;
using System.Collections.Generic;
using System.Linq;
using FV = SUPPLIERS.Model.FieldsValidation;

namespace SUPPLIERS.Model
{
    class Tag_Model
    {
        public List<TAG> GetAllTag() 
            => new AMBIT().TAG.ToList();

        public TAG GetTag(int Id) 
            => new AMBIT().TAG.Where(ts => ts.TAG_ID == Id).FirstOrDefault();

        public List<TAG_SUPPLIER> GetAllSupplierTag(int Id)
            => new AMBIT().TAG_SUPPLIER.Where(ts => ts.FK_SUPPLIER_ID == Id).ToList();

        public List<TAG_PURCHASE_INVOICE> GetAllPurchaseInvoiceTag(int Id) 
            => new AMBIT().TAG_PURCHASE_INVOICE.Where(ts => ts.FK_PURCHASE_INVOICE_ID == Id).ToList();

        public List<TAG_DELIVERY_CONTRACT> GetAllDeliveryContractTag(int Id)
            => new AMBIT().TAG_DELIVERY_CONTRACT.Where(ts => ts.FK_DELIVERY_CONTRACT_ID == Id).ToList();


        private void NewTag(string Name, string Color, string Description, int UserId)
        {
            AMBIT db = new AMBIT();
            TAG t = new TAG()
            {
                NAME = Name,
                COLOR = Color,
                DESCRIPTION = Description,
                FK_USER_ID_CREATOR = UserId,
                DATE_CREATION = DateTime.Now,
            };

            db.TAG.Add(t);

            db.SaveChanges();
        }

        public bool Save(string Name, string Color, string Description, int UserId, Action<string> Error)
        {
            if (Check(Name, Color, Error))
            {
                NewTag(Name, Color, Description, UserId);
                return true;
            }
            return false;
        }

        private bool Check(string Name, string Color, Action<string> Error)
        {
            if (FV.EmptinessField(Name, "Название пусто!", Error)) 
            { 
                return false; 
            }
            if (string.IsNullOrEmpty(Color))
            {
                Error("Цвет пуст!"); 
                return false; 
            }
            return true;
        }

        public void SavePurchaseInvoiceTag(int Id, List<int> Tag)
        {
            AMBIT db = new AMBIT();

            if (Id > 0)
            {
                db.TAG_PURCHASE_INVOICE.RemoveRange(db.TAG_PURCHASE_INVOICE.Where(tdc => tdc.FK_PURCHASE_INVOICE_ID == Id));
            }

            if (Tag.Count != 0)
            {
                foreach (int tag in Tag)
                {
                    db.TAG_PURCHASE_INVOICE.Add(new TAG_PURCHASE_INVOICE()
                    {
                        FK_PURCHASE_INVOICE_ID = Id,
                        FK_TAG_ID = tag
                    });
                }
            }

            db.SaveChanges();
        }

        public void SaveSupplierTag(int Id, List<int> Tag)
        {
            AMBIT db = new AMBIT();

            if (Id > 0)
            {
                db.TAG_SUPPLIER.RemoveRange(db.TAG_SUPPLIER.Where(tdc => tdc.FK_SUPPLIER_ID == Id));
            }

            if (Tag.Count != 0)
            {
                foreach (int tag in Tag)
                {
                    db.TAG_SUPPLIER.Add(new TAG_SUPPLIER()
                    {
                        FK_SUPPLIER_ID = Id,
                        FK_TAG_ID = tag
                    });

                }
            }

            db.SaveChanges();
        }

        public void SaveDeliveryContractTag(int Id, List<int> Tag)
        {
            AMBIT db = new AMBIT();

            if (Id > 0)
            {
                db.TAG_DELIVERY_CONTRACT.RemoveRange(db.TAG_DELIVERY_CONTRACT.Where(tdc => tdc.FK_DELIVERY_CONTRACT_ID == Id));
            }

            if (Tag.Count != 0)
            {
                foreach (int tag in Tag)
                {
                    db.TAG_DELIVERY_CONTRACT.Add(new TAG_DELIVERY_CONTRACT()
                    {
                        FK_DELIVERY_CONTRACT_ID = Id,
                        FK_TAG_ID = tag
                    });

                }
            }

            db.SaveChanges();
        }

    }
}
