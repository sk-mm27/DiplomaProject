using SUPPLIERS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUPPLIERS.Controller
{
    class WinTag_Controller
    {
        public static bool Save(string Name, string Color, string Description, int UserId)
            => new Tag_Model().Save(Name, Color, Description, UserId, FieldsValidation.ErrorMessage);
    }
}
