using SUPPLIERS.Model;

namespace SUPPLIERS.Controller
{
    class WinTypeActivity_Controller
    {
        public static bool Save(string Name)
            => new TypeActivity_Model().Save(Name, FieldsValidation.ErrorMessage);
    }
}
