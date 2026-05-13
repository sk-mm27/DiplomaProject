using SUPPLIERS.Model;
using SUPPLIERS.Windows;
using System;

namespace SUPPLIERS.Controller
{
    class MainWindow_Controller
    {
        public static void Login(string Login, string Password, Action Hide)
        {
            var authorization = User_Model.Authorization(Login, Password);

            if (authorization.Item1)
            {
                Hide();
                new WorkWindow((bool)authorization.Item3, (int)authorization.Item2).Show();
            }
            else
            {
                FieldsValidation.ErrorMessage("Неправильный логин или пароль!");
            }
        }

        public static void Registration() => new WinRegistration().ShowDialog();
    }
}
