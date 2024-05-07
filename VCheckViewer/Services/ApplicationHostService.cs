using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui;
using Microsoft.Extensions.DependencyInjection;
using VCheckViewer.Views.Windows;
using VCheck.Lib.Data;
using VCheckViewer.Views.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VCheckViewer.Views.Pages.Login;

namespace VCheckViewer.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private INavigationWindow _navigationWindow;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            //if (!Application.Current.Windows.OfType<Login>().Any())
            //{
            //     _navigationWindow = (
            //        _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow
            //    )!;
            //     _navigationWindow!.ShowWindow();

            //     //_navigationWindow.Navigate(typeof(DashboardPage));
            // }

            App.SignInManager = _serviceProvider.GetRequiredService<SignInManager<IdentityUser>>();
            App.UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            App.RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            App.UserStore = _serviceProvider.GetRequiredService<IUserStore<IdentityUser>>();

            LoginWindow LoginPage = new LoginWindow();
            LoginPage.Navigate(new LoginPage());
            LoginPage.Show();

            await Task.CompletedTask;
        }
    }
}
