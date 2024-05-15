using Microsoft.Extensions.Configuration;
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
using NLog;

namespace VCheckViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public IConfiguration Configuration { get; }

        public static MainViewModel MainViewModel { get; } = new MainViewModel();

        public static event EventHandler GoPreviousPage;
        public static event EventHandler Popup;

        public static event EventHandler GoToSettingUserPage;
        public static event EventHandler GoToSettingLanguageCountryPage;
        public static event EventHandler GoToSettingDevicePage;
        public static event EventHandler GoToSettingConfigurationPage;

        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
        public static RoleManager<IdentityRole> RoleManager { get; set; }
        public static IUserStore<IdentityUser> UserStore { get; set; }

        public static string newPassword {  get; set; }
        public static SMTPModel SMTP {  get; set; }

        //App()
        //{
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
        //}

        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => 
            { 
                c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                services.AddSingleton<IPageService, PageServices>();
                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<ITaskBarService, TaskBarService>();
                services.AddSingleton<INavigationService, NavigationService>();

                //services.AddSingleton<INavigationWindow, Main>();

                //services.AddSingleton<INavigationWindow, Login>();

                //services.Add(new ServiceDescriptor(typeof(VCheck.Lib.Data.VCheckDBContext), new VCheck.Lib.Data.VCheckDBContext(context.Configuration)));

                //services.Add(new ServiceDescriptor(
                //                typeof(VCheck.Lib.Data.SampleClass), 
                //                new VCheck.Lib.Data.SampleClass(context.Configuration)
                //));

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
                ConfigurationDBContext ConfigurationContext = GetService<ConfigurationDBContext>();
                UserDBContext usersContext = GetService<UserDBContext>();
                RolesDBContext roleContext = GetService<RolesDBContext>();

                MainViewModel.ConfigurationModel = ConfigurationContext.GetConfigurationData("");

                var language = MainViewModel.ConfigurationModel.Where(x => x.ConfigurationKey == "SystemSettings_Language").FirstOrDefault().ConfigurationValue;

                CultureInfo sZHCulture = new CultureInfo(language);
                CultureResources.ChangeCulture(sZHCulture);

                _host.Start();

                var roles = RoleManager.Roles.ToList();
                IdentityRole role = new IdentityRole();
                bool addRoleSuccess;

                if (!roles.Where(x => x.Name == "Lab User").Any())
                {
                    role = new IdentityRole("Lab User");
                    await RoleManager.CreateAsync(role);
                    addRoleSuccess = roleContext.InsertRole(new RolesModel() { RoleID = role.Id, RoleName = "Lab User", IsActive = true, IsSuperadmin = false, IsAdmin = false });
                    if (addRoleSuccess) { }
                    else { }
                }


                if (!roles.Where(x => x.Name == "Lab Superadmin").Any())
                {
                    role = new IdentityRole("Lab Superadmin");
                    await RoleManager.CreateAsync(role);
                    addRoleSuccess = roleContext.InsertRole(new RolesModel() { RoleID = role.Id, RoleName = "Lab Superadmin", IsActive = true, IsSuperadmin = false, IsAdmin = true });
                    if (addRoleSuccess) { }
                    else { }
                }


                if (!roles.Where(x => x.Name == "Superadmin").Any())
                {
                    role = new IdentityRole("Superadmin");
                    var test = await RoleManager.CreateAsync(role);
                    addRoleSuccess = roleContext.InsertRole(new RolesModel() { RoleID = role.Id, RoleName = "Superadmin", IsActive = true, IsSuperadmin = true, IsAdmin = false });
                    if (addRoleSuccess) { }
                    else { }
                }

                roles = RoleManager.Roles.ToList();

                var user = await UserManager.FindByNameAsync("superadmin");

                if (user == null)
                {
                    UserModel adminAccount = new UserModel()
                    {
                        Title = "Dr.",
                        //FirstName = "Lee",
                        StaffName = "Dr. Lee Eunji",
                        FullName = "Lee Eunji",
                        EmployeeID = "456783",
                        RegistrationNo = "456783",
                        Gender = "M",
                        DateOfBirth = "1991-03-15",
                        RoleID = roles.Where(x => x.Name == "Superadmin").FirstOrDefault().Id,
                        EmailAddress = "superadmin@superadmin.com",
                        StatusID = 1
                    };

                    user = Activator.CreateInstance<IdentityUser>();

                    var emailStore = (IUserEmailStore<IdentityUser>)UserStore;

                    await UserStore.SetUserNameAsync(user, "superadmin", CancellationToken.None);
                    await emailStore.SetEmailAsync(user, "superadmin@superadmin.com", CancellationToken.None);
                    var result = await UserManager.CreateAsync(user, "Abcd@1234");

                    if (result.Succeeded)
                    {
                        adminAccount.UserId = user.Id;

                        if (usersContext.InsertUser(adminAccount)) { var roleResult = await UserManager.AddToRoleAsync(user, "superadmin"); }

                    }
                }
            }
            catch (Exception ex)
            {

            }        
        }
        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
            var ex = e.Exception;
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
    }

}
