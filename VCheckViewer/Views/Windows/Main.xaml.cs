using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VCheck.Helper;
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Culture;
using VCheckViewer.Lib.Function;
using VCheckViewer.Views.Pages;
using VCheckViewer.Views.Pages.Login;
using VCheckViewer.Views.Pages.Notification;
using VCheckViewer.Views.Pages.Setting.Device;
using VCheckViewer.Views.Pages.Setting.LanguageCountry;
using VCheckViewer.Views.Pages.Setting.User;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Brushes = System.Windows.Media.Brushes;
using VCheckViewer.Views.Pages.Setting.Interface;
using VCheckViewer.Views.Pages.Schedule;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Windows.Media;
using VCheckViewer.Views.Pages.Results;
using Org.BouncyCastle.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.Logging;
using DocumentFormat.OpenXml.Drawing;
using System.Runtime.Caching;

namespace VCheckViewer.Views.Windows
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    //public partial class Main : INavigationWindow
    public partial class Main : Window
    {
        INavigationService _navigationService;
        IPageService _pageService;

        static MasterCodeDataDBContext sContext = App.GetService<MasterCodeDataDBContext>();
        static RolesDBContext rolesContext = App.GetService<RolesDBContext>();
        static UserDBContext usersContext = App.GetService<UserDBContext>();
        static DeviceDBContext deviceContext = App.GetService<DeviceDBContext>();

        //List<MasterCodeDataModel> masterCodeDataList = sContext.GetMasterCodeData();
        List<RolesModel> roleList = rolesContext.GetRoles();
        ConfigurationDBContext ConfigurationContext = App.GetService<ConfigurationDBContext>();
        TemplateDBContext TemplateContext = App.GetService<TemplateDBContext>();
        NotificationDBContext NotificationContext = App.GetService<NotificationDBContext>();

        public static event EventHandler InitializedUserPage;
        public static event EventHandler ResetCurrentPassword;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Main()
        {
            InitializeComponent();
            DataContext = this;

            //page
            UserPage.GoToAddUserPage += new EventHandler(GoToAddUserPage);
            UserPage.GoToUpdateUserPage += new EventHandler(GoToUpdateUserPage);
            UserPage.GoToViewUserPage += new EventHandler(GoToViewUserPage);
            ViewUserPage.GoToUpdateCurrentUserPage += new EventHandler(GoToUpdateUserPage);
            UserPage.GoToLanguageCountryPage += new EventHandler(GoToLanguageCountryPage);

            App.GoToSettingUserPage += new EventHandler(SettingUserPage);
            App.GoToSettingLanguageCountryPage += new EventHandler(GoToLanguageCountryPage);
            App.GoToSettingDevicePage += new EventHandler(GoToDevicePage);
            App.GoToSettingConfigurationPage += new EventHandler(GoToConfigurationPage);

            //popup
            App.Popup += new EventHandler(Popup);
            App.GoPreviousPage += new EventHandler(PreviousPage);

            initializedDropdownSelectionList();

            Username.Header = App.MainViewModel.CurrentUsers.StaffName;

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Dashboard_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            CheckThemesSettings();

            refreshMenuItemStyle(mnDashboard);
            //mnDashboard.Background = System.Windows.Media.Brushes.White;
            //mnDashboard.BorderBrush = new BrushConverter().ConvertFrom("#404D5B") as SolidColorBrush;
        }

        #region INavigationWindow methods

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        private void ViewProfileButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Users = App.MainViewModel.CurrentUsers;

            if (frameContent.CanGoBack) { frameContent.GoBack(); }

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Setting_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            GoToViewUserPage(sender, e);
        }

        private void ResetPassword(object sender, RoutedEventArgs e)
        {
            frameContent.Content = new ResetCurrentUserPasswordPage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("ResetPassword_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Origin = "Logout";
            Popup(sender, e);
        }

        void GoToAddUserPage(object sender, EventArgs e)
        {
            frameContent.Content = new AddUserPage();
        }

        void GoToUpdateUserPage(object sender, EventArgs e)
        {
            frameContent.Content = new UpdateUserPage();
        }

        void GoToViewUserPage(object sender, EventArgs e)
        {
            frameContent.Content = new ViewUserPage();
        }

        void GoToLanguageCountryPage(object sender, EventArgs e)
        {
            frameContent.Content = new LanguageCountryPage();
        }
        void GoToDevicePage(object sender, EventArgs e)
        {
            frameContent.Content = new DevicePage();
        }

        void GoToConfigurationPage(object sender, EventArgs e)
        {
            frameContent.Content = new ConfigurationPage();
        }

        void SettingUserPage(object sender, EventArgs e)
        {
            frameContent.Content = new UserPage();
        }

        void Popup(object sender, EventArgs e)
        {
            if (App.MainViewModel.Origin == "UserDeleteRow") { PopupContent.Text = Properties.Resources.Popup_Message_DeleteUser; }
            if (App.MainViewModel.Origin == "UserAddRow") { PopupContent.Text = Properties.Resources.Popup_Message_CreateUser; }
            if (App.MainViewModel.Origin == "UserUpdateRow") { PopupContent.Text = Properties.Resources.Popup_Message_UpdateUser; }
            if (App.MainViewModel.Origin == "ChangeLanguageCountry") { PopupContent.Text = Properties.Resources.Popup_Message_LanguageCountryChange; }
            if (App.MainViewModel.Origin == "Logout") { PopupContent.Text = Properties.Resources.Popup_Message_Logout; }
            if (App.MainViewModel.Origin == "ResetPassword") { PopupContent.Text = Properties.Resources.Popup_Message_ResetPassword; }
            if (App.MainViewModel.Origin == "DeviceAdd") { PopupContent.Text = Properties.Resources.Popup_Message_AddAnalyzer;  }
            if (App.MainViewModel.Origin == "DeviceDelete") { PopupContent.Text = Properties.Resources.Popup_Message_RemoveAnalyzer; }
            if (App.MainViewModel.Origin == "DeviceUpdate") { PopupContent.Text = Properties.Resources.Popup_Message_UpdateAnalyzer; }
            if (App.MainViewModel.Origin == "SettingsUpdate") { PopupContent.Text = Properties.Resources.Popup_Message_SaveSettings; }
            if (App.MainViewModel.Origin == "SetingsUpdateCompleted") {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Popup_Message_SavePMSLISHIS; 
            }
            if (App.MainViewModel.Origin == "ListingDownloadCompleted") {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Results_Message_DownloadComplete; 
            }
            if (App.MainViewModel.Origin == "TestResultDownloadCompleted")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Results_Message_TestResultDownloadCompleted;
            }
            if (App.MainViewModel.Origin == "FailedDownloadListing")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedDownloadListing;
            }
            if (App.MainViewModel.Origin == "FailedToShowPrint")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedOpenPrint;
            }
            if (App.MainViewModel.Origin == "FailedAddDevice")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedAddDevice;
            }
            if (App.MainViewModel.Origin == "FailedDeleteDevice")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedDeleteDevice;
            }
            if (App.MainViewModel.Origin == "FailedUpdateDevice")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedUpdateDevice;
            }

            PopupBackground.Background = Brushes.DimGray;
            PopupBackground.Opacity = 0.5;
            popup.IsOpen = true;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;

            PopupBackground.Background = null;

            if (App.MainViewModel.Origin == "UserDeleteRow") { DeleteUserRowHandler(e, sender); }
            if (App.MainViewModel.Origin == "UserAddRow") { AddUserRowHandler(e, sender); }
            if (App.MainViewModel.Origin == "UserUpdateRow") { UpdateUserRowHandler(e, sender); }
            if (App.MainViewModel.Origin == "DeviceAdd") { AddDeviceHandler(e, sender); }
            if (App.MainViewModel.Origin == "DeviceDelete") { DeleteDeviceHandler(e, sender); }
            if (App.MainViewModel.Origin == "DeviceUpdate") { UpdateDeviceHandler(e, sender); }
            if (App.MainViewModel.Origin == "ChangeLanguageCountry") { ChangeLanguageCountryHandler(e, sender); }
            if (App.MainViewModel.Origin == "SettingsUpdate") { UpdateSettingsHandler(e, sender);  }
            if (App.MainViewModel.Origin == "Logout")
            {
                if (!System.Windows.Application.Current.Windows.OfType<LoginWindow>().Any())
                {
                    LoginWindow login = new LoginWindow();
                    login.LoginFrame.Content = new LoginPage();
                    login.Show();
                }

                Close();
            }
            if (App.MainViewModel.Origin == "ResetPassword") { ResetPasswordHandler(e, sender); }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable main window
            this.IsEnabled = true;
            // Close the popup
            popup.IsOpen = false;

            PopupBackground.Background = null;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (App.MainViewModel.Origin == "SetingsUpdateCompleted" || 
                App.MainViewModel.Origin == "ListingDownloadCompleted" ||
                App.MainViewModel.Origin == "TestResultDownloadCompleted" ||
                App.MainViewModel.Origin == "FailedDownloadListing" ||
                App.MainViewModel.Origin == "FailedToShowPrint" ||
                App.MainViewModel.Origin == "FailedAddDevice" ||
                App.MainViewModel.Origin == "FailedDeleteDevice" ||
                App.MainViewModel.Origin == "FailedUpdateDevice")
            {
                this.IsEnabled = true;
                popup.IsOpen = false;

                PopupBackground.Background = null;

                btnOk.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Visible;
                btnNo.Visibility = Visibility.Visible;
            }
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            var originButton = (System.Windows.Controls.Button)sender;

            if (originButton.Name.ToString() == "UserPage" || App.MainViewModel.Origin == "UserUpdateRow")
            {
                MainUserPage();
            }
            else
            {
                frameContent.GoBack();
            }
        }

        private void DashboardPage()
        {
            frameContent.Content = new DashboardPage();
        }

        private void MainUserPage()
        {
            frameContent.Content = new UserPage();
        }

        private void isActive(NavigationViewItem navigationItem)
        {
            navigationItem.IsActive = true;
        }

        public void initializedDropdownSelectionList()
        {
            var titleList = sContext.GetMasterCodeData("Title");
            var genderList = sContext.GetMasterCodeData("Gender");
            var rolesList = rolesContext.GetRoles();
            var statusList = sContext.GetMasterCodeData("UserStatus");
            var sortDirectionList = sContext.GetMasterCodeData("SortDirection");

            App.MainViewModel.cbTitle = new ObservableCollection<ComboBoxItem>();
            App.MainViewModel.cbGender = new ObservableCollection<ComboBoxItem>();
            App.MainViewModel.cbRoles = new ObservableCollection<ComboBoxItem>();
            App.MainViewModel.cbStatus = new ObservableCollection<ComboBoxItem>();
            App.MainViewModel.cbSort = new ObservableCollection<ComboBoxItem>();


            var cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbTitle = cbDefaultItem;
            App.MainViewModel.cbTitle.Add(cbDefaultItem);
            foreach (var item in titleList) { App.MainViewModel.cbTitle.Add(new ComboBoxItem { Content = item.CodeID }); }

            cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbGender = cbDefaultItem;
            App.MainViewModel.cbGender.Add(cbDefaultItem);
            foreach (var item in genderList) { App.MainViewModel.cbGender.Add(new ComboBoxItem { Content = item.CodeName, Tag = item.CodeID }); }

            cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbRoles = cbDefaultItem;
            App.MainViewModel.cbRoles.Add(cbDefaultItem);
            foreach (var item in rolesList) { App.MainViewModel.cbRoles.Add(new ComboBoxItem { Content = item.RoleName, Tag = item.RoleID }); }

            cbDefaultItem = new ComboBoxItem { Content = "" };
            App.MainViewModel.SelectedcbStatus = cbDefaultItem;
            App.MainViewModel.cbStatus.Add(cbDefaultItem);
            foreach (var item in statusList) { App.MainViewModel.cbStatus.Add(new ComboBoxItem { Content = item.CodeName, Tag = item.CodeID }); }

            //cbDefaultItem = new ComboBoxItem { Content = "" };
            //App.MainViewModel.SelectedcbStatus = cbDefaultItem;
            //App.MainViewModel.cbSort.Add(cbDefaultItem);
            foreach(var item in sortDirectionList)
            {
                App.MainViewModel.cbSort.Add(new ComboBoxItem
                {
                    Content = item.CodeName,
                    Tag = item.CodeID
                });
            }
        }

        private void RootNavigation_PaneClosed(NavigationView sender, RoutedEventArgs args)
        {

        }

        private async static void DeleteUserRowHandler(EventArgs e, object sender)
        {
            try
            {
                if (usersContext.DeleteUser(App.MainViewModel.Users.UserId))
                {
                    var user = await App.UserManager.FindByIdAsync(App.MainViewModel.Users.UserId);

                    await App.UserManager.DeleteAsync(user);

                    if (InitializedUserPage != null)
                    {
                        InitializedUserPage(sender, e);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                App.log.Error("User Deletion Error >>> ", ex);
            }
        }


        private async void AddUserRowHandler(EventArgs e, object sender)
        {
            try
            {
                var user = Activator.CreateInstance<IdentityUser>();

                var emailStore = (IUserEmailStore<IdentityUser>)App.UserStore;

                await App.UserStore.SetUserNameAsync(user, App.MainViewModel.Users.LoginID, CancellationToken.None);
                await emailStore.SetEmailAsync(user, App.MainViewModel.Users.EmailAddress, CancellationToken.None);
                var result = await App.UserManager.CreateAsync(user, App.newPassword);

                ConfigurationModel sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();

                if (result.Succeeded)
                {
                    App.MainViewModel.Users.UserId = user.Id;
                    App.MainViewModel.Users.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    App.MainViewModel.Users.CreatedBy = App.MainViewModel.CurrentUsers.FullName;

                    if (usersContext.InsertUser(App.MainViewModel.Users))
                    {
                        var roleResult = await App.UserManager.AddToRoleAsync(user, App.MainViewModel.Users.Role);

                        if (roleResult.Succeeded)
                        {
                            //var notificationTemplate = TemplateContext.GetTemplateByCode("US05");
                            var notificationTemplate = TemplateContext.GetTemplateByCodeLang("US05", (sLangCode != null ? sLangCode.ConfigurationValue : ""));
                            notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("###<staff_id>###", App.MainViewModel.Users.EmployeeID).Replace("###<staff_fullname>###", App.MainViewModel.Users.FullName).Replace("'", "''");

                            NotificationModel notification = new NotificationModel()
                            {
                                NotificationType = "Updates",
                                NotificationTitle = notificationTemplate.TemplateTitle,
                                NotificationContent = notificationTemplate.TemplateContent,
                                CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CreatedBy = App.MainViewModel.CurrentUsers.FullName
                            };

                            if (NotificationContext.InsertNotification(notification))
                            {

                            }
                            else
                            {

                            }

                            //notificationTemplate = TemplateContext.GetTemplateByCode("EN01");
                            notificationTemplate = TemplateContext.GetTemplateByCodeLang("EN01", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                            notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("'", "''").Replace("###<staff_fullname>###", App.MainViewModel.Users.FullName).Replace("###<password>###", App.newPassword).Replace("###<login_id>###", user.UserName);

                            string sErrorMessage = "";

                            try
                            {
                                EmailObject sEmail = new EmailObject();

                                sEmail.SenderEmail = App.SMTP.Sender;

                                List<string> sRecipientList = [App.MainViewModel.Users.EmailAddress, "azwan@svrtech.com.my"];


                                sEmail.RecipientEmail = sRecipientList;
                                sEmail.IsHtml = true;
                                sEmail.Subject = "[VCheck Viewer] " + notificationTemplate.TemplateTitle;
                                sEmail.Body = notificationTemplate.TemplateContent;
                                sEmail.SMTPHost = App.SMTP.Host;
                                sEmail.PortNo = App.SMTP.Port;
                                sEmail.HostUsername = App.SMTP.Username;
                                sEmail.HostPassword = App.SMTP.Password;
                                sEmail.EnableSsl = true;
                                sEmail.UseDefaultCredentials = false;

                                EmailHelper.SendEmail(sEmail, out sErrorMessage);

                                if(!String.IsNullOrEmpty(sErrorMessage)) { throw new Exception(sErrorMessage); }

                                notification = new NotificationModel()
                                {
                                    NotificationType = "Email",
                                    NotificationTitle = notificationTemplate.TemplateTitle,
                                    NotificationContent = notificationTemplate.TemplateContent,
                                    Receiver = string.Join(", ", sRecipientList),
                                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    CreatedBy = App.MainViewModel.CurrentUsers.FullName
                                };

                                if (NotificationContext.InsertNotification(notification))
                                {

                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                                App.log.Error("Email Error >>> ", ex);
                            }

                            if (InitializedUserPage != null)
                            {
                                InitializedUserPage(sender, e);
                            }
                            PreviousPage(sender, e);
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch(Exception ex)
            {
                App.log.Error("Add User Error >>> ", ex);
            }
        }

        private async void UpdateUserRowHandler(EventArgs e, object sender)
        {
            TemplateModel notificationTemplate;
            App.MainViewModel.Users.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            App.MainViewModel.Users.UpdatedBy = App.MainViewModel.CurrentUsers.FullName;

            ConfigurationModel sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();

            try
            {
                if (usersContext.UpdateUser(App.MainViewModel.Users))
                {
                    if (App.MainViewModel.CurrentUsers.UserId == App.MainViewModel.Users.UserId)
                    {
                        App.MainViewModel.CurrentUsers.EmployeeID = App.MainViewModel.Users.EmployeeID;
                        App.MainViewModel.CurrentUsers.Title = App.MainViewModel.Users.Title;
                        //App.MainViewModel.CurrentUsers.FirstName = Surname.Text;
                        //App.MainViewModel.CurrentUsers.LastName = LastName.Text;
                        App.MainViewModel.CurrentUsers.StaffName = App.MainViewModel.Users.Title + " " + App.MainViewModel.Users.FullName;
                        App.MainViewModel.CurrentUsers.FullName = App.MainViewModel.Users.FullName;
                        App.MainViewModel.CurrentUsers.RegistrationNo = App.MainViewModel.Users.RegistrationNo;
                        App.MainViewModel.CurrentUsers.Gender = App.MainViewModel.Users.Gender == "M" ? "Male" : "Female";
                        App.MainViewModel.CurrentUsers.DateOfBirth = App.MainViewModel.Users.DateOfBirth;
                        App.MainViewModel.CurrentUsers.EmailAddress = App.MainViewModel.Users.EmailAddress;
                        App.MainViewModel.CurrentUsers.Status = App.MainViewModel.Users.Status;
                        App.MainViewModel.CurrentUsers.Role = App.MainViewModel.Users.Role;
                    }

                    if (App.MainViewModel.Users.StatusChanged)
                    {
                        if (App.MainViewModel.Users.Status == "Active")
                        {
                            //notificationTemplate = TemplateContext.GetTemplateByCode("US03");
                            notificationTemplate = TemplateContext.GetTemplateByCodeLang("US03", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                        }
                        else
                        {
                            //notificationTemplate = TemplateContext.GetTemplateByCode("US04");
                            notificationTemplate = TemplateContext.GetTemplateByCodeLang("US04", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                        }

                        notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("'", "''").Replace("###<staff_id>###", App.MainViewModel.Users.EmployeeID).Replace("###<staff_fullname>###", App.MainViewModel.Users.FullName).Replace("###<admin_id>###", App.MainViewModel.CurrentUsers.EmployeeID).Replace("###<admin_fullname>###", App.MainViewModel.CurrentUsers.FullName);
                    }
                    else if (App.MainViewModel.CurrentUsers.UserId == App.MainViewModel.Users.UserId)
                    {
                        App.MainViewModel.Users = App.MainViewModel.CurrentUsers;
                        Username.Header = App.MainViewModel.CurrentUsers.StaffName;

                        //notificationTemplate = TemplateContext.GetTemplateByCode("US02");
                        notificationTemplate = TemplateContext.GetTemplateByCodeLang("US02", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                        notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("'", "''");
                    }
                    else
                    {
                        //notificationTemplate = TemplateContext.GetTemplateByCode("US01");
                        notificationTemplate = TemplateContext.GetTemplateByCodeLang("US01", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                        notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("'", "''").Replace("###<staff_id>###", App.MainViewModel.Users.EmployeeID).Replace("###<staff_fullname>###", App.MainViewModel.Users.FullName).Replace("###<admin_id>###", App.MainViewModel.CurrentUsers.EmployeeID).Replace("###<admin_fullname>###", App.MainViewModel.CurrentUsers.FullName);
                    }

                    if (App.MainViewModel.Users.EmailAddressChanged)
                    {
                        var user = await App.UserManager.FindByIdAsync(App.MainViewModel.Users.UserId);
                        await App.UserManager.SetEmailAsync(user, App.MainViewModel.Users.EmailAddress);
                    }

                    if (App.MainViewModel.Users.RoleChanged)
                    {
                        var user = await App.UserManager.FindByIdAsync(App.MainViewModel.Users.UserId);
                        var roleList = await App.UserManager.GetRolesAsync(user);
                        await App.UserManager.RemoveFromRolesAsync(user, roleList);
                        await App.UserManager.AddToRoleAsync(user, App.MainViewModel.Users.Role);
                    }

                    NotificationModel notification = new NotificationModel()
                    {
                        NotificationType = "Updates",
                        NotificationTitle = notificationTemplate.TemplateTitle,
                        NotificationContent = notificationTemplate.TemplateContent,
                        CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        CreatedBy = App.MainViewModel.CurrentUsers.FullName
                    };

                    if (NotificationContext.InsertNotification(notification))
                    {

                    }
                    else
                    {

                    }


                    PreviousPage(sender, e);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                App.log.Error("Update User Error >>> ", ex);
            }
            
        }

        private void AddDeviceHandler(EventArgs e, object sender)
        {
            if (DeviceRepository.InsertDevice(App.MainViewModel.DeviceModel, ConfigSettings.GetConfigurationSettings()))
            {
                ConfigurationModel? sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();
                String sNotificationContent = "";

                //var sTemplateObj = TemplateContext.GetTemplateByCode("DS01");
                var sTemplateObj = TemplateContext.GetTemplateByCodeLang("DS01", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                if (sTemplateObj != null)
                {
                    sNotificationContent = sTemplateObj.TemplateContent;
                }
                sNotificationContent = sNotificationContent.Replace("###<analyzer_name>###", App.MainViewModel.DeviceModel.DeviceName)
                                                           .Replace("###<admin_fullname>###", App.MainViewModel.CurrentUsers.FullName)
                                                           .Replace("###<admin_id>###", App.MainViewModel.CurrentUsers.EmployeeID);

                NotificationModel sNotificationSend = new NotificationModel()
                {
                    NotificationType = "Updates",
                    NotificationTitle = (sTemplateObj != null) ? sTemplateObj.TemplateTitle : "",
                    NotificationContent = sNotificationContent,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = App.MainViewModel.CurrentUsers.FullName
                };
                NotificationContext.InsertNotification(sNotificationSend);

                frameContent.Content = new DevicePage();
            }
            else
            {
                System.Windows.Controls.Primitives.Popup sErrorPopup = new System.Windows.Controls.Primitives.Popup();
                sErrorPopup.IsOpen = true;

                App.MainViewModel.Origin = "FailedAddDevice";
                App.PopupHandler(e, sender);
            }
        }

        private void DeleteDeviceHandler(EventArgs e, object sender)
        {
            if (DeviceRepository.UpdateDevice(App.MainViewModel.DeviceModel, ConfigSettings.GetConfigurationSettings()))
            {
                ConfigurationModel? sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();
                String sNotificationContent = "";

                //var sTemplateObj = TemplateContext.GetTemplateByCode("DS03");
                var sTemplateObj = TemplateContext.GetTemplateByCodeLang("DS03", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                if (sTemplateObj != null)
                {
                    sNotificationContent = sTemplateObj.TemplateContent;
                }
                sNotificationContent = sNotificationContent.Replace("###<analyzer_name>###", App.MainViewModel.DeviceModel.DeviceName)
                                                           .Replace("###<admin_fullname>###", App.MainViewModel.CurrentUsers.FullName)
                                                           .Replace("###<admin_id>###", App.MainViewModel.CurrentUsers.EmployeeID);

                NotificationModel sNotificationSend = new NotificationModel()
                {
                    NotificationType = "Updates",
                    NotificationTitle = (sTemplateObj != null) ? sTemplateObj.TemplateTitle : "",
                    NotificationContent = sNotificationContent,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = App.MainViewModel.CurrentUsers.FullName
                };
                NotificationContext.InsertNotification(sNotificationSend);

                frameContent.Content = new DevicePage();
            }
            else
            {
                System.Windows.Controls.Primitives.Popup sErrorPopup = new System.Windows.Controls.Primitives.Popup();
                sErrorPopup.IsOpen = true;

                App.MainViewModel.Origin = "FailedDeleteDevice";
                App.PopupHandler(e, sender);
            }
        }

        private void UpdateDeviceHandler(EventArgs e, object sender)
        {
            if (DeviceRepository.UpdateDevice(App.MainViewModel.DeviceModel, ConfigSettings.GetConfigurationSettings()))
            {
                ConfigurationModel? sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();
                String sNotificationContent = "";

                //var sTemplateObj = TemplateContext.GetTemplateByCode("DS02");
                var sTemplateObj = TemplateContext.GetTemplateByCodeLang("DS02", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                if (sTemplateObj != null)
                {
                    sNotificationContent = sTemplateObj.TemplateContent;
                }
                sNotificationContent = sNotificationContent.Replace("###<analyzer_name>###", App.MainViewModel.OldDeviceName)
                                                           .Replace("###<new_analyzer_name>###", App.MainViewModel.DeviceModel.DeviceName)
                                                           .Replace("###<admin_fullname>###", App.MainViewModel.CurrentUsers.FullName)
                                                           .Replace("###<admin_id>###", App.MainViewModel.CurrentUsers.EmployeeID);

                NotificationModel sNotificationSend = new NotificationModel()
                {
                    NotificationType = "Updates",
                    NotificationTitle = (sTemplateObj != null) ? sTemplateObj.TemplateTitle : "",
                    NotificationContent = sNotificationContent,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = App.MainViewModel.CurrentUsers.FullName
                };
                NotificationContext.InsertNotification(sNotificationSend);

                frameContent.Content = new DevicePage();
            }
            else
            {
                System.Windows.Controls.Primitives.Popup sErrorPopup = new System.Windows.Controls.Primitives.Popup();
                sErrorPopup.IsOpen = true;

                App.MainViewModel.Origin = "FailedUpdateDevice";
                App.PopupHandler(e, sender);
            }
        }

        private void UpdateSettingsHandler(EventArgs e, object sender)
        {
            try
            {
                var sSettingsObj = App.MainViewModel.ConfigurationModel;
                foreach (var s in sSettingsObj)
                {
                    var sConfigObj = ConfigurationContext.GetConfigurationData(s.ConfigurationKey).FirstOrDefault();
                    if (sConfigObj != null)
                    {
                        ConfigurationContext.UpdateConfiguration(s.ConfigurationKey, s.ConfigurationValue);
                    }
                    else
                    {
                        ConfigurationContext.AddConfiguration(s.ConfigurationKey, s.ConfigurationValue);
                    }
                }

                // --- Add Notification --//
                ConfigurationModel? sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();
                String sNotificationContent = "";

                //var sTemplateObj = TemplateContext.GetTemplateByCode("CS01");
                var sTemplateObj = TemplateContext.GetTemplateByCodeLang("CS01", sLangCode.ConfigurationValue);
                if (sTemplateObj != null)
                {
                    sNotificationContent = sTemplateObj.TemplateContent;
                }
                sNotificationContent = sNotificationContent.Replace("###<admin_fullname>###", App.MainViewModel.CurrentUsers.FullName)
                                                           .Replace("###<admin_id>###", App.MainViewModel.CurrentUsers.EmployeeID);

                NotificationModel sNotificationSend = new NotificationModel()
                {
                    NotificationType = "Updates",
                    NotificationTitle = (sTemplateObj != null) ? sTemplateObj.TemplateTitle : "",
                    NotificationContent = sNotificationContent,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = App.MainViewModel.CurrentUsers.FullName
                };
                NotificationContext.InsertNotification(sNotificationSend);

                System.Windows.Controls.Primitives.Popup sMessagePopup = new System.Windows.Controls.Primitives.Popup();
                sMessagePopup.IsOpen = true;

                App.MainViewModel.Origin = "SetingsUpdateCompleted";
                App.MainViewModel.ConfigurationModel = ConfigurationContext.GetConfigurationData("");

                App.PopupHandler(e, sender);
            }
            catch (Exception ex)
            {
                App.log.Error("Update Setting Error >>> ", ex);
                return;
            }
        }

        private static void ResetPasswordHandler(EventArgs e, object sender)
        {
            if (ResetCurrentPassword != null)
            {
                ResetCurrentPassword(sender, e);
            }
        }

        private void ChangeLanguageCountryHandler(EventArgs e, object sender)
        {
            try
            {
                System.Globalization.CultureInfo sZHCulture = new(App.MainViewModel.ConfigurationModel.FirstOrDefault(x => x.ConfigurationKey == "SystemSettings_Language").ConfigurationValueTemp);

                CultureResources.ChangeCulture(sZHCulture);

                bool updateCountrySuccess = ConfigurationContext.UpdateConfiguration("SystemSettings_Country", App.MainViewModel.ConfigurationModel.FirstOrDefault(x => x.ConfigurationKey == "SystemSettings_Country").ConfigurationValueTemp);
                bool updateLanguageSuccess = ConfigurationContext.UpdateConfiguration("SystemSettings_Language", App.MainViewModel.ConfigurationModel.FirstOrDefault(x => x.ConfigurationKey == "SystemSettings_Language").ConfigurationValueTemp);

                if (updateCountrySuccess && updateLanguageSuccess)
                {
                    var currentCountry = App.MainViewModel.ConfigurationModel.Where(x => x.ConfigurationKey == "SystemSettings_Country").FirstOrDefault();
                    var currentLanguage = App.MainViewModel.ConfigurationModel.Where(x => x.ConfigurationKey == "SystemSettings_Language").FirstOrDefault();

                    currentCountry.ConfigurationValue = currentCountry.ConfigurationValueTemp;
                    currentLanguage.ConfigurationValue = currentLanguage.ConfigurationValueTemp;

                    System.Windows.Data.Binding b = new System.Windows.Data.Binding("Setting_Title_PageTitle");
                    b.Source = System.Windows.Application.Current.TryFindResource("Resources");
                    PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

                    //var notificationTemplate = TemplateContext.GetTemplateByCode("LC01");
                    var notificationTemplate = TemplateContext.GetTemplateByCodeLang("LC01", sZHCulture.Name);
                    notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("'", "''");

                    NotificationModel notification = new NotificationModel()
                    {
                        NotificationType = "Updates",
                        NotificationTitle = notificationTemplate.TemplateTitle,
                        NotificationContent = notificationTemplate.TemplateContent,
                        CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        CreatedBy = App.MainViewModel.CurrentUsers.FullName
                    };

                    if (NotificationContext.InsertNotification(notification))
                    {

                    }
                    else
                    {

                    }

                    //PreviousPage(sender, e);
                }
            }
            catch(Exception ex)
            {
                App.log.Error("Change language Error >>> ", ex);
            }
        }

        private void mnDashboard_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Content = new DashboardPage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Dashboard_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            refreshMenuItemStyle(mnDashboard);
        }

        private void mnSchedule_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Content = new SchedulePage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Schedule_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            refreshMenuItemStyle(mnSchedule);
        }

        private void mnResults_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Content = new ResultPage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Results_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            refreshMenuItemStyle(mnResults);
        }

        private void mnNotifications_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Content = new NotificationPage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Notification_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            PageTitle.Text = Properties.Resources.Main_Label_SideMenuNotification;

            refreshMenuItemStyle(mnNotifications);
        }

        private void mnSettings_Click(object sender, RoutedEventArgs e)
        {
            if(App.MainViewModel.CurrentUsers.Role == "Lab User") { frameContent.Content = new LanguageCountryPage(); }
            else { frameContent.Content = new UserPage();}
            

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Setting_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            refreshMenuItemStyle(mnSettings);
        }

        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {
            menuslide("hidemenu", panel1);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            menuslide("showmenu", panel1);
        }

        private void menuslide(String p, StackPanel leftMenu)
        {
            Storyboard? sb = Resources[p] as Storyboard;
            sb.Begin(leftMenu);

            if (p.Contains("show"))
            {
                btnCollapse.Visibility = System.Windows.Visibility.Visible;

                btnOpen.Visibility = System.Windows.Visibility.Hidden;
                btnOpen.Visibility = System.Windows.Visibility.Hidden;
                thumbDashboard.Visibility = System.Windows.Visibility.Hidden;
                thumbSchedule.Visibility = System.Windows.Visibility.Hidden;
                thumbResult.Visibility = System.Windows.Visibility.Hidden;
                thumbNotification.Visibility = System.Windows.Visibility.Hidden;
                thumbSetting.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                if (p.Contains("hide"))
                {
                    btnCollapse.Visibility = System.Windows.Visibility.Hidden;

                    btnOpen.Visibility = System.Windows.Visibility.Visible;
                    thumbDashboard.Visibility = System.Windows.Visibility.Visible;
                    thumbSchedule.Visibility = System.Windows.Visibility.Visible;
                    thumbResult.Visibility = System.Windows.Visibility.Visible;
                    thumbNotification.Visibility = System.Windows.Visibility.Visible;
                    thumbSetting.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void ClearMenuItemStyle()
        {
            mnSettings.Background = System.Windows.Media.Brushes.Transparent;
            mnDashboard.Background = System.Windows.Media.Brushes.Transparent;
            mnSchedule.Background = System.Windows.Media.Brushes.Transparent;
            mnResults.Background = System.Windows.Media.Brushes.Transparent;
            mnNotifications.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void btnDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            var sButton = (System.Windows.Controls.Button)sender;
            ConfigurationContext.UpdateConfiguration("SystemSettings_Themes", (sButton != null) ? sButton.Tag.ToString() : "");

            CheckThemesSettings();
            refreshContent();
        }

        private void btnLightTheme_Click(object sender, RoutedEventArgs e)
        {
            var sButton = (System.Windows.Controls.Button)sender;
            ConfigurationContext.UpdateConfiguration("SystemSettings_Themes", (sButton != null) ? sButton.Tag.ToString() : "");
            
            CheckThemesSettings();
            refreshContent();
        }

        private void CheckThemesSettings()
        {
            String? sTheme = "Light";

            var sConfigData = ConfigurationContext.GetConfigurationData("SystemSettings_Themes");
            if (sConfigData != null && sConfigData.Count > 0)
            {
                sTheme = sConfigData[0].ConfigurationValue;
            }

            String sThemeName = sTheme + ".xaml";
            AppTheme.ChangeTheme(new Uri("Themes/" + sThemeName, UriKind.Relative));
            if (sTheme.ToLower() == "light")
            {
                btnDarkTheme.Background = System.Windows.Media.Brushes.Transparent;

                System.Windows.Media.Color sBlue = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e76fb");
                btnLightTheme.Background = new SolidColorBrush(sBlue);
            }
            else if (sTheme.ToLower() == "dark")
            {
                btnLightTheme.Background = System.Windows.Media.Brushes.Transparent;

                System.Windows.Media.Color sBlue = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e76fb");
                btnDarkTheme.Background = new SolidColorBrush(sBlue);
            }
        }

        private void refreshContent()
        {
            System.Windows.Controls.MenuItem sMenu = new System.Windows.Controls.MenuItem();

            var sCurrentContent = frameContent.Content;
            if (sCurrentContent.ToString().Contains("Dashboard")){ 
                frameContent.Content = new DashboardPage();
                sMenu = mnDashboard;
            }
            if (sCurrentContent.ToString().Contains("LanguageCountryPage")){ 
                frameContent.Content = new LanguageCountryPage();
                sMenu = mnSettings;
            }
            if (sCurrentContent.ToString().Contains("DevicePage")){ 
                frameContent.Content = new DevicePage();
                sMenu = mnSettings;
            }
            if (sCurrentContent.ToString().Contains("SchedulePage"))
            {
                sMenu = mnSchedule;
            }
            if (sCurrentContent.ToString().Contains("ResultPage"))
            {
                sMenu = mnResults;
            }
            if (sCurrentContent.ToString().Contains("User"))
            {
                sMenu = mnSettings;
            }
            if (sCurrentContent.ToString().Contains("Configuration"))
            {
                sMenu = mnSettings;
            }

            if (sCurrentContent.ToString().Contains("NotificationPage"))
            {
                sMenu = mnNotifications;

                var sSearchModel = App.MainViewModel.SearchModel;

                if (sSearchModel != null)
                {
                    var Notification = new NotificationPage();
                    Notification.reloadData(sSearchModel.SearchStart, sSearchModel.SearchEnd, 
                                            (sSearchModel.SearchType == "All" ? null : sSearchModel.SearchType), 
                                            sSearchModel.SearchStartDate, sSearchModel.SearchEndDate, sSearchModel.SearchKeyword, 
                                            sSearchModel.SearchReset);

                    // Pre-selected search criteria //
                    Notification.KeywordSearchBar.Text = sSearchModel.SearchKeyword;

                    foreach(var t in Notification.NotificationType.Items)
                    {
                        var comboItem = t as ComboBoxItem;

                        if (comboItem.Content.ToString() == sSearchModel.SearchType)
                        {
                            comboItem.IsSelected = true;
                        }
                        else
                        {
                            comboItem.IsSelected = false;
                        }
                    }

                    if (sSearchModel.SearchStartDate != null && sSearchModel.SearchEndDate != null)
                    {
                        Notification.RangeDate.SelectedDates = GenerateSelectedDateRange(sSearchModel.SearchStartDate, sSearchModel.SearchEndDate);

                        String sStart = Notification.RangeDate.SelectedDates.FirstOrDefault().ToString();
                        String sEnd = Notification.RangeDate.SelectedDates.LastOrDefault().ToString();
                        DateTime dtStart = DateTime.ParseExact(sStart, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dtEnd = DateTime.ParseExact(sEnd, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                        Notification.RangeDate.DateRangePicker_TextBox.Text = dtStart.ToString("dd/MM/yyyy") + " - " + dtEnd.ToString("dd/MM/yyyy");
                    }


                    frameContent.Content = Notification;
                }
                else
                {
                    frameContent.Content = new NotificationPage();
                }

            }

            refreshMenuItemStyle(sMenu);
        }

        private void refreshMenuItemStyle(System.Windows.Controls.MenuItem sItem)
        {
            ClearMenuItemStyle();

            String? sColor = System.Windows.Application.Current.Resources["Themes_MenuHighligted"].ToString();

            sItem.Background = new BrushConverter().ConvertFrom(sColor) as SolidColorBrush;
            sItem.BorderBrush = new BrushConverter().ConvertFrom("#404D5B") as SolidColorBrush;
        }

        private ObservableCollection<DateTime> GenerateSelectedDateRange(String sStart, String sEnd)
        {
            ObservableCollection<DateTime> sDateList = new ObservableCollection<DateTime>();

            if (sStart == null && sEnd == null)
            {
                return null;
            }
            else if (sStart == sEnd)
            {
                DateTime sDate = DateTime.ParseExact(sStart, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                sDateList.Add(sDate);
            }
            else
            {
                DateTime sSDate = DateTime.ParseExact(sStart, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime sEDate = DateTime.ParseExact(sEnd, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                
                sDateList.Add(sSDate);

                while (sSDate < sEDate.AddDays(-1))
                {
                    sSDate = sSDate.AddDays(1);

                    sDateList.Add(sSDate);
                }
            }

            return sDateList;
        }
    }

}
