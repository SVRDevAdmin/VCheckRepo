using System;
using System.Collections.Generic;
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
using VCheck.Lib.Data.Models;
using VCheckViewer.ViewModels.Windows;

namespace VCheckViewer.Views.Pages.Setting.User
{
    /// <summary>
    /// Interaction logic for UpdateUserPage.xaml
    /// </summary>
    public partial class UpdateUserPage : Page
    {
        VCheck.Lib.Data.DBContext.UserDBContext sContext = VCheckViewer.App.GetService<VCheck.Lib.Data.DBContext.UserDBContext>();

        UserModel userInfo;
        public UpdateUserPage()
        {
            InitializeComponent();

            //Title.SelectedValue = userInfo.Title;
            userInfo = App.MainViewModel.Users;

            Title.Text = userInfo.Title;
            Surname.Text = userInfo.FirstName;
            LastName.Text = userInfo.LastName;
            StaffID.Text = userInfo.EmployeeID;
            RegistrationNo.Text = userInfo.RegistrationNo;
            Gender.Text = userInfo.Gender;
            DateOfBirth.Text = userInfo.DateOfBirth;
            Role.Text = userInfo.Role;
            EmailAddress.Text = userInfo.EmailAddress;
            Status.Text = userInfo.Status;
            
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
