using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml.Linq;
using VCheck.Lib.Data.Models;
using TextBox = System.Windows.Controls.TextBox;
using ComboBox = System.Windows.Controls.ComboBox;
using Brushes = System.Windows.Media.Brushes;

namespace VCheckViewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        public ObservableCollection<ComboBoxItem> cbTitle { get; set; }
        public ObservableCollection<ComboBoxItem> cbGender { get; set; }
        public ObservableCollection<ComboBoxItem> cbRoles { get; set; }
        public ObservableCollection<ComboBoxItem> cbStatus { get; set; }
        public ComboBoxItem SelectedcbTitle { get; set; }
        public ComboBoxItem SelectedcbGender { get; set; }
        public ComboBoxItem SelectedcbRoles { get; set; }
        public ComboBoxItem SelectedcbStatus { get; set; }


        public AddUserPage()
        {
            InitializeComponent();
            DataContext = this;

            cbTitle = App.MainViewModel.cbTitle;
            cbGender = App.MainViewModel.cbGender;
            cbRoles = App.MainViewModel.cbRoles;
            cbStatus = App.MainViewModel.cbStatus;

            SelectedcbTitle = cbTitle.FirstOrDefault();
            SelectedcbGender = cbGender.FirstOrDefault();
            SelectedcbRoles = cbRoles.FirstOrDefault();
            SelectedcbStatus = cbStatus.Where(x => x.Content.ToString() == "Active").FirstOrDefault();

            UserPage.DataContext = App.MainViewModel;

            App.MainViewModel.BackButtonText = Properties.Resources.Setting_Label_UserBackButton;

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.GoPreviousPageHandler(e, sender);
        }

        private void CheckValueExist(object sender, RoutedEventArgs e)
        {
            CheckValue(sender);
        }

        private void CheckValue_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CheckValue(sender);
        }

        private void CheckValue(Object element)
        {
            ComboBox comboBox = null;
            DatePicker datePicker = null;
            TextBox textBox = null;
            Border parent;
            DateTime temp;

            if (element.GetType() == typeof(ComboBox)) { comboBox = element as ComboBox; parent = comboBox.Parent as Border; }
            else if (element.GetType() == typeof(TextBox)) { textBox = element as TextBox; parent = textBox.Parent as Border; }
            else { datePicker = element as DatePicker; parent = datePicker.Parent as Border; }

            if (textBox != null && textBox.Name == "EmailAddress" && !textBox.Text.Contains("@"))
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "This field must include “@” symbol.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_EmailFormat;
                CheckAllValueExisted();
            }
            else if (textBox != null && textBox.Name == "FullName" && textBox.Text.Length < 2)
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "This field must contain at least 2 characters.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_TwoCharMin;
                CheckAllValueExisted();
            }
            else if (textBox != null && (textBox.Name == "StaffID" || textBox.Name == "RegistrationNo" || textBox.Name == "LoginID") && textBox.Text.Length < 5)
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "This field must contain at least 5 characters.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_FiveCharMin;
                CheckAllValueExisted();
            }
            else if (datePicker != null && !DateTime.TryParse(datePicker.Text.ToString(), out temp))
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "Please key in correct date format.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_DateFormat;
                CheckAllValueExisted();
            }
            else if ((textBox != null && textBox.Text == "") || (comboBox != null && comboBox.Text == "") || (datePicker != null && datePicker.Text == ""))
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "This is a mandatory field.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
                CheckAllValueExisted();
            }
            else
            {
                parent.BorderBrush = Brushes.Black;
                parent.BorderThickness = new Thickness(1);
                parent.ToolTip = "No issue";
                CheckAllValueExisted();
            }
        }

        private void CheckAllValueExisted()
        {
            DateTime temp;

            if (Convert.ToString((StaffID.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Title.Parent as Border).ToolTip) == "No issue" && Convert.ToString((FullName.Parent as Border).ToolTip) == "No issue" && Convert.ToString((RegistrationNo.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Gender.Parent as Border).ToolTip) == "No issue" && Convert.ToString((DateOfBirth.Parent as Border).ToolTip) == "No issue" && Convert.ToString((EmailAddress.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Role.Parent as Border).ToolTip) == "No issue" && DateTime.TryParse(DateOfBirth.ToString(), out temp) && Convert.ToString((LoginID.Parent as Border).ToolTip) == "No issue")
            {
                Create.IsEnabled = true;
            }
            else
            {
                Create.IsEnabled = false;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = new UserModel()
            {
                EmployeeID = StaffID.Text,
                Title = Title.Text,
                //FirstName = Surname.Text,
                FullName = FullName.Text,
                RegistrationNo = RegistrationNo.Text,
                Gender = ((ComboBoxItem)Gender.SelectedItem).Tag.ToString(),
                DateOfBirth = Convert.ToDateTime(DateOfBirth.Text).ToString("yyyy-MM-dd"),
                EmailAddress = EmailAddress.Text,
                StatusID = Convert.ToInt32(((ComboBoxItem)Status.SelectedItem).Tag.ToString()),
                RoleID = ((ComboBoxItem)Role.SelectedItem).Tag.ToString(),
                Role = Role.Text,
                LoginID = LoginID.Text
            };

            Popup popup = new Popup();
            popup.IsOpen = true;


            App.MainViewModel.Origin = "UserAddRow";

            App.MainViewModel.Users = user;

            App.newPassword = RandomPasswordGenerator();

            App.PopupHandler(e, sender);
        }

        private void LanguageCountry(object sender, RoutedEventArgs e)
        {
            App.GoToSettingLanguageCountryPageHandler(e, sender);
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingDevicePageHandler(e, sender);
        }

        //private string RandomPasswordGenerator()
        //{
        //    string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        //    string digits = "0123456789";
        //    string specialChars = "!@#$%^&*()-_=+<>?/:;";

        //    int length = 8;
        //    bool includeUppercase = true;
        //    bool includeLowercase = true;
        //    bool includeDigits = true;
        //    bool includeSpecialChars = true;

        //    StringBuilder charSet = new StringBuilder();
        //    if (includeUppercase) charSet.Append(uppercaseLetters);
        //    if (includeLowercase) charSet.Append(lowercaseLetters);
        //    if (includeDigits) charSet.Append(digits);
        //    if (includeSpecialChars) charSet.Append(specialChars);

        //    StringBuilder password = new StringBuilder(length);
        //    Random rnd = new Random();
        //    bool passwordIncorrect = true;

        //    while (passwordIncorrect)
        //    {
        //        password.Clear();

        //        for (int i = 0; i < length; i++)
        //        {
        //            int randomIndex = rnd.Next(charSet.Length);
        //            password.Append(charSet[randomIndex]);
        //        }


        //        if (password.ToString().IndexOfAny(specialChars.ToCharArray()) != -1 && password.ToString().IndexOfAny(uppercaseLetters.ToCharArray()) != -1 && password.ToString().IndexOfAny(lowercaseLetters.ToCharArray()) != -1 && password.ToString().IndexOfAny(digits.ToCharArray()) != -1)
        //        {
        //            passwordIncorrect = false;
        //        }
        //    }


        //    return password.ToString();
        //}

        private string RandomPasswordGenerator()
        {
            Random res = new Random();

            // String that contain alphabets, numbers and special character
            String lowerCase = "abcdefghijklmnopqrstuvwxyz";
            String upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            String number = "0123456789";
            String specialChar = "!@#$%^&*()_+-={}|[];,./:<>?";

            // Initializing the empty string 
            String randomstring = "";

            randomstring += lowerCase[res.Next(lowerCase.Length)];
            randomstring += upperCase[res.Next(upperCase.Length)];
            randomstring += number[res.Next(number.Length)];
            randomstring += specialChar[res.Next(specialChar.Length)];
            randomstring += lowerCase[res.Next(lowerCase.Length)];
            randomstring += upperCase[res.Next(upperCase.Length)];
            randomstring += number[res.Next(number.Length)];
            randomstring += specialChar[res.Next(specialChar.Length)];

            return randomstring;
        }
    }
}
