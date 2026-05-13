using System.Collections.Generic;
using System.Linq;

namespace SUPPLIERS.Model
{
    class TimeZone_Model
    {
        public List<TIME_ZONE> GetAllTimeZone()
            => new AMBIT().TIME_ZONE.ToList();
    }
}
