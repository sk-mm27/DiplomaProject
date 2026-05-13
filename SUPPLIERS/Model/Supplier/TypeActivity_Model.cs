using System;
using System.Collections.Generic;
using System.Linq;

namespace SUPPLIERS.Model
{
    class TypeActivity_Model
    {
        public TYPE_ACTIVITY GetTypeActivity(int Id)
        {
            return new AMBIT().TYPE_ACTIVITY.Where(ta => ta.TYPE_ACTIVITY_ID == Id).FirstOrDefault();
        }

        public List<TYPE_ACTIVITY> GetAllTypeActivity()
        {
            return new AMBIT().TYPE_ACTIVITY.ToList();
        }

        public bool Save(string Name, Action<string> Error)
        {
            if (Check(Name, Error))
            {
                NewTypeActivity(Name);
                return true;
            }
            return false;
        }

        private void NewTypeActivity(string Name)
        {
            AMBIT db = new AMBIT();

            db.TYPE_ACTIVITY.Add(new TYPE_ACTIVITY() { NAME = Name });

            db.SaveChanges();
        }

        private bool Check(string Name, Action<string> Error)
            => !FieldsValidation.EmptinessField(Name, "Название пусто!", Error);
    }
}
