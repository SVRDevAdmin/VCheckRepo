﻿#pragma checksum "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3A3719233243FE7FABA2F9A73E70D6FB07ACA6C7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using VCheckViewer.Views.Pages.Login;


namespace VCheckViewer.Views.Pages.Login {
    
    
    /// <summary>
    /// LoginPage
    /// </summary>
    public partial class LoginPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 45 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDarkTheme;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLightTheme;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Login_Label_LeftMain;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ErrorText;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Username;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Password;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PasswordPlaceholder;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VCheckViewer;V1.0.0.1;component/views/pages/login/loginpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnDarkTheme = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            this.btnDarkTheme.Click += new System.Windows.RoutedEventHandler(this.btnDarkTheme_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnLightTheme = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            this.btnLightTheme.Click += new System.Windows.RoutedEventHandler(this.btnLightTheme_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Login_Label_LeftMain = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.ErrorText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.Username = ((System.Windows.Controls.TextBox)(target));
            
            #line 101 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            this.Username.KeyUp += new System.Windows.Input.KeyEventHandler(this.CheckValue);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Password = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 122 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            this.Password.PasswordChanged += new System.Windows.RoutedEventHandler(this.PasswordPlaceholderHandler);
            
            #line default
            #line hidden
            
            #line 122 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            this.Password.KeyUp += new System.Windows.Input.KeyEventHandler(this.CheckValue);
            
            #line default
            #line hidden
            return;
            case 7:
            this.PasswordPlaceholder = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            
            #line 132 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LoginButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 133 "..\..\..\..\..\..\Views\Pages\Login\LoginPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ResetPassword);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

