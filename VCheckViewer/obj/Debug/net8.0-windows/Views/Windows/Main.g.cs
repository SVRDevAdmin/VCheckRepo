﻿#pragma checksum "..\..\..\..\..\Views\Windows\Main.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8E8B37A0EF02889B647C7CA622EC86C8D0B67793"
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
using VCheckViewer.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Converters;
using Wpf.Ui.Markup;


namespace VCheckViewer.Views.Windows {
    
    
    /// <summary>
    /// Main
    /// </summary>
    public partial class Main : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel dcPanel;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationView RootNavigation;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.BreadcrumbBar BreadcrumbBar;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationViewItem T1;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationViewItem T2;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationViewItem T3;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationViewItem T4;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\..\Views\Windows\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Wpf.Ui.Controls.NavigationViewItem T5;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VCheckViewer;component/views/windows/main.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Windows\Main.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.dcPanel = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 2:
            this.RootNavigation = ((Wpf.Ui.Controls.NavigationView)(target));
            
            #line 31 "..\..\..\..\..\Views\Windows\Main.xaml"
            this.RootNavigation.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.RootNavigation_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BreadcrumbBar = ((Wpf.Ui.Controls.BreadcrumbBar)(target));
            return;
            case 4:
            this.T1 = ((Wpf.Ui.Controls.NavigationViewItem)(target));
            return;
            case 5:
            this.T2 = ((Wpf.Ui.Controls.NavigationViewItem)(target));
            return;
            case 6:
            this.T3 = ((Wpf.Ui.Controls.NavigationViewItem)(target));
            return;
            case 7:
            this.T4 = ((Wpf.Ui.Controls.NavigationViewItem)(target));
            return;
            case 8:
            this.T5 = ((Wpf.Ui.Controls.NavigationViewItem)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

