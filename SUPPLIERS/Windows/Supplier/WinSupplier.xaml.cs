using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using GC = SUPPLIERS.Controller.GeneralController;
using WSC= SUPPLIERS.Controller.WinSupplier_Controller;

namespace SUPPLIERS.Windows
{
    public partial class WinSupplier : Window
    {
        private readonly int ID, USERID;

        public WinSupplier(int UserId, int Id = 0)
        {
            InitializeComponent();

            UpdateComboBox();

            if (Id > 0) 
            { 
                ID = Id; 
                Filling(); 
            }

            USERID = UserId;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (
                WSC.Save(
                ID, NAME.Text, (int)TIME_ZONE.SelectedValue, INN.Text, FORM_OWNERSHIP.SelectedIndex, (int)TYPE_ACTIVITY.SelectedValue, (int)PROFILE.SelectedValue, GetFullAddress(),
                new string[] { GetText(Phone_Organization), GetText(Phone_Manager), GetText(Phone_Additional) },
                new int?[] { (int?)PhoneFormat_Organization.SelectedValue, (int?)PhoneFormat_Manager.SelectedValue, (int?)PhoneFormat_Additional.SelectedValue },
                new string[] { Email_Organization.Text, Email_Manager.Text, Email_Additional.Text },
                GetOpeningHours(),
                DataGrid_Tag.Items.OfType<object>().ToArray(),
                USERID)
                )
            {
                Close();
            }
        }

        private void NewTypeActivity(object sender, RoutedEventArgs e) =>
            WSC.OpenTypeActivity(UpdateComboBox);
        private void NewProfile(object sender, RoutedEventArgs e) =>
            WSC.OpenProfile(UpdateComboBox);
        private void NewTag(object sender, RoutedEventArgs e) =>
            WSC.OpenTag(ID, UpdateComboBox);

        private void ButtonClose(object sender, RoutedEventArgs e) => Close();

        #region Filling
        private void Filling()
        {
            FillingSupplier();
            FillingContactDetails();
            FillingOpeningHours();

            var tags = WSC.SupplierTags(ID);

            if (tags.Count == 0) 
            { 
                return; 
            }

            foreach (var t in tags) 
            { 
                DataGrid_Tag.Items.Add(t.TAG);
            }
        }
        private void FillingSupplier()
        {
            SUPPLIER su = WSC.Supplier(ID);

            NAME.Text = su.NAME;
            TIME_ZONE.SelectedValue = su.FK_TIME_ZONE_ID;
            INN.Text = su.INN;
            FORM_OWNERSHIP.SelectedIndex = su.FORM_OWNERSHIP ? 0 : 1;
            TYPE_ACTIVITY.SelectedValue = su.FK_TYPE_ACTIVITY_ID;
            PROFILE.SelectedValue = su.FK_PROFILE_ID;

            string[] a = su.ADDRESS.Split('^');

            for (int i = 0; i < 4; i++)
            { 
                (FindName("ADDRESS_" + i) as TextBox).Text = a[i]; 
            }
        }
        private void FillingContactDetails()
        {
            var ph = WSC.SupplierPhone(ID);
            var em = WSC.SupplierEmail(ID);

            if (ph.Count == 0 && em.Count == 0) 
            { 
                return; 
            }
            
            string c = "омд";
            string[] pm = new string[] { "Organization", "Manager", "Additional" };
            
            for (int i = 0; i < 3; i++)
            {
                if (ph.Count != 0)
                {
                    var p = ph.FirstOrDefault(pho => pho.COMMENT == c[i].ToString());
                    if (p != null)
                    {
                        List<PHONE_FORMAT> pf = (FindName("PhoneFormat_" + pm[i]) as ComboBox).Items.Cast<PHONE_FORMAT>().ToList();
                        (FindName("PhoneFormat_" + pm[i]) as ComboBox).SelectedIndex = pf.IndexOf(pf.Single(f => f.PHONE_FORMAT_ID == p.FK_PHONE_FORMAT_ID));
                        (FindName("Phone_" + pm[i]) as MaskedTextBox).Text = p.NUMBER;
                    }
                }

                if (em.Count != 0)
                {
                    var e = em.FirstOrDefault(ema => ema.COMMENT == c[i].ToString());
                    (FindName("Email_" + pm[i]) as TextBox).Text = e != null ? e.EMAIL1 : "";
                }
            }
        }
        private void FillingOpeningHours()
        {
            var oh = WSC.OpeningHours(ID);

            if (oh.Count == 0) 
            { 
                return; 
            }
            foreach(OPENING_HOURS h in oh)
            {
                (FindName("OPENING_HOURS_" + h.DAY + "_0") as TimePicker).Text =
                    h.START.ToString().Substring(0, h.START.ToString().Length - 3);
                (FindName("OPENING_HOURS_" + h.DAY + "_1") as TimePicker).Text =
                    h.END.ToString().Substring(0, h.END.ToString().Length - 3);
            }
        }
        #endregion Filling

