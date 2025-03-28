using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.ViewModels.Windows;
using TextBox = System.Windows.Controls.TextBox;
using ComboBox = System.Windows.Controls.ComboBox;
using Brushes = System.Windows.Media.Brushes;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VCheckViewer.Views.Pages.Setting.User
{
    /// <summary>
    /// Interaction logic for UpdateUserPage.xaml
    /// </summary>
    public partial class UpdateUserPage : Page
    {
        UserModel userInfoUpdatePage = new UserModel();

        public ObservableCollection<ComboBoxItem> cbTitle { get; set; }
        public ObservableCollection<ComboBoxItem> cbGender { get; set; }
        public ObservableCollection<ComboBoxItem> cbRoles { get; set; }
        public ObservableCollection<ComboBoxItem> cbStatus { get; set; }
        public ComboBoxItem SelectedcbTitle { get; set; }
        public ComboBoxItem SelectedcbGender { get; set; }
        public ComboBoxItem SelectedcbRoles { get; set; }
        public ComboBoxItem SelectedcbStatus { get; set; }


        public static event EventHandler UpdateUser;

        public UpdateUserPage()
        {
            InitializeComponent();
            DataContext = this;

            cbTitle = App.MainViewModel.cbTitle;
            cbGender = App.MainViewModel.cbGender;
            cbRoles = App.MainViewModel.cbRoles;
            cbStatus = App.MainViewModel.cbStatus;

            userInfoUpdatePage = App.MainViewModel.Users;

            SelectedcbTitle = cbTitle.Where(a => (string)a.Content == userInfoUpdatePage.Title).FirstOrDefault();
            SelectedcbGender = cbGender.Where(a => (string)a.Content == userInfoUpdatePage.Gender).FirstOrDefault();
            SelectedcbRoles = cbRoles.Where(a => (string)a.Content == userInfoUpdatePage.Role).FirstOrDefault();
            SelectedcbStatus = cbStatus.Where(a => (string)a.Content == userInfoUpdatePage.Status).FirstOrDefault();

            //Surname.Text = userInfo.FirstName;
            FullName.Text = userInfoUpdatePage.FullName;
            StaffID.Text = userInfoUpdatePage.EmployeeID;
            RegistrationNo.Text = userInfoUpdatePage.RegistrationNo;
            DateOfBirth.Text = userInfoUpdatePage.DateOfBirth;
            EmailAddress.Text = userInfoUpdatePage.EmailAddress;
            LoginID.Text = userInfoUpdatePage.LoginID;

            if (App.MainViewModel.CurrentUsers.Role == "Lab User")
            {
                btnSettings.IsEnabled = false;
                btnDeviceSetting.IsEnabled = false;

                UserPage.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnSettings.IsEnabled = true;
                btnDeviceSetting.IsEnabled = true;

                UserPage.DataContext = App.MainViewModel;

                App.MainViewModel.BackButtonText = Properties.Resources.Setting_Label_UserBackButton;
            }
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



            if ((textBox != null && textBox.Text == "") || (comboBox != null && comboBox.Text == "") || (datePicker != null && datePicker.Text == ""))
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "This is a mandatory field.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
                CheckAllValueExisted();
            }
            else if (textBox != null && textBox.Name == "EmailAddress" && !textBox.Text.Contains("@"))
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "This field must include “@” symbol.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_EmailFormat;
                CheckAllValueExisted();
            }
            else if (textBox != null && (textBox.Name == "FullName") && textBox.Text.Length < 2)
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                //parent.ToolTip = "This field must contain at least 2 characters.";
                parent.ToolTip = Properties.Resources.Setting_ErrorMessage_TwoCharMin;
                CheckAllValueExisted();
            }
            else if (textBox != null && (textBox.Name == "StaffID" || textBox.Name == "RegistrationNo") && textBox.Text.Length < 5)
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
            //DateTime temp;

            if (Convert.ToString((StaffID.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Title.Parent as Border).ToolTip) == "No issue" && Convert.ToString((FullName.Parent as Border).ToolTip) == "No issue" && Convert.ToString((RegistrationNo.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Gender.Parent as Border).ToolTip) == "No issue" && Convert.ToString((DateOfBirth.Parent as Border).ToolTip) == "No issue" && Convert.ToString((EmailAddress.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Status.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Role.Parent as Border).ToolTip) == "No issue")
            {
                Update.IsEnabled = true;
            }
            else
            {
                Update.IsEnabled = false;
            }

            ErrorText.Visibility = Visibility.Hidden;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = new UserModel()
            {
                UserId = userInfoUpdatePage.UserId,
                EmployeeID = StaffID.Text,
                Title = Title.Text,
                //FirstName = Surname.Text,
                //LastName = LastName.Text,
                FullName = FullName.Text,
                RegistrationNo = RegistrationNo.Text,
                Gender = ((ComboBoxItem)Gender.SelectedItem).Tag.ToString(),
                DateOfBirth = Convert.ToDateTime(DateOfBirth.Text).ToString("yyyy-MM-dd"),
                EmailAddress = EmailAddress.Text,
                StatusID = Convert.ToInt32(((ComboBoxItem)Status.SelectedItem).Tag.ToString()),
                Status = Status.Text,
                RoleID = ((ComboBoxItem)Role.SelectedItem).Tag.ToString(),
                Role = Role.Text
            };

            if(user.Title != userInfoUpdatePage.Title || user.FullName != userInfoUpdatePage.FullName || user.EmployeeID != userInfoUpdatePage.EmployeeID || user.RegistrationNo != userInfoUpdatePage.RegistrationNo
                || Gender.Text != userInfoUpdatePage.Gender || DateTime.Parse(user.DateOfBirth)  != DateTime.Parse(userInfoUpdatePage.DateOfBirth) || user.Role != userInfoUpdatePage.Role || user.EmailAddress != userInfoUpdatePage.EmailAddress || user.Status != userInfoUpdatePage.Status)
            {
                App.MainViewModel.Origin = "UserUpdateRow";

                if(user.Status != userInfoUpdatePage.Status) { user.StatusChanged = true; }
                else { user.StatusChanged = false; }

                if(user.EmailAddress != userInfoUpdatePage.EmailAddress) { user.EmailAddressChanged = true; }
                else { user.EmailAddressChanged= false; }

                if(user.Role != userInfoUpdatePage.Role) {  user.RoleChanged = true; }
                else { user.RoleChanged= false; }

                App.MainViewModel.Users = user;

                App.PopupHandler(e, sender);
            }
            else
            {
                ErrorText.Visibility = Visibility.Visible;
            }

            //App.MainViewModel.Origin = "UserUpdateRow";

            //App.MainViewModel.Users = user;

            //App.MainViewModel.Users.UserId = userInfo.UserId;
            //App.MainViewModel.Users.EmployeeID = StaffID.Text;
            //App.MainViewModel.Users.Title = Title.Text;
            //App.MainViewModel.Users.FullName = FullName.Text;
            //App.MainViewModel.Users.RegistrationNo = RegistrationNo.Text;
            //App.MainViewModel.Users.Gender = ((ComboBoxItem)Gender.SelectedItem).Tag.ToString();
            //App.MainViewModel.Users.DateOfBirth = Convert.ToDateTime(DateOfBirth.Text).ToString("yyyy-MM-dd");
            //App.MainViewModel.Users.EmailAddress = EmailAddress.Text;
            //App.MainViewModel.Users.StatusID = Convert.ToInt32(((ComboBoxItem)Status.SelectedItem).Tag.ToString());
            //App.MainViewModel.Users.NewStatus = Status.Text;
            //App.MainViewModel.Users.RoleID = ((ComboBoxItem)Role.SelectedItem).Tag.ToString();
            //App.MainViewModel.Users.Role = Role.Text;

            //App.PopupHandler(e, sender);
        }

        private void LanguageCountry(object sender, RoutedEventArgs e)
        {
            App.GoToSettingLanguageCountryPageHandler(e, sender);
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingDevicePageHandler(e, sender);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingConfigurationPageHandler(e, sender);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingReportPageHandler(e, sender);
        }
    }
}
