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

namespace VCheckViewer.Views.Pages.Setting.User
{
    /// <summary>
    /// Interaction logic for UpdateUserPage.xaml
    /// </summary>
    public partial class UpdateUserPage : Page
    {
        UserDBContext sContext = App.GetService<UserDBContext>();

        UserModel userInfo;

        public ObservableCollection<ComboBoxItem> cbTitle { get; set; }
        public ObservableCollection<ComboBoxItem> cbGender { get; set; }
        public ObservableCollection<ComboBoxItem> cbRoles { get; set; }
        public ObservableCollection<ComboBoxItem> cbStatus { get; set; }
        public ComboBoxItem SelectedcbTitle { get; set; }
        public ComboBoxItem SelectedcbGender { get; set; }
        public ComboBoxItem SelectedcbRoles { get; set; }
        public ComboBoxItem SelectedcbStatus { get; set; }

        public UpdateUserPage()
        {
            InitializeComponent();
            DataContext = this;

            cbTitle = App.MainViewModel.cbTitle;
            cbGender = App.MainViewModel.cbGender;
            cbRoles = App.MainViewModel.cbRoles;
            cbStatus = App.MainViewModel.cbStatus;

            userInfo = App.MainViewModel.Users;

            var test = cbTitle.Where(a => a.Content == userInfo.Title);

            SelectedcbTitle = cbTitle.Where(a => (string)a.Content == userInfo.Title).FirstOrDefault();
            SelectedcbGender = cbGender.Where(a => (string)a.Content == userInfo.Gender).FirstOrDefault();
            SelectedcbRoles = cbRoles.Where(a => (string)a.Content == userInfo.Role).FirstOrDefault();
            SelectedcbStatus = cbStatus.Where(a => (string)a.Content == userInfo.Status).FirstOrDefault();

            Surname.Text = userInfo.FirstName;
            LastName.Text = userInfo.LastName;
            StaffID.Text = userInfo.EmployeeID;
            RegistrationNo.Text = userInfo.RegistrationNo;
            DateOfBirth.Text = userInfo.DateOfBirth;
            EmailAddress.Text = userInfo.EmailAddress;

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserPage.GoToMainUserPageHandler(e, sender);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = new UserModel()
            {
                UserId = userInfo.UserId,
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

            sContext.UpdateUser(user);

            AddUserPage.GoToMainUserPageHandler(e, sender);
        }
    }
}
