using SUPPLIERS.Model;

namespace SUPPLIERS.Controller
{
    class WinQuality_Controller
    {
        public static bool Save(string Name, string Description)
            => new Quality_Model().Save(Name, Description, FieldsValidation.ErrorMessage);
    }
}
