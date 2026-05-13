using SUPPLIERS.Model;

namespace SUPPLIERS.Controller
{
    class WinRegistration_Controller
    {
        public static bool Registration(string Fio, string Login, string Password)
            => new User_Model().Save(Fio, Login, Password, FieldsValidation.ErrorMessage);
    }
}
