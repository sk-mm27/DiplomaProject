using SUPPLIERS.Model;

namespace SUPPLIERS.Controller
{
    class WinStatus_Controller
    {
        public static bool Save(string Name, string Description)
            => new Status_Model().Save(Name, Description, FieldsValidation.ErrorMessage);
    }
}
