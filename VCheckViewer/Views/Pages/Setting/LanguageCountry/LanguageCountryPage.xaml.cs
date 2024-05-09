﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewer.Lib.Culture;

namespace VCheckViewer.Views.Pages.Setting.LanguageCountry
{
    /// <summary>
    /// Interaction logic for LanguageCountryPage.xaml
    /// </summary>
    public partial class LanguageCountryPage : Page
    {
        CountryDBContext sContext = App.GetService<CountryDBContext>();
        MasterCodeDataDBContext MasterCodeDataContext = App.GetService<MasterCodeDataDBContext>();
        ConfigurationDBContext ConfigurationContext = App.GetService<ConfigurationDBContext>();

        ConfigurationModel currentCountry;
        ConfigurationModel currentLanguage;

        public string languageSelected = null;
        public string countrySelected = null;

        public LanguageCountryPage()
        {
            InitializeComponent();

            initializedList();
        }

        public void initializedList()
        {
            System.Windows.Controls.RadioButton radioButton;
            DockPanel dockPanel;
            System.Windows.Controls.Image image;
            TextBlock textBlock;

            currentCountry = App.MainViewModel.ConfigurationModel.Where(x => x.ConfigurationKey == "SystemSettings_Country").FirstOrDefault();
            currentLanguage = App.MainViewModel.ConfigurationModel.Where(x => x.ConfigurationKey == "SystemSettings_Language").FirstOrDefault();

            LanguageListView.Items.Clear();

            var languageList = MasterCodeDataContext.GetMasterCodeData("LanguageSelection");

            foreach ( var language in languageList )
            {
                radioButton = new System.Windows.Controls.RadioButton();
                dockPanel = new DockPanel();
                image = new System.Windows.Controls.Image();
                textBlock = new TextBlock();

                var uri = new Uri("pack://application:,,,/Content/Images/Icons/" + language.CodeID+".png");
                var bitmap = new BitmapImage(uri);

                if(language.CodeID == currentLanguage.ConfigurationValue) { radioButton.IsChecked = true; languageSelected = currentLanguage.ConfigurationValue; }

                radioButton.Tag = language.CodeID;
                radioButton.Checked += new RoutedEventHandler(LanguageSelected_Checked);
                dockPanel.Height = 34;
                dockPanel.Margin = new Thickness(0, -6, 0, 0);
                image.Source = bitmap;
                image.Margin = new Thickness(2);
                textBlock.Text = language.CodeName;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Margin = new Thickness(2,0,0,2);

                dockPanel.Children.Add(image);
                dockPanel.Children.Add(textBlock);
                radioButton.Content = dockPanel;

                LanguageListView.Items.Add(radioButton);
            }


            List<CountryModel> countryList = sContext.GetCountryData("");

            CountryListView.Items.Clear();

            foreach (CountryModel country in countryList)
            {
                MenuItem item = new MenuItem();
                item.Header = country.CountryName;
                item.Tag = country.CountryCode;

                if (country.CountryCode == currentCountry.ConfigurationValue) { item.Header += " (selected)"; countrySelected = currentCountry.ConfigurationValue; item.Foreground = System.Windows.Media.Brushes.DarkOrange; }

                item.BorderThickness = new Thickness(0);
                item.Click += new RoutedEventHandler(CountrySelected_Click);
                CountryListView.Items.Add(item);
            }
        }

        private void LanguageSelected_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = (System.Windows.Controls.RadioButton)sender;

            languageSelected = radioButton.Tag.ToString();

            //var getSelectedLanguage = (TextBlock)VisualTreeHelper.GetChild((DockPanel)radioButton.Content, 1);
            //SelectedLanguage.Text = getSelectedLanguage.Text;   



        }

        private void CountrySearchBar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string partialName = CountrySearchBar.Text;
            List<CountryModel> countryList = null;

            if (partialName.Length > 2)
            {
                CountryListView.Items.Clear();

                countryList = sContext.GetCountryData(partialName);
            }
            else
            {
                CountryListView.Items.Clear();

                countryList = sContext.GetCountryData("");
            }

            foreach (CountryModel country in countryList)
            {
                MenuItem item = new MenuItem();
                item.Header = country.CountryName;
                item.Tag = country.CountryCode;

                if (country.CountryCode == countrySelected) { item.Header += " (selected)"; item.Foreground = System.Windows.Media.Brushes.DarkOrange; }

                item.BorderThickness = new Thickness(0);
                item.Click += new RoutedEventHandler(CountrySelected_Click);
                CountryListView.Items.Add(item);
            }

        }

        private void CountrySelected_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            countrySelected = item.Tag.ToString();

            CountrySearchBar_KeyUp(null, null);

            //SelectedCountry.Text = item.Header.ToString();

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            List<ConfigurationModel> configurationModels = new List<ConfigurationModel>()
            {
                new ConfigurationModel() { ConfigurationKey = "SystemSettings_Country" , ConfigurationValue = countrySelected},
                new ConfigurationModel() { ConfigurationKey = "SystemSettings_Language" , ConfigurationValue = languageSelected}
            };

            if(currentCountry.ConfigurationValue == countrySelected && currentLanguage.ConfigurationValue == languageSelected) { ErrorText.Visibility = Visibility.Visible; }
            else 
            { 
                ErrorText.Visibility = Visibility.Hidden;

                //App.MainViewModel.ConfigurationModel = configurationModels;
                currentCountry.ConfigurationValueTemp = countrySelected;
                currentLanguage.ConfigurationValueTemp = languageSelected;

                App.MainViewModel.Origin = "ChangeLanguageCountry";

                App.PopupHandler(e, sender);
            }
        }

        private void UserPage(object sender, RoutedEventArgs e)
        {
            App.GoToSettingUserPageHandler(e, sender);
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            App.GoToSettingDevicePageHandler(e, sender);
        }
    }
}