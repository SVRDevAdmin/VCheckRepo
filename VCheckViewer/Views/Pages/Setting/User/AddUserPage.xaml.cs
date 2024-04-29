using System.Collections.ObjectModel;
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


        public static event EventHandler AddUser;


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
            SelectedcbStatus = cbStatus.FirstOrDefault();
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
                parent.ToolTip = "This field must include “@” symbol.";
                CheckAllValueExisted();
            }
            else if (textBox != null && (textBox.Name == "Surname" || textBox.Name == "LastName") && textBox.Text.Length < 2)
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                parent.ToolTip = "This field must contain at least 2 characters.";
                CheckAllValueExisted();
            }
            else if (textBox != null && (textBox.Name == "StaffID" || textBox.Name == "RegistrationNo") && textBox.Text.Length < 5)
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                parent.ToolTip = "This field must contain at least 5 characters.";
                CheckAllValueExisted();
            }
            else if (datePicker != null && !DateTime.TryParse(datePicker.Text.ToString(), out temp))
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                parent.ToolTip = "Please key in correct date format.";
                CheckAllValueExisted();
            }
            else if ((textBox != null && textBox.Text == "") || (comboBox != null && comboBox.Text == "") || (datePicker != null && datePicker.Text == ""))
            {
                parent.BorderBrush = Brushes.Red;
                parent.BorderThickness = new Thickness(1);
                parent.ToolTip = "This is a mandatory field.";
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

            if (Convert.ToString((StaffID.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Title.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Surname.Parent as Border).ToolTip) == "No issue" && Convert.ToString((LastName.Parent as Border).ToolTip) == "No issue" && Convert.ToString((RegistrationNo.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Gender.Parent as Border).ToolTip) == "No issue" && Convert.ToString((DateOfBirth.Parent as Border).ToolTip) == "No issue" && Convert.ToString((EmailAddress.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Status.Parent as Border).ToolTip) == "No issue" && Convert.ToString((Role.Parent as Border).ToolTip) == "No issue" && DateTime.TryParse(DateOfBirth.ToString(), out temp))
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
                FirstName = Surname.Text,
                LastName = LastName.Text,
                RegistrationNo = RegistrationNo.Text,
                Gender = ((ComboBoxItem)Gender.SelectedItem).Tag.ToString(),
                DateOfBirth = Convert.ToDateTime(DateOfBirth.Text).ToString("yyyy-MM-dd"),
                EmailAddress = EmailAddress.Text,
                StatusID = Convert.ToInt32(((ComboBoxItem)Status.SelectedItem).Tag.ToString()),
                RoleID = Convert.ToInt32(((ComboBoxItem)Role.SelectedItem).Tag.ToString())
            };

            Popup popup = new Popup();
            popup.IsOpen = true;


            App.MainViewModel.Origin = "UserAddRow";

            App.MainViewModel.Users = user;

            App.PopupHandler(e, sender);
        }
    }
}
