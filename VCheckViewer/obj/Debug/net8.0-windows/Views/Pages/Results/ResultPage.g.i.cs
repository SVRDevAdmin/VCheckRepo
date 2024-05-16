﻿#pragma checksum "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4D58250112805BA269286CB7FE6A14FBF1F920F9"
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
using VCheckViewer.Views.Pages.Results;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Converters;
using Wpf.Ui.Markup;


namespace VCheckViewer.Views.Pages.Results {
    
    
    /// <summary>
    /// ResultPage
    /// </summary>
    public partial class ResultPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 72 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox KeywordSearchBar;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VCheckViewer.UserControls.DateRangePicker RangeDate;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboSort;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDownload;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPrint;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgResult;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel paginationPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/VCheckViewer;component/views/pages/results/resultpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.3.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.KeywordSearchBar = ((System.Windows.Controls.TextBox)(target));
            
            #line 72 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            this.KeywordSearchBar.KeyUp += new System.Windows.Input.KeyEventHandler(this.KeywordSearchBar_KeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RangeDate = ((VCheckViewer.UserControls.DateRangePicker)(target));
            return;
            case 3:
            this.cboSort = ((System.Windows.Controls.ComboBox)(target));
            
            #line 105 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            this.cboSort.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cboSort_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnDownload = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.btnPrint = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.dgResult = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 9:
            this.paginationPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 161 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.menuDownload_Click);
            
            #line default
            #line hidden
            break;
            case 8:
            
            #line 162 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.menuPrint_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

