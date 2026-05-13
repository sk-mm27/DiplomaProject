using SUPPLIERS.Controller;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using FV = SUPPLIERS.Model.FieldsValidation;

namespace SUPPLIERS.Windows
{
    public partial class WinTag : Window
    {
        int ID = 0;

        public WinTag(int UserId)
        {
            InitializeComponent();
            ID = UserId;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (WinTag_Controller.Save(NAME.Text, COLOR.SelectedColorText, DESCRIPTION.Text, ID))
                Close();
        }

        private void ButtonClose(object sender, RoutedEventArgs e) => Close();
    }
}
