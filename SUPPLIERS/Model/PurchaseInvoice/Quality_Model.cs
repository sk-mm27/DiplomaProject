using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUPPLIERS.Model
{
    class Quality_Model
    {
        public List<QUALITY> GetAllQuality()
        {
            return new AMBIT().QUALITY.ToList();
        }

        public bool Save(string Name, string Description, Action<string> Error)
        {
            if (Check(Name, Error))
            {
                NewQuality(Name, Description);
                return true;
            }
            return false;
        }

        private void NewQuality(string Name, string Description)
        {
            AMBIT db = new AMBIT();

            db.QUALITY.Add(new QUALITY() { NAME = Name, DESCRIPTION = Description });

            db.SaveChanges();
        }

        private bool Check(string Name, Action<string> Error)
            => !FieldsValidation.EmptinessField(Name, "Название пусто!", Error);
    }
}
