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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;

namespace VCheckViewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        UserDBContext sContext = App.GetService<UserDBContext>();
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
            SelectedcbStatus = cbStatus.FirstOrDefault();
        }

        public static event EventHandler GoToMainUserPage;
        public static void GoToMainUserPageHandler(EventArgs e, object sender)
        {
            if (GoToMainUserPage != null)
            {
                GoToMainUserPage(sender, e);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GoToMainUserPageHandler(e, sender);
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

            sContext.InsertUser(user);

            GoToMainUserPageHandler(e, sender);
        }

    }
}
