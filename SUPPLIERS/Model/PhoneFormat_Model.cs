using System.Collections.Generic;
using System.Linq;

namespace SUPPLIERS.Model
{
    class PhoneFormat_Model
    {
        public List<PHONE_FORMAT> GetAllPhoneFormat()
            => new AMBIT().PHONE_FORMAT.ToList();
        public PHONE_FORMAT GetPhoneFormat(int Id)
            => new AMBIT().PHONE_FORMAT.Where(s => s.PHONE_FORMAT_ID == Id).FirstOrDefault();
    }
}
