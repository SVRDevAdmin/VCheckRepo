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
//using VCheckViewer.Lib.Base;
using Microsoft.AspNetCore.Hosting.Internal;
using VCheck.Lib.Data.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using VCheckViewer.Lib.Models;

namespace VCheckViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public IConfiguration Configuration { get; }

        public static MainViewModel MainViewModel { get; } = new MainViewModel();

        public static event EventHandler GoPreviousPage;
        public static event EventHandler Popup;

        public static SignInManager<IdentityUser> SignInManager { get; set; }
        public static UserManager<IdentityUser> UserManager { get; set; }
        public static RoleManager<IdentityRole> RoleManager { get; set; }
        public static IUserStore<IdentityUser> UserStore { get; set; }

        public static string newPassword {  get; set; }

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
                services.Add(new ServiceDescriptor(typeof(UserLoginDBContext), new UserLoginDBContext(context.Configuration)));

                services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(context.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21))));

                var identitySettings = context.Configuration.GetSection("IdentitySettings");

                services.AddIdentityCore<IdentityUser>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 1;


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
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
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
    }

}
