using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheck.Helper;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Views.Windows;
using Brushes = System.Windows.Media.Brushes;
using TextBox = System.Windows.Controls.TextBox;

namespace VCheckViewer.Views.Pages.Login
{
    /// <summary>
    /// Interaction logic for PasswordRecoveryPage.xaml
    /// </summary>
    public partial class PasswordRecoveryPage : Page
    {
        public static event EventHandler Popup;
        public static event EventHandler GoToLoginPage;
        public string newPassword;
        public IdentityUser user;
        public UserModel userModel;

        TemplateDBContext TemplateContext = App.GetService<TemplateDBContext>();
        UserDBContext UserContext = App.GetService<UserDBContext>();
        NotificationDBContext NotificationContext = App.GetService<NotificationDBContext>();
        ConfigurationDBContext ConfigurationContext = App.GetService<ConfigurationDBContext>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PasswordRecoveryPage()
        {
            InitializeComponent();
            CheckThemesSettings();

            LoginWindow.ResetPassword += new EventHandler(ProceedResetPassword);

            var pageTitle = Properties.Resources.Login_Label_PasswordRecovery.Split("<nextline>");
            Login_Label_PasswordRecovery.Text = pageTitle[0] + "\r\n" + pageTitle[1];

            var Login_Label_LeftMain_array = Properties.Resources.Login_Label_LeftMain.Split("<nextline>");
            Login_Label_LeftMain.Text = Login_Label_LeftMain_array[0] + "\r\n" + 
                                        ((Login_Label_LeftMain_array.Length > 1) ? Login_Label_LeftMain_array[1] : "");

        }

        private async void ResetPassword(object sender, RoutedEventArgs e)
        {
            user = await App.UserManager.FindByNameAsync(Username.Text);

            if(user != null)
            {
                if (user.NormalizedEmail == Email.Text.ToUpper())
                {
                    newPassword = RandomPasswordGenerator();

                    userModel = UserContext.GetUserByID(user.Id);

                    if(userModel != null) { PopupHandler(e, sender); }

                    
                }
                else
                {
                    ErrorText.Visibility = Visibility.Visible;
                    //ErrorText.Text = "Wrong email linked to the account, please verify it is the correct email and try again.";
                    //ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongEmail;
                    ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongUsernameEmail;
                }
            }
            else
            {
                ErrorText.Visibility = Visibility.Visible;
                //ErrorText.Text = "Cannot find user according to Login ID, please verify it is the correct login ID and try again.";
                //ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongLoginIDRecovery;
                ErrorText.Text = Properties.Resources.Login_ErrorMessage_WrongUsernameEmail;
            }
        }

        private void CheckValue(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = null;
            Border parentBorder;
            Grid parentGrid;

            textBox = sender as TextBox; parentGrid = textBox.Parent as Grid;

            parentBorder = parentGrid.Parent as Border;


            if ((textBox != null && textBox.Text == ""))
            {
                parentBorder.BorderBrush = Brushes.Red;
                parentBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_MandatoryField;
            }
            else if (textBox != null && textBox.Name == "Username" && textBox.Text.Length < 5)
            {
                parentBorder.BorderBrush = Brushes.Red;
                parentBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_FiveCharMin;
            }
            else if (textBox != null && textBox.Name == "Email" && !textBox.Text.Contains("@"))
            {
                parentBorder.BorderBrush = Brushes.Red;
                parentBorder.ToolTip = Properties.Resources.Setting_ErrorMessage_EmailFormat;
            }
            else
            {
                parentBorder.BorderBrush = Brushes.Black;
                parentBorder.ToolTip = "No issue";
            }
        }

