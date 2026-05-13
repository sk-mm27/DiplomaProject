using System;
using System.Collections.Generic;
using System.Linq;

namespace SUPPLIERS.Model
{
    class Status_Model
    {
        public List<STATUS> GetAllStatus()
        {
            return new AMBIT().STATUS.ToList();
        }

        public bool Save(string Name, string Description, Action<string> Error)
        {
            if (Check(Name, Error))
            {
                NewStatus(Name, Description);
                return true;
            }
            return false;
        }

        private void NewStatus(string Name, string Description)
        {
            AMBIT db = new AMBIT();

            db.STATUS.Add(new STATUS() { NAME = Name, DESCRIPTION = Description });

            db.SaveChanges();
        }

        private bool Check(string Name, Action<string> Error)
            => !FieldsValidation.EmptinessField(Name, "Название пусто!", Error);
    }
}
