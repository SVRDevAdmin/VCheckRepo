﻿#pragma checksum "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9AFC2CA31FE37E04D7CE9694ADE1079EA084FC4B"
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
    /// ResetPasswordPage
    /// </summary>
    public partial class ResetPasswordPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ErrorText;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Email;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox NewPassword;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock NewPasswordPlaceholder;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock NewPasswordLength;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image NewpasswordCorrect;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox ConfirmPassword;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ConfirmPasswordPlaceholder;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ConfirmPasswordLength;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ConfirmPasswordCorrect;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.2.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VCheckViewer;component/views/pages/login/resetpasswordpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ErrorText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.Email = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.NewPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 76 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
            this.NewPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.PasswordPlaceholderHandler);
            
            #line default
            #line hidden
            return;
            case 4:
            this.NewPasswordPlaceholder = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.NewPasswordLength = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.NewpasswordCorrect = ((System.Windows.Controls.Image)(target));
            return;
            case 7:
            this.ConfirmPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 102 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
            this.ConfirmPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.PasswordPlaceholderHandler);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ConfirmPasswordPlaceholder = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.ConfirmPasswordLength = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.ConfirmPasswordCorrect = ((System.Windows.Controls.Image)(target));
            return;
            case 11:
            
            #line 118 "..\..\..\..\..\..\Views\Pages\Login\ResetPasswordPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BackToLogin);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

