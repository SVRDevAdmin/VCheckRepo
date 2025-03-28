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
using VCheckViewer.Views.Pages.Setting.Report;
using DocumentFormat.OpenXml.EMMA;
using System.Windows.Diagnostics;
using CheckBox = System.Windows.Controls.CheckBox;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.IO;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

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
        public ObservableCollection<ComboBoxItem> deviceComboList = new ObservableCollection<ComboBoxItem>();
        public ObservableCollection<CheckedListItem> ParameterList = new ObservableCollection<CheckedListItem>();

        public class CheckedListItem
        {
            public string Name { get; set; }
            public bool isChecked { get; set; }
        }

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
            App.GoToViewResultPage += new EventHandler(GoToViewReportPage);

            App.GoToSettingUserPage += new EventHandler(SettingUserPage);
            App.GoToSettingLanguageCountryPage += new EventHandler(GoToLanguageCountryPage);
            App.GoToSettingDevicePage += new EventHandler(GoToDevicePage);
            App.GoToSettingConfigurationPage += new EventHandler(GoToConfigurationPage);
            App.GoToReportPage += new EventHandler(GoToReportPage);

            //popup
            App.Popup += new EventHandler(Popup);
            App.GoPreviousPage += new EventHandler(PreviousPage);

            this.SizeChanged += MainWindow_SizeChanged;

            initializedDropdownSelectionList();

            Username.Header = App.MainViewModel.CurrentUsers.StaffName;

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Dashboard_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            CheckThemesSettings();

            refreshMenuItemStyle(mnDashboard);
            //mnDashboard.Background = System.Windows.Media.Brushes.White;
            //mnDashboard.BorderBrush = new BrushConverter().ConvertFrom("#404D5B") as SolidColorBrush;

            var sBuilder = new ConfigurationBuilder();
            sBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            App.iConfig = sBuilder.Build();
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
            checklanguage();

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
            checklanguage();

            frameContent.Content = new ViewUserPage();
        }

        void GoToLanguageCountryPage(object sender, EventArgs e)
        {
            frameContent.Content = new LanguageCountryPage();
        }
        void GoToDevicePage(object sender, EventArgs e)
        {
            checklanguage();

            frameContent.Content = new DevicePage();
        }

        void GoToConfigurationPage(object sender, EventArgs e)
        {
            checklanguage();

            frameContent.Content = new ConfigurationPage();
        }

        void GoToReportPage(object sender, EventArgs e)
        {
            checklanguage();

            frameContent.Content = new ReportPage();
        }

        void GoToViewReportPage(object sender, EventArgs e)
        {
            frameContent.Content = new ViewResultPage();
        }

        void SettingUserPage(object sender, EventArgs e)
        {
            checklanguage();

            frameContent.Content = new UserPage();
        }

        public void UpdatePatientName(object sender, EventArgs e)
        {
            ResultPage resultPage = frameContent.Content as ResultPage;

            resultPage.LoadResultDataGrid();
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
            if (App.MainViewModel.Origin == "SettingsUpdate" || App.MainViewModel.Origin == "ReportSettingsUpdate") { PopupContent.Text = Properties.Resources.Popup_Message_SaveSettings; }
            if (App.MainViewModel.Origin == "CancelSchedule") { PopupContent.Text = Properties.Resources.Popup_Message_CancelSchedule; }
            if (App.MainViewModel.Origin == "SetingsUpdateCompleted") {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_SavePMSLISHIS; 
            }
            if (App.MainViewModel.Origin == "ListingDownloadCompleted") {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Results_Message_DownloadComplete; 
            }
            if (App.MainViewModel.Origin == "TestResultDownloadCompleted")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();

                PopupContent.Text = Properties.Resources.Results_Message_TestResultDownloadCompleted + "\r\n "+ Properties.Resources.Popup_Message_TheFileIsAt + " \r\n" + sBuilder.Configuration.GetSection("Configuration:DownloadFolderPath").Value;
            }
            if (App.MainViewModel.Origin == "FailedDownloadListing")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedDownloadListing;
            }
            if (App.MainViewModel.Origin == "FailedToShowPrint")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedOpenPrint;
            }
            if (App.MainViewModel.Origin == "FailedAddDevice")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedAddDevice;
            }
            if (App.MainViewModel.Origin == "FailedDeleteDevice")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedDeleteDevice;
            }
            if (App.MainViewModel.Origin == "FailedUpdateDevice")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedUpdateDevice;
            }
            if (App.MainViewModel.Origin == "GreywindSendUniqueID")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
                txtInput.Visibility = Visibility.Visible;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;
                txtInput.Text = "";

                PopupContent.Text = Properties.Resources.Popup_Message_EnterUniqueID;
            }
            if (App.MainViewModel.Origin == "SendToAnalyzer")
            {
                List<DeviceModel> devices = DeviceRepository.GetTwoWayCommDevice(ConfigSettings.GetConfigurationSettings());

                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                deviceComboList.Clear();

                if (devices.Count() > 0)
                {

                    if (devices != null)
                    {
                        foreach (var d in devices)
                        {
                            deviceComboList.Add(new ComboBoxItem
                            {
                                Content = d.DeviceName,
                                Tag = d.id
                            });
                        }
                    }

                    if (deviceComboList.Count > 0)
                    {
                        deviceList.ItemsSource = deviceComboList;
                    }

                    PopupContent.Text = Properties.Resources.Popup_Message_SelectDevice;
                    deviceList.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                }
                else
                {
                    App.MainViewModel.Origin = "NoDeviceFound";
                    PopupContent.Text = Properties.Resources.Popup_Message_NoDeviceFound;
                }

            }
            if (App.MainViewModel.Origin == "SentToAnalyzer")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_SentToAnalyzer;
            }
            if (App.MainViewModel.Origin == "FailedToSendToAnalyzer")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedToSendToAnalyzer;
            }
            if (App.MainViewModel.Origin == "CanceledSendToAnalyzer")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_CanceledSendToAnalyzer;
            }
            if (App.MainViewModel.Origin == "UpdatePatientName")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
                txtInput.Visibility = Visibility.Visible;
                txtInput.Text = App.TestResultInfo.PatientName;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_UpdatePatientName;
            }
            if (App.MainViewModel.Origin == "PatientNameUpdated")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_PatientNameUpdated;
            }
            if (App.MainViewModel.Origin == "ReportSetingsUpdateCompleted")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_ReportSettingUpdated;
            }
            if (App.MainViewModel.Origin == "FailedToDownload")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedToDownload;
            }
            if (App.MainViewModel.Origin == "FailedToPrint")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedToPrint;
            }
            if (App.MainViewModel.Origin == "SelectParameters")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Visible;

                parameterList.Items.Clear();

                int currentParameter = 0;
                int remainder = 0;
                bool excess = false;
                int totalRow = Math.DivRem(App.Parameters.Count, 5, out remainder);
                if (remainder > 0)
                {
                    totalRow += 1;
                    excess = true;
                }

                for (int i = 0; i < totalRow; i++)
                {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                    stackPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    int elementPerRow = (i == (totalRow - 1) && excess) ? remainder : 5;

                    for (int j = 0; j < elementPerRow; j++)
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.Foreground = Brushes.White;
                        checkBox.BorderBrush = Brushes.White;
                        checkBox.Content = App.Parameters[currentParameter];
                        stackPanel.Children.Add(checkBox);
                        currentParameter++;
                    }

                    parameterList.Items.Add(stackPanel);
                }

                if (App.IsPrint)
                {
                    PopupContent.Text = Properties.Resources.Popup_Message_SelectParameterPrint;
                }
                else
                {
                    PopupContent.Text = Properties.Resources.Popup_Message_SelectParameterDownload;
                }
            }
            if (App.MainViewModel.Origin == "SelectAdditionalTestResult")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Visible;

                App.Device.AddRange(TestResultsRepository.GetRelatedTestResultByPatientID(ConfigSettings.GetConfigurationSettings(), App.DowloadPrintObject[0].TestResult.PatientID, App.Device[0].DeviceName));                

                if(App.Device.Count > 1)
                {
                    parameterList.Items.Clear();

                    int currentParameter = 1;
                    int remainder = 0;
                    bool excess = false;
                    int totalRow = Math.DivRem(App.Device.Count - 1, 5, out remainder);
                    if (remainder > 0)
                    {
                        totalRow += 1;
                        excess = true;
                    }

                    for (int i = 0; i < totalRow; i++)
                    {
                        StackPanel stackPanel = new StackPanel();
                        stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                        stackPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        int elementPerRow = (i == (totalRow - 1) && excess) ? remainder : 5;

                        for (int j = 0; j < elementPerRow; j++)
                        {
                            CheckBox checkBox = new CheckBox();
                            checkBox.Foreground = Brushes.White;
                            checkBox.BorderBrush = Brushes.White;
                            checkBox.Content = App.Device[currentParameter].DeviceName;
                            stackPanel.Children.Add(checkBox);
                            currentParameter++;
                        }

                        parameterList.Items.Add(stackPanel);
                    }

                    PopupContent.Text = Properties.Resources.Popup_Message_SelectAdditionalTestResult;
                }
                else
                {
                    App.MainViewModel.Origin = "SelectParameters";
                    App.PopupHandler(e, sender);
                }
            }
            if (App.MainViewModel.Origin == "FailedUpdateLIS")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_FailedUpdateLISInfo;
            }
            if (App.MainViewModel.Origin == "IpPortUnavailable")
            {
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                PopupContent.Text = Properties.Resources.Popup_Message_IpPortUnavailable;
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
            if (App.MainViewModel.Origin == "SettingsUpdate" || App.MainViewModel.Origin == "ReportSettingsUpdate") { UpdateSettingsHandler(e, sender); }
            if (App.MainViewModel.Origin == "CancelSchedule") { App.CancelScheduleHandler(e, sender); }
            if (App.MainViewModel.Origin == "Logout")
            {
                checklanguage();

                //if (!System.Windows.Application.Current.Windows.OfType<LoginWindow>().Any())
                //{
                //    LoginWindow login = new LoginWindow();
                //    login.LoginFrame.Content = new LoginPage();
                //    login.Show();
                //}

                foreach (var window in System.Windows.Application.Current.Windows.OfType<LoginWindow>())
                {
                    window.Close();
                }

                LoginWindow login = new LoginWindow();
                login.LoginFrame.Content = new LoginPage();
                login.Show();
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

            btnOk.Visibility = Visibility.Collapsed;
            btnYes.Visibility = Visibility.Visible;
            btnNo.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Collapsed;
            txtInput.Visibility = Visibility.Collapsed;
            deviceList.Visibility = Visibility.Collapsed;
            ParameterView.Visibility = Visibility.Collapsed;
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
                App.MainViewModel.Origin == "FailedUpdateDevice" ||
                App.MainViewModel.Origin == "SentToAnalyzer" ||
                App.MainViewModel.Origin == "FailedToSendToAnalyzer" ||
                App.MainViewModel.Origin == "CanceledSendToAnalyzer" ||
                App.MainViewModel.Origin == "NoDeviceFound" ||
                App.MainViewModel.Origin == "PatientNameUpdated" ||
                App.MainViewModel.Origin == "ReportSetingsUpdateCompleted" ||
                App.MainViewModel.Origin == "FailedToDownload" ||
                App.MainViewModel.Origin == "FailedToPrint" ||
                App.MainViewModel.Origin == "FailedUpdateLIS" ||
                App.MainViewModel.Origin == "IpPortUnavailable")
            {
                CancelButton_Click(null, null);
            }

            else if (App.MainViewModel.Origin == "GreywindSendUniqueID")
            {
                this.IsEnabled = true;
                popup.IsOpen = false;

                PopupBackground.Background = null;

                App.MainViewModel.ScheduleUniqueID = txtInput.Text;

                btnOk.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Visible;
                btnNo.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                SendToPMSHandler(e, sender); 
            }

            else if (App.MainViewModel.Origin == "SendToAnalyzer")
            {
                this.IsEnabled = true;
                popup.IsOpen = false;

                PopupBackground.Background = null;

                var sSelectedItem = ((ComboBoxItem)deviceList.SelectedItem);
                App.AnalyzerID = Convert.ToInt32(sSelectedItem.Tag);

                SendRequestToAnalyzer(e, sender);
            }

            else if (App.MainViewModel.Origin == "UpdatePatientName")
            {
                this.IsEnabled = true;
                popup.IsOpen = false;

                PopupBackground.Background = null;

                App.TestResultInfo.PatientName = txtInput.Text;

                btnOk.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Visible;
                btnNo.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                UpdatePatientName(e, sender);
            }

            else if (App.MainViewModel.Origin == "SelectParameters")
            {
                this.IsEnabled = true;
                popup.IsOpen = false;

                PopupBackground.Background = null;

                btnOk.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Visible;
                btnNo.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Collapsed;
                deviceList.Visibility = Visibility.Collapsed;
                ParameterView.Visibility = Visibility.Collapsed;

                GetSelectedParameters(e, sender);
            }

            else if (App.MainViewModel.Origin == "SelectAdditionalTestResult")
            {
                GetTestResult(e, sender);
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

                                List<string> sRecipientList = [App.MainViewModel.Users.EmailAddress];


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

                                if (!String.IsNullOrEmpty(sErrorMessage)) { throw new Exception(sErrorMessage); }

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
            catch (Exception ex)
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

                if(App.MainViewModel.Origin == "ReportSettingsUpdate")
                {
                    App.MainViewModel.Origin = "ReportSetingsUpdateCompleted";
                }
                else
                {
                    App.MainViewModel.Origin = "SetingsUpdateCompleted";
                }

                App.MainViewModel.ConfigurationModel = ConfigurationContext.GetConfigurationData("");

                App.PopupHandler(e, sender);

                if (App.RestartListener)
                {
                    RunListener();
                }
            }
            catch (Exception ex)
            {
                App.log.Error("Update Setting Error >>> ", ex);
                return;
            }
        }

        private async void SendRequestToAnalyzer(EventArgs e, object sender)
        {
            VCheckViewerAPI.HL7MessageSender.Main sendMessage = new VCheckViewerAPI.HL7MessageSender.Main();
            await sendMessage.SendMessage(App.ScheduleTestInfo);

            App.PopupHandler(e, sender);
        }

        private void UpdatePatientName(EventArgs e, object sender)
        {
            TestResultsRepository.UpdateTestResult(ConfigSettings.GetConfigurationSettings(), App.TestResultInfo);
            App.UpdatePatientNameHandler(e, sender);

            if (App.isEmptyName)
            {
                App.DowloadPrintObject[0].TestResult.PatientName = App.TestResultInfo.PatientName;

                App.MainViewModel.Origin = "SelectAdditionalTestResult";
                App.PopupHandler(e, sender);
            }
            else
            {
                App.MainViewModel.Origin = "PatientNameUpdated";
                App.PopupHandler(e, sender);
            }

        }

        private void GetTestResult(EventArgs e, object sender)
        {
            var stackPanels = parameterList.Items.OfType<StackPanel>();
            List<CheckBox> selectedTest = new List<CheckBox>();

            foreach (var stackpanel in stackPanels)
            {
                selectedTest.AddRange(stackpanel.Children.OfType<CheckBox>().Where(x => x.IsChecked == true));
            }

            foreach(var test in selectedTest)
            {
                var TestID = App.Device.FirstOrDefault(x => x.DeviceName == test.Content).TestID;
                var TestResult = TestResultsRepository.GetTestResultByID(ConfigSettings.GetConfigurationSettings(), TestID);

                DownloadPrintResultModel downloadPrintModelTemp = new DownloadPrintResultModel();
                TestResultModel previousTest = new TestResultModel();
                downloadPrintModelTemp.TestResult = TestResult;
                downloadPrintModelTemp.TestResultDetails = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), TestID);
                downloadPrintModelTemp.PreviousTestResultDetails = TestResultsRepository.GetPreviousTestRecord(ConfigSettings.GetConfigurationSettings(), TestResult, out previousTest);
                downloadPrintModelTemp.PreviousTestResult = previousTest;

                App.DowloadPrintObject.Add(downloadPrintModelTemp);
                App.Parameters.AddRange(downloadPrintModelTemp.TestResultDetails.Select(x => x.TestParameter).Where(y => !App.Parameters.Contains(y)));
            }

            var parameterOrder = TestResultsRepository.GetAllParameters(ConfigSettings.GetConfigurationSettings()).Select(x => x.Parameter).ToList();
            App.Parameters = App.Parameters.OrderBy(d => parameterOrder.IndexOf(d)).ToList();

            App.MainViewModel.Origin = "SelectParameters";
            App.PopupHandler(e, sender);
        }

        private void GetSelectedParameters(EventArgs e, object sender)
        {
            var stackPanels = parameterList.Items.OfType<StackPanel>();
            List<CheckBox> selectedParameters = new List<CheckBox>();

            foreach (var stackpanel in stackPanels)
            {
                selectedParameters.AddRange(stackpanel.Children.OfType<CheckBox>().Where(x => x.IsChecked == true));
            }

            App.Parameters = selectedParameters.Select(x => x.Content.ToString()).ToList();

            App.DownloadPrintReportHandler(e, sender);
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

        // ------------- Temporary ------------------- //
        private void SendToPMSHandler(EventArgs e, object sender)
        {
            VCheck.Interface.API.Greywind.RequestMessage.UpdateResultRequest sRequestAPI = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultRequest();
            long iTestResultID = 0;
            //App.MainViewModel.TestResultID
            //var sMenu = sender as System.Windows.Controls.MenuItem;
            //if (!String.IsNullOrEmpty(sMenu.Tag.ToString()))
            String sUniqueID = App.MainViewModel.ScheduleUniqueID;

            if (!String.IsNullOrEmpty(App.MainViewModel.TestResultID))
            {
                //iTestResultID = Convert.ToInt64(sMenu.Tag);
                iTestResultID = Convert.ToInt64(App.MainViewModel.TestResultID);

                var sTestResultObj = TestResultsRepository.GetTestResultByID(ConfigSettings.GetConfigurationSettings(), iTestResultID);
                if (sTestResultObj != null)
                {
                    List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject> sResultListing = new List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject>();
                    List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject> sPanelListing = new List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject>();

                    String sOrderID = "";
                    //var sScheduledTestObj = ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), "TBLAM FIA-8");
                    var sScheduledTestObj = ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), sUniqueID, sTestResultObj.PatientID);
                    if (sScheduledTestObj != null)
                    {
                        if (sScheduledTestObj.ScheduleUniqueID.Contains("-"))
                        {
                            var UniqueIDSplit = sScheduledTestObj.ScheduleUniqueID.Split("-");
                            if (UniqueIDSplit.Length > 0)
                            {
                                sOrderID = UniqueIDSplit[1];
                                sRequestAPI.accessionnumber = UniqueIDSplit[2];
                            }
                        }

                        //sRequestAPI.accessionnumber = iTestResultID.ToString();
                        sRequestAPI.clinic_id = "";
                        sRequestAPI.reportdate = sTestResultObj.CreatedDate.Value.ToString("yyyy-MM-dd");
                        sRequestAPI.providerid = "";

                        VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPatientObject sPatientObj = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPatientObject();
                        sPatientObj.patientid = sTestResultObj.PatientID;
                        sPatientObj.firstname = (sScheduledTestObj != null) ? sScheduledTestObj.PatientName : "";
                        sPatientObj.lastname = "";
                        sPatientObj.gender = (sScheduledTestObj != null) ? sScheduledTestObj.Gender : "";
                        sPatientObj.birthday = "2023-01-01";
                        sPatientObj.species = (sScheduledTestObj != null) ? sScheduledTestObj.Species : "";
                        sPatientObj.breed = "";

                        sRequestAPI.patient = sPatientObj;

                        VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject sPanelObj = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject();
                        sPanelObj.code = sTestResultObj.TestResultType;
                        sPanelObj.name = sTestResultObj.TestResultType;
                        sPanelObj.status = "F";
                        sPanelObj.source = "";
                        sPanelObj.resultdate = sTestResultObj.CreatedDate.Value.ToString("yyyy-MM-dd");


                        var sDetailsObj = TestResultsRepository.GetResultDetailsByTestResultID(ConfigSettings.GetConfigurationSettings(), iTestResultID);
                        if (sDetailsObj != null && sDetailsObj.Count > 0)
                        {
                            foreach (var d in sDetailsObj)
                            {
                                String[] sRange = Array.Empty<string>();
                                if (d.ReferenceRange != null)
                                {
                                    String sReferenceRange = d.ReferenceRange.Replace("[", "").Replace("]", "");
                                    if (sReferenceRange != "")
                                    {
                                        if (sReferenceRange.Contains(";"))
                                        {
                                            sRange = sReferenceRange.Split(";");
                                        }
                                        else if (sReferenceRange.Contains("-"))
                                        {
                                            sRange = sReferenceRange.Split("-");
                                        }
                                    }
                                }

                                sResultListing.Add(new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject
                                {
                                    name = d.TestParameter,
                                    code = d.TestParameter,
                                    result = d.TestResultValue,
                                    referencelow = (sRange.Length > 0) ? sRange[0] : "",
                                    referencehigh = (sRange.Length > 0) ? sRange[1] : "",
                                    unitofmeasure = d.TestResultUnit,
                                    status = "F",
                                    notes = ""
                                });
                            }

                            sPanelObj.tests = sResultListing;
                        }
                        sPanelListing.Add(sPanelObj);
                        //sPanelListing.Add(sPanelObj);

                        sRequestAPI.panels = sPanelListing;

                        VCheck.Interface.API.GreywindAPI sAPI = new VCheck.Interface.API.GreywindAPI();
                        var sRespAPI = sAPI.UpdateResult(sRequestAPI, sOrderID);
                        if (sRespAPI)
                        {
                            System.Windows.Forms.MessageBox.Show(Properties.Resources.Popup_Message_APIUpdateSuccess);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show(Properties.Resources.Popup_Message_APIUpdateFailed);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(Properties.Resources.Popup_Message_WrongID);
                    }

                    
                }
            }
        }

        private void mnDashboard_Click(object sender, RoutedEventArgs e)
        {
            checklanguage();

            frameContent.Content = new DashboardPage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Dashboard_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            refreshMenuItemStyle(mnDashboard);
        }

        private void mnSchedule_Click(object sender, RoutedEventArgs e)
        {
            checklanguage();

            frameContent.Content = new SchedulePage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Schedule_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            refreshMenuItemStyle(mnSchedule);
        }

        private void mnResults_Click(object sender, RoutedEventArgs e)
        {
            checklanguage();

            frameContent.Content = new ResultPage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Results_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            refreshMenuItemStyle(mnResults);
        }

        private void mnNotifications_Click(object sender, RoutedEventArgs e)
        {
            checklanguage();

            frameContent.Content = new NotificationPage();

            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Notification_Title_PageTitle");
            b.Source = System.Windows.Application.Current.TryFindResource("Resources");
            PageTitle.SetBinding(System.Windows.Controls.TextBlock.TextProperty, b);

            PageTitle.Text = Properties.Resources.Main_Label_SideMenuNotification;

            refreshMenuItemStyle(mnNotifications);
        }

        private void mnSettings_Click(object sender, RoutedEventArgs e)
        {
            checklanguage();

            if (App.MainViewModel.CurrentUsers.Role == "Lab User") { frameContent.Content = new LanguageCountryPage(); }
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

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowHeight = e.NewSize.Height;

            GridElement.Height = windowHeight * 0.04;
            BorderElement.Height = (windowHeight * 0.04) + 5;
            CollapseGrid.Height = windowHeight * 0.04;
            DashboardGrid.Height = windowHeight * 0.08;
            ScheduleGrid.Height = windowHeight * 0.08;
            ResultGrid.Height = windowHeight * 0.08;
            NotificationGrid.Height = windowHeight * 0.08;
            SettingsGrid.Height = windowHeight * 0.08;
            BackgroundElement.Height = windowHeight;
        }

        private void checklanguage()
        {
            if (App.isLanguagePage)
            {
                System.Globalization.CultureInfo sZHCulture = new(App.MainViewModel.ConfigurationModel.FirstOrDefault(x => x.ConfigurationKey == "SystemSettings_Language").ConfigurationValue);

                CultureResources.ChangeCulture(sZHCulture);

                App.isLanguagePage = false;
            }

        }

        private void RunListener()
        {
            var sBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            String sListenerPath = sBuilder.Configuration.GetSection("Configuration:ListenerPath").Value;

            foreach (var processKill in Process.GetProcessesByName("VCheckListenerWorker"))
            {
                processKill.Kill();
            }

            string exePath = sListenerPath;

            Process process = new Process();
            process.StartInfo.FileName = exePath;

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {

            }

            //OverwriteSetting("ConnectionStrings", "");
        }

        private void OverwriteSetting(string key, string value)
        {
            Dictionary<string, string> newValue = new Dictionary<string, string>();
            newValue.Add("DefaultConnection", "Server=localhost;Database=vcheckdb;User=root;Password=password;POOLING=FALSE;");

            var configJson = File.ReadAllText("appsettings.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);
            config[key] = newValue;
            var updatedConfigJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("appsettings.json", updatedConfigJson);
        }
    }

}
