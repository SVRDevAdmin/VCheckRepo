﻿#pragma checksum "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A8AD62EDBC86B087407F72145AE5406CBAC8FB93"
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
using VCheckViewer.UserControls;
using VCheckViewer.Views.Pages.Notification;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Converters;
using Wpf.Ui.Markup;


namespace VCheckViewer.Views.Pages.Notification {
    
    
    /// <summary>
    /// NotificationPage
    /// </summary>
    public partial class NotificationPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 45 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox KeywordSearchBar;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VCheckViewer.UserControls.DateRangePicker RangeDate;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox NotificationType;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel NotificationViewList;
        
        #line default
        #line hidden
        
        
        #line 158 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VCheckViewer.UserControls.PagingControl pagination;
        
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
            System.Uri resourceLocater = new System.Uri("/VCheckViewer;V1.0.0.1;component/views/pages/notification/notificationpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.2.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 24 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.CloseCalenderPopup);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 38 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.CloseCalenderPopup);
            
            #line default
            #line hidden
            return;
            case 3:
            this.KeywordSearchBar = ((System.Windows.Controls.TextBox)(target));
            
            #line 45 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
            this.KeywordSearchBar.GotFocus += new System.Windows.RoutedEventHandler(this.ChangeNotificationType);
            
            #line default
            #line hidden
            return;
            case 4:
            this.RangeDate = ((VCheckViewer.UserControls.DateRangePicker)(target));
            return;
            case 5:
            this.NotificationType = ((System.Windows.Controls.ComboBox)(target));
            
            #line 98 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
            this.NotificationType.GotFocus += new System.Windows.RoutedEventHandler(this.ChangeNotificationType);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 108 "..\..\..\..\..\..\Views\Pages\Notification\NotificationPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.UpdateFilter);
            
            #line default
            #line hidden
            return;
            case 7:
            this.NotificationViewList = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.pagination = ((VCheckViewer.UserControls.PagingControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

