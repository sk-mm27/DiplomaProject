using SUPPLIERS.Model;

namespace SUPPLIERS.Controller
{
    class WinProfile_Controller
    {
        public static bool Save(string Name)
            => new Profile_Model().Save(Name, FieldsValidation.ErrorMessage);
    }
}
