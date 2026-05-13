using System;
using System.Windows;

namespace SUPPLIERS.Model
{
    public class FieldsValidation
    {
        public static void ErrorMessage(string Message) => MessageBox.Show(Message);

        public static bool EmptinessField(string Text, string Message, Action<string> Error)
        {
            if (string.IsNullOrEmpty(Text.Trim(' ')))
            {
                Error(Message); 
                return true; 
            }
            return false;
        }
        public static bool SelectedValue<T>(object Value, string Message, Action<string> Error)
        {
            if (Value == null)
            {
                Error(Message);
                return true; 
            }
            else if (!(Value is T))
            {
                Error("Переданы данные неверного типа!");
                return true;
            }
            return false;
        }
        public static bool SelectedDate(DateTime? Date, string Message, Action<string> Error)
        {
            if (Date == null)
            {
                Error(Message);
                return true; 
            }
            return false;
        }

        public static bool ConfirmationRequest(string Message)
        {
            if (MessageBox.Show(Message + "\nПродолжить сохранение?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) 
            { 
                return true; 
            }
            return false;
        }
    }
}
