using SUPPLIERS.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SUPPLIERS.Model
{
    class User_Model
    {
        public List<USER> GetAllUser() 
            => new AMBIT().USER.Where(u => u.USER_ID != 1).ToList();

        public void SetLastActivity(int Id)
        {
            AMBIT db = new AMBIT();            

            db.USER.First(us => us.USER_ID == Id).LAST_ACTIVITY = DateTime.Now;

            db.SaveChanges();
        }

        public bool ActivateDeactivate(object User, bool Status)
        {
            if (FieldsValidation.SelectedValue<USER>(User, "", null))
                return false;

            if (Status)
                ActivateUser(((USER)User).USER_ID);
            else
                DeactivateUser(((USER)User).USER_ID);

            return true;
        }

        private void ActivateUser(int Id)
        {
            AMBIT db = new AMBIT();

            db.USER.First(u => u.USER_ID == Id).STATUS = true;

            db.SaveChanges();
        }

        private void DeactivateUser(int Id)
        {
            AMBIT db = new AMBIT();

            db.USER.First(u => u.USER_ID == Id).STATUS = false;

            db.SaveChanges();
        }

        public bool Save(string Fio, string Login, string Password, Action<string> Error)
        {
            if (Check(Fio, Login, Password, Error))
            {
                NewUser(Fio, Login, Password);
                return true;
            }
            return false;
        }

        private void NewUser(string Fio, string Login, string Password)
        {
            AMBIT db = new AMBIT();

            USER us = new USER() 
            {
                FIO = Fio,
                LOGIN = Login,
                PASSWORD = Password,
                FK_ROLE_ID = 2,
                STATUS = false
            };

            db.USER.Add(us);

            db.SaveChanges();
        }

        private bool Check(string Fio, string Login, string Password, Action<string> Error)
        {
            if (string.IsNullOrEmpty(Fio))
            {
                Error("ФИО пусто!"); 
                return false; 
            }
            else if (!Regex.IsMatch(Fio, @"^\S+ \S+$") && !Regex.IsMatch(Fio, @"^\S+ \S+ \S+$"))
            {
                Error("ФИО введено неправильно!"); 
                return false; 
            }

            if (string.IsNullOrEmpty(Login))
            { 
                Error("Логин пуст!"); 
                return false; 
            }
            else if (Login.Contains(" ") ||
                Regex.IsMatch(Login, @"([а-я])+") || Regex.IsMatch(Login, @"([А-Я])+"))
            { 
                Error("Логин введён неправильно!"); 
                return false; 
            }
            else if (new AMBIT().USER.Any(l => l.LOGIN == Login))
            { 
                Error("Логин уже занят!"); 
                return false; 
            }

            if (string.IsNullOrEmpty(Password))
            { 
                Error("Пароль пуст!"); 
                return false; 
            }
            else if (Password.Contains(" ") ||
                Regex.IsMatch(Password, @"([а-я])+") || Regex.IsMatch(Password, @"([А-Я])+"))
            { 
                Error("Пароль введён неправильно!"); 
                return false; 
            }

            return true;
        }

        public static (bool, int?, bool?) Authorization(string login, string Password)
        {
            AMBIT db = new AMBIT();

            var user = db.USER.FirstOrDefault(u => u.LOGIN == login && u.PASSWORD == Password && u.STATUS);

            if (user == null) 
            { 
                return (false, null, null); 
            }

            user.LAST_ACTIVITY = DateTime.Now;

            db.SaveChanges();

            return (true, user.USER_ID, user.FK_ROLE_ID != 1);
        }
    }
}
