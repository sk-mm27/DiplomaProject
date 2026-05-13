using System;
using System.Collections.Generic;
using System.Linq;

namespace SUPPLIERS.Model
{
    class Profile_Model
    {
        public PROFILE GetProfile(int Id)
        {
            return new AMBIT().PROFILE.Where(p => p.PROFILE_ID == Id).FirstOrDefault();
        }

        public List<PROFILE> GetAllProfile()
        {
            return new AMBIT().PROFILE.ToList();
        }

        public bool Save(string Name, Action<string> Error)
        {
            if (Check(Name, Error))
            {
                NewProfile(Name);
                return true;
            }
            return false;
        }

        private void NewProfile(string Name)
        {
            AMBIT db = new AMBIT();

            db.PROFILE.Add(new PROFILE() { NAME = Name });

            db.SaveChanges();
        }

        private bool Check(string Name, Action<string> Error) 
            => !FieldsValidation.EmptinessField(Name, "Название пусто!", Error);
    }
}