        private async void ProceedResetPassword(object sender, EventArgs e)
        {
            var removepassword = await App.UserManager.RemovePasswordAsync(user);
            if(removepassword.Succeeded)
            {
                var resetPassword = await App.UserManager.AddPasswordAsync(user, newPassword);

                if (resetPassword.Succeeded)
                {
                    ConfigurationModel sLangCode = ConfigurationContext.GetConfigurationData("SystemSettings_Language").FirstOrDefault();

                    //var notificationTemplate = TemplateContext.GetTemplateByCode("EN02");
                    var notificationTemplate = TemplateContext.GetTemplateByCodeLang("EN02", (sLangCode != null) ? sLangCode.ConfigurationValue : "");
                    notificationTemplate.TemplateContent = notificationTemplate.TemplateContent.Replace("'", "''").Replace("###<staff_fullname>###", userModel.FullName).Replace("###<password>###", newPassword);

                    string sErrorMessage;

                    try
                    {
                        EmailObject sEmail = new EmailObject();

                        sEmail.SenderEmail = App.SMTP.Sender;

                        List<string> sRecipientList = [user.NormalizedEmail, "azwan@svrtech.com.my"];


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

                        NotificationModel notification = new NotificationModel()
                        {
                            NotificationType = "Email",
                            NotificationTitle = notificationTemplate.TemplateTitle,
                            NotificationContent = notificationTemplate.TemplateContent,
                            Receiver = string.Join(", ", sRecipientList),
                            CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            CreatedBy = userModel.FullName
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
                        ErrorText.Visibility = Visibility.Visible;
                        //ErrorText.Text = "Error occur when creating password. Please contact administrator.";
                        ErrorText.Text = Properties.Resources.Login_Message_CreatePasswordError;
                        App.log.Error("Password Recovery Error >>> ", ex);
                    }
                }
                else
                {
                    ErrorText.Visibility = Visibility.Visible;
                    //ErrorText.Text = "Error occur when creating password. Please contact administrator.";
                    ErrorText.Text = Properties.Resources.Login_Message_CreatePasswordError;
                }
            }
            else
            {
                ErrorText.Visibility = Visibility.Visible;
                //ErrorText.Text = "Error occur when removing password. Please contact administrator.";
                ErrorText.Text = Properties.Resources.Login_Message_RemovePasswordError;
            }
            
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            GoToLoginPageHandler(e, sender);
        }

        private static void GoToLoginPageHandler(EventArgs e, object sender)
        {
            if (GoToLoginPage != null)
            {
                GoToLoginPage(sender, e);
            }
        }

        private static void PopupHandler(EventArgs e, object sender)
        {
            if (Popup != null)
            {
                Popup(sender, e);
            }
        }

        private string RandomPasswordGenerator()
        {
            Random res = new Random();

            // String that contain alphabets, numbers and special character
            String lowerCase = "abcdefghijklmnopqrstuvwxyz";
            String upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            String number = "0123456789";
            String specialChar = "!@#$%^&*()_+-={}|[];,./:<>?";

            // Initializing the empty string 
            String randomstring = "";

            randomstring += lowerCase[res.Next(lowerCase.Length)];
            randomstring += upperCase[res.Next(upperCase.Length)];
            randomstring += number[res.Next(number.Length)];
            randomstring += specialChar[res.Next(specialChar.Length)];
            randomstring += lowerCase[res.Next(lowerCase.Length)];
            randomstring += upperCase[res.Next(upperCase.Length)];
            randomstring += number[res.Next(number.Length)];
            randomstring += specialChar[res.Next(specialChar.Length)];

            char[] chars = randomstring.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = res.Next(0, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }

            randomstring = string.Join("",chars);

            return randomstring;
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

        private void btnDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            var sButton = (System.Windows.Controls.Button)sender;
            ConfigurationContext.UpdateConfiguration("SystemSettings_Themes", (sButton != null) ? sButton.Tag.ToString() : "");

            CheckThemesSettings();
        }

        private void btnLightTheme_Click(object sender, RoutedEventArgs e)
        {
            var sButton = (System.Windows.Controls.Button)sender;
            ConfigurationContext.UpdateConfiguration("SystemSettings_Themes", (sButton != null) ? sButton.Tag.ToString() : "");

            CheckThemesSettings();
        }
    }
}
