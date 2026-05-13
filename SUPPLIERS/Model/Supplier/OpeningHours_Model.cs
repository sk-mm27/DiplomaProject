using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUPPLIERS.Model
{
    class OpeningHours_Model
    {
        public List<OPENING_HOURS> GetOpeningHours(int Id)
        {
            return new AMBIT().OPENING_HOURS.Where(p => p.FK_SUPPLIER_ID == Id).ToList();
        }

        public void NewOpeningHours(int ID, string[,] OpeningHours)
        {
            AMBIT db = new AMBIT();

            TimeSpan GetTimeSpan(DateTime dt) => TimeSpan.Parse("" + dt.Hour + ":" + dt.Minute + ":00");

            for (int i = 0; i < 7; i++)
            {
                if (!(string.IsNullOrEmpty(OpeningHours[i, 0]) && string.IsNullOrEmpty(OpeningHours[i, 1])))
                {
                    OPENING_HOURS oh = new OPENING_HOURS()
                    {
                        FK_SUPPLIER_ID = ID,
                        DAY = i,
                        START = GetTimeSpan(DateTime.Parse(OpeningHours[i, 0])),
                        END = GetTimeSpan(DateTime.Parse(OpeningHours[i, 1]))
                    };

                    db.OPENING_HOURS.Add(oh);

                    db.SaveChanges();
                }
            }
        }

        public void ChangeOpeningHours(int ID, string[,] OpeningHours)
        {
            AMBIT db = new AMBIT();

            TimeSpan GetTimeSpan(DateTime dt) => TimeSpan.Parse("" + dt.Hour + ":" + dt.Minute + ":00");

            for (int i = 0; i < 7; i++)
            {
                if (!(string.IsNullOrEmpty(OpeningHours[i, 0]) && string.IsNullOrEmpty(OpeningHours[i, 1])))
                {
                    OPENING_HOURS oh = db.OPENING_HOURS.Where(h => h.FK_SUPPLIER_ID == ID && h.DAY == i).First();

                    oh.FK_SUPPLIER_ID = ID;
                    oh.DAY = i;
                    oh.START = GetTimeSpan(DateTime.Parse(OpeningHours[i, 0]));
                    oh.END = GetTimeSpan(DateTime.Parse(OpeningHours[i, 1]));

                    db.SaveChanges();
                }
            }
        }
    }
}