        #region Update

        private void UpdateComboBox()
        {
            int[,] index = new int[,]
            {
                { TYPE_ACTIVITY.SelectedIndex, TYPE_ACTIVITY.Items.Count },
                { PROFILE.SelectedIndex, PROFILE.Items.Count },
                { TAG.SelectedIndex, TAG.Items.Count }
            };

            TIME_ZONE.ItemsSource = WSC.AllTimeZone();
            TYPE_ACTIVITY.ItemsSource = WSC.AllTypeActivity();
            PROFILE.ItemsSource = WSC.AllProfile();
            TAG.ItemsSource = WSC.AllTag();

            PhoneFormat_Organization.ItemsSource = WSC.AllPhoneFormat();
            PhoneFormat_Manager.ItemsSource = WSC.AllPhoneFormat();
            PhoneFormat_Additional.ItemsSource = WSC.AllPhoneFormat();

            TYPE_ACTIVITY.SelectedIndex = GC.NewIndexComboBox(TYPE_ACTIVITY.Items.Count, index[0, 1], index[0, 0]);
            PROFILE.SelectedIndex = GC.NewIndexComboBox(PROFILE.Items.Count, index[1, 1], index[1, 0]);
            TAG.SelectedIndex = GC.NewIndexComboBox(TAG.Items.Count, index[2, 1], index[2, 0]);
        }

        private void FormOwnership_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Активировать поле ввода ИНН после первого выбора вида собственности.
            if (!INN.IsEnabled)
            {
                INN.IsEnabled = true;
            }

            string inn = INN.Text;

            INN.Text = "";
            INN.Mask = FORM_OWNERSHIP.SelectedIndex == 1 ? "000000000000" : "0000000000";
            INN.Text = inn;
        }

        private void PhoneFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = GC.GetName((sender as ComboBox).Name);

            if (!(FindName("Phone_" + name) as MaskedTextBox).IsEnabled)
                (FindName("Phone_" + name) as MaskedTextBox).IsEnabled = true;

            (FindName("Phone_" + name) as MaskedTextBox).Text = "";
            (FindName("Phone_" + name) as MaskedTextBox).Mask = WSC.PhoneFormat((int)(sender as ComboBox).SelectedValue);
        }

        #endregion Update

        #region Tag

        private void AddTag(object sender, RoutedEventArgs e)
        {
            if(TAG.SelectedIndex > -1)
                DataGrid_Tag.Items.Add(WSC.Tag(int.Parse(TAG.SelectedValue.ToString())));
        }
        private void DelTag(object sender, RoutedEventArgs e)
        {
            if (DataGrid_Tag.SelectedIndex > -1)
                DataGrid_Tag.Items.Remove(DataGrid_Tag.SelectedItem);
        }
        
        #endregion Tag
        
        
        private string GetText(MaskedTextBox mtb)
        {
            string res = "";
            for (int i = 0; i < mtb.Mask.Length; i++)
            { res += mtb.Mask[i] == '0' ? mtb.Text[i].ToString() : ""; }
            return res;
        }
        private string GetFullAddress()
        {
            string address = "";
            for (int i = 0; i < 4; i++)
            { address += (FindName("ADDRESS_" + i) as TextBox).Text + (i != 3 ? "^" : ""); }
            return address;
        }
        private string[,] GetOpeningHours()
        {
            string[,] oh = new string[7,2];

            for (int i = 0; i < 7; i++)
            {
                oh[i, 0] = (FindName("OPENING_HOURS_" + i + "_0") as TimePicker).Text;
                oh[i, 1] = (FindName("OPENING_HOURS_" + i + "_1") as TimePicker).Text;
            }

            return oh;
        }
    }    
}