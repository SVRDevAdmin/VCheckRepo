﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.IO;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using System.Reflection;
using Wpf.Ui;
using VCheckViewer.Views.Windows;
using VCheckViewer.ViewModels.Windows;
using VCheckViewer.Services;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Internal;
using VCheck.Lib.Data.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using VCheckViewer.Lib.Models;
using VCheckViewer.Lib.Culture;
//using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Globalization;
using System.Text.RegularExpressions;
using VCheck.Lib.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Markup;
using VCheck.Helper;
using VCheckViewer.Views.Pages.Schedule;
using VCheckViewer.Views.Pages.Results;

namespace VCheckViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public IConfiguration Configuration { get; }

        public static MainViewModel MainViewModel { get; } = new MainViewModel();

        public static event EventHandler GoPreviousPage;
        public static event EventHandler Popup;

        public static event EventHandler GoToSettingUserPage;
        public static event EventHandler GoToSettingLanguageCountryPage;
        public static event EventHandler GoToSettingDevicePage;
        public static event EventHandler GoToSettingConfigurationPage;
        public static event EventHandler GoToReportPage;

        public static event EventHandler? GoToViewResultPage;

        public static event EventHandler DownloadPrintReport;
        public static event EventHandler UpdatePatientName;

        public static event EventHandler? CancelSchedule;

        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
        public static RoleManager<IdentityRole> RoleManager { get; set; }
        public static IUserStore<IdentityUser> UserStore { get; set; }

        public static string newPassword { get; set; }
        public static SMTPModel SMTP { get; set; }
        public static string UpdateLink {  get; set; }
        public static long TestResultID { get; set; }
        public static ScheduledTestModel ScheduleTestInfo { set; get; }
        public static int AnalyzerID { set; get; }
        public static List<string> Parameters { get; set; }
        public static List<TestDeviceName> Device { get; set; }
        public static TestResultModel TestResultInfo { get; set; }
        public static string FilePath { get; set; }
        public static bool ErrorOccur { get; set; }
        public static bool IsPrint { get; set; }
        public static string PMSFunction { get; set; }
        public static bool isLanguagePage { get; set; }
        public static bool isEmptyName { get; set; }
        public static TestResultListingExtendedObj sTestResultObj { get; set; }
        public static List<DownloadPrintResultModel> DowloadPrintObject { get; set; }
        public static bool ResultPageNotInitialized { get; set; } = true;
        public static bool SchedulePageNotInitialized { get; set; } = true;
        public static bool LoginWindowNotInitialized { get; set; } = true;
        public static bool RestartListener { get; set; }

        public static IConfiguration iConfig { get; set; }



        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging

        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => 
            {
                try
                {
                    c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
                }
                catch (Exception ex)
                {
                    log.Error("App Configuration Error >>> ", ex);
                }
            })
            .ConfigureServices((context, services) =>
            {
                try
                {
                    services.AddHostedService<ApplicationHostService>();

                    services.AddSingleton<IPageService, PageServices>();
                    services.AddSingleton<IThemeService, ThemeService>();
                    services.AddSingleton<ITaskBarService, TaskBarService>();
                    services.AddSingleton<INavigationService, NavigationService>();


                    services.Add(new ServiceDescriptor(typeof(UserDBContext), new UserDBContext(context.Configuration)));
                    services.Add(new ServiceDescriptor(typeof(MasterCodeDataDBContext), new MasterCodeDataDBContext(context.Configuration)));
                    services.Add(new ServiceDescriptor(typeof(RolesDBContext), new RolesDBContext(context.Configuration)));
                    services.Add(new ServiceDescriptor(typeof(CountryDBContext), new CountryDBContext(context.Configuration)));
                    services.Add(new ServiceDescriptor(typeof(ConfigurationDBContext), new ConfigurationDBContext(context.Configuration)));
                    services.Add(new ServiceDescriptor(typeof(TemplateDBContext), new TemplateDBContext(context.Configuration)));
                    services.Add(new ServiceDescriptor(typeof(NotificationDBContext), new NotificationDBContext(context.Configuration)));
                    services.Add(new ServiceDescriptor(typeof(DeviceDBContext), new DeviceDBContext(context.Configuration)));

                    services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(context.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21))));

                    var smtpSetting = context.Configuration.GetSection("SMTP");

                    SMTP = new SMTPModel()
                    {
                        Host = smtpSetting.GetValue<string>("Host"),
                        Port = smtpSetting.GetValue<int>("Port"),
                        Username = smtpSetting.GetValue<string>("Username"),
                        Password = smtpSetting.GetValue<string>("Password"),
                        Sender = smtpSetting.GetValue<string>("Sender")
                    };

                    var identitySettings = context.Configuration.GetSection("IdentitySettings");

                    services.AddIdentityCore<IdentityUser>(options =>
                    {
                        // Password settings.
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 8;

                        //Lockedout setting
                        options.SignIn.RequireConfirmedAccount = identitySettings.GetValue<bool>("RequireConfirmedAccount");
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.GetValue<int>("DefaultLockedOutTimeSpan"));
                        options.Lockout.MaxFailedAccessAttempts = identitySettings.GetValue<int>("MaxFailedAccessAttempts");
                    })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

                    services.AddScoped<SignInManager<IdentityUser>>();
                    services.AddHttpContextAccessor();
                    services.AddAuthentication();

                    UpdateLink = context.Configuration.GetValue<string>("UpdateLink");
                }
                catch(Exception ex)
                {
                    log.Error("Service Configuration Error >>> ", ex);
                }
                
            })
            .Build();


        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                ConfigurationDBContext ConfigurationContext = GetService<ConfigurationDBContext>();
                UserDBContext usersContext = GetService<UserDBContext>();
                RolesDBContext roleContext = GetService<RolesDBContext>();

                SignInManager = GetService<SignInManager<IdentityUser>>();
                UserManager = GetService<UserManager<IdentityUser>>();
                RoleManager = GetService<RoleManager<IdentityRole>>();
                UserStore = GetService<IUserStore<IdentityUser>>();

                MainViewModel.ConfigurationModel = ConfigurationContext.GetConfigurationData("");

                var language = MainViewModel.ConfigurationModel.Where(x => x.ConfigurationKey == "SystemSettings_Language").FirstOrDefault()?.ConfigurationValue;

                CultureInfo sZHCulture = new CultureInfo(language != null ? language : "en");
                CultureResources.ChangeCulture(sZHCulture);

                //var roles = RoleManager.Roles.ToList();
                //IdentityRole role = new IdentityRole();
                //bool addRoleSuccess;
            
                //if (!roles.Where(x => x.Name == "Lab User").Any())
                //{
                //    role = new IdentityRole("Lab User");
                //    await RoleManager.CreateAsync(role);
                //    addRoleSuccess = roleContext.InsertRole(new RolesModel() { RoleID = role.Id, RoleName = "Lab User", IsActive = true, IsSuperadmin = false, IsAdmin = false });
                //    if (addRoleSuccess) { }
                //    else { }
                //}

                //var roles = RoleManager.Roles.ToList();
                //IdentityRole role = new IdentityRole();
                //bool addRoleSuccess;

                //if (!roles.Where(x => x.Name == "Lab User").Any())
                //{
                //    role = new IdentityRole("Lab User");
                //    await RoleManager.CreateAsync(role);
                //    addRoleSuccess = roleContext.InsertRole(new RolesModel() { RoleID = role.Id, RoleName = "Lab User", IsActive = true, IsSuperadmin = false, IsAdmin = false });
                //    if (addRoleSuccess) { }
                //    else { }
                //}

                //if (!roles.Where(x => x.Name == "Lab Superadmin").Any())
                //{
                //    role = new IdentityRole("Lab Superadmin");
                //    await RoleManager.CreateAsync(role);
                //    addRoleSuccess = roleContext.InsertRole(new RolesModel() { RoleID = role.Id, RoleName = "Lab Superadmin", IsActive = true, IsSuperadmin = false, IsAdmin = true });
                //    if (addRoleSuccess) { }
                //    else { }
                //}


                //if (!roles.Where(x => x.Name == "Superadmin").Any())
                //{
                //    role = new IdentityRole("Superadmin");
                //    var test = await RoleManager.CreateAsync(role);
                //    addRoleSuccess = roleContext.InsertRole(new RolesModel() { RoleID = role.Id, RoleName = "Superadmin", IsActive = true, IsSuperadmin = true, IsAdmin = false });
                //    if (addRoleSuccess) { }
                //    else { }
                //}

                //roles = RoleManager.Roles.ToList();

                //var user = await UserManager.FindByNameAsync("superadmin");

                //if (user == null)
                //{
                //    UserModel adminAccount = new UserModel()
                //    {
                //        Title = "Dr.",
                //        //FirstName = "Lee",
                //        StaffName = "Dr. Lee Eunji",
                //        FullName = "Lee Eunji",
                //        EmployeeID = "456783",
                //        RegistrationNo = "456783",
                //        Gender = "M",
                //        DateOfBirth = "1991-03-15",
                //        RoleID = roles.FirstOrDefault(x => x.Name == "Superadmin").Id,
                //        EmailAddress = "superadmin@superadmin.com",
                //        StatusID = 1,
                //        CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //        CreatedBy = "System"
                //    };

                //    user = Activator.CreateInstance<IdentityUser>();

                //    var emailStore = (IUserEmailStore<IdentityUser>)UserStore;

                //    await UserStore.SetUserNameAsync(user, "superadmin", CancellationToken.None);
                //    await emailStore.SetEmailAsync(user, "superadmin@superadmin.com", CancellationToken.None);
                //    var result = await UserManager.CreateAsync(user, "Abcd@1234");

                //    if (result.Succeeded)
                //    {
                //        adminAccount.UserId = user.Id;

                //        if (usersContext.InsertUser(adminAccount)) { var roleResult = await GetService<UserManager<IdentityUser>>().AddToRoleAsync(user, "superadmin"); }

                //    }
                //}

            }
            catch (Exception ex)
            {
                log.Error("Startup Error >>> ", ex);
            }

            _host.Start();
        }
        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            try
            {
                await _host.StopAsync();

                _host.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Exit Error >>> ", ex);
            }
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
            var ex = e.Exception;
            log.Error("General Error >>> ", ex);
            e.Handled = true;
        }

        public static void GoPreviousPageHandler(EventArgs e, object sender)
        {
            if (GoPreviousPage != null)
            {
                GoPreviousPage(sender, e);
            }
        }

        public static void PopupHandler(EventArgs e, object sender)
        {
            if (Popup != null)
            {
                Popup(sender, e);
            }
        }

        public static void GoToSettingUserPageHandler(EventArgs e, object sender)
        {
            if (GoToSettingUserPage != null)
            {
                GoToSettingUserPage(sender, e);
            }
        }

        public static void GoToSettingLanguageCountryPageHandler(EventArgs e, object sender)
        {
            if (GoToSettingLanguageCountryPage != null)
            {
                GoToSettingLanguageCountryPage(sender, e);
            }
        }

        public static void GoToSettingDevicePageHandler(EventArgs e, object sender)
        {
            if (GoToSettingDevicePage != null)
            {
                GoToSettingDevicePage(sender, e);
            }
        }

        public static void GoToSettingConfigurationPageHandler(EventArgs e, object sender)
        {
            if (GoToSettingConfigurationPage != null)
            {
                GoToSettingConfigurationPage(sender, e);
            }
        }

        public static void GoToSettingReportPageHandler(EventArgs e, object sender)
        {
            if (GoToReportPage != null)
            {
                GoToReportPage(sender, e);
            }
        }

        public static void DownloadPrintReportHandler(EventArgs e, object sender)
        {
            if (DownloadPrintReport != null)
            {
                DownloadPrintReport(sender, e);
            }
        }

        public static void UpdatePatientNameHandler(EventArgs e, object sender)
        {
            if (UpdatePatientName != null)
            {
                UpdatePatientName(sender, e);
            }
        }

        public static void CancelScheduleHandler(EventArgs e, object sender)
        {
            if (CancelSchedule != null)
            {
                CancelSchedule(sender, e);
            }
        }

        public static void GoToViewResultPageHandler(EventArgs e, object sender)
        {
            if (GoToViewResultPage != null)
            {
                GoToViewResultPage(sender, e);
            }
        }

        public async static void CreateUser()
        {
            ConfigurationDBContext ConfigurationContext = GetService<ConfigurationDBContext>();
            MasterCodeDataDBContext sContext = GetService<MasterCodeDataDBContext>();
            RolesDBContext rolesContext = GetService<RolesDBContext>();
            UserDBContext usersContext = GetService<UserDBContext>();
            TemplateDBContext TemplateContext = GetService<TemplateDBContext>();
            NotificationDBContext NotificationContext = GetService<NotificationDBContext>();

            try
            {
                var user = Activator.CreateInstance<IdentityUser>();

                var emailStore = (IUserEmailStore<IdentityUser>)UserStore;

                await UserStore.SetUserNameAsync(user, MainViewModel.Users.LoginID, CancellationToken.None);
                if(MainViewModel.Users.EmailAddress != "")
                {
                    await emailStore.SetEmailAsync(user, MainViewModel.Users.EmailAddress, CancellationToken.None);
                }
                var result = await UserManager.CreateAsync(user, newPassword);

                ConfigurationModel sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();

                if (result.Succeeded)
                {
                    MainViewModel.Users.UserId = user.Id;
                    MainViewModel.Users.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    MainViewModel.Users.CreatedBy = (MainViewModel.CurrentUsers != null) ? MainViewModel.CurrentUsers.FullName : MainViewModel.Users.LoginID;

                    if (usersContext.InsertUser(MainViewModel.Users))
                    {
                        var roleResult = await UserManager.AddToRoleAsync(user, MainViewModel.Users.Role);

                        if (roleResult.Succeeded)
                        {
                            var notificationTemplate = TemplateContext.GetTemplateByCodeLang("US05", (sLangCode != null ? sLangCode.ConfigurationValue : ""));
                            notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("###<staff_id>###", MainViewModel.Users.EmployeeID).Replace("###<staff_fullname>###", MainViewModel.Users.FullName).Replace("'", "''");

                            NotificationModel notification = new NotificationModel()
                            {
                                NotificationType = "Updates",
                                NotificationTitle = notificationTemplate.TemplateTitle,
                                NotificationContent = notificationTemplate.TemplateContent,
                                CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CreatedBy = (MainViewModel.CurrentUsers != null) ? MainViewModel.CurrentUsers.FullName : MainViewModel.Users.LoginID
                            };

                            if (NotificationContext.InsertNotification(notification))
                            {

                            }
                            else
                            {

                            }

                            notificationTemplate = TemplateContext.GetTemplateByCodeLang("EN01", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                            notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("'", "''").Replace("###<staff_fullname>###", MainViewModel.Users.FullName).Replace("###<password>###", newPassword).Replace("###<login_id>###", user.UserName);

                            string sErrorMessage = "";

                            try
                            {
                                EmailObject sEmail = new EmailObject();

                                sEmail.SenderEmail = SMTP.Sender;

                                List<string> sRecipientList = [MainViewModel.Users.EmailAddress];


                                sEmail.RecipientEmail = sRecipientList;
                                sEmail.IsHtml = true;
                                sEmail.Subject = "[VCheck Viewer] " + notificationTemplate.TemplateTitle;
                                sEmail.Body = notificationTemplate.TemplateContent;
                                sEmail.SMTPHost = SMTP.Host;
                                sEmail.PortNo = SMTP.Port;
                                sEmail.HostUsername = SMTP.Username;
                                sEmail.HostPassword = SMTP.Password;
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
                                    CreatedBy = (MainViewModel.CurrentUsers != null) ? MainViewModel.CurrentUsers.FullName : MainViewModel.Users.LoginID
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
                                log.Error("Email Error >>> ", ex);
                            }
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
    }

}
