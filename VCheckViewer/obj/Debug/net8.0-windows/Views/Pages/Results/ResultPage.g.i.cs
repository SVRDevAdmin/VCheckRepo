﻿#pragma checksum "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B66477323B843C8CF6A32069DF1CBE2AE4BA6E1D"
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
        
        
        #line 120 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox KeywordSearchBar;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VCheckViewer.UserControls.DateRangePicker RangeDate;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboSort;
        
        #line default
        #line hidden
        
        
        #line 166 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilter;
        
        #line default
        #line hidden
        
        
        #line 177 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDownload;
        
        #line default
        #line hidden
        
        
        #line 184 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPrint;
        
        #line default
        #line hidden
        
        
        #line 191 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtTotalCount;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgResult;
        
        #line default
        #line hidden
        
        
        #line 239 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VCheckViewer.UserControls.PagingControl pagination;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.10.0")]
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
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.10.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 82 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.CloseCalenderPopup);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 97 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.CloseCalenderPopup);
            
            #line default
            #line hidden
            return;
            case 3:
            this.KeywordSearchBar = ((System.Windows.Controls.TextBox)(target));
            
            #line 120 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            this.KeywordSearchBar.GotFocus += new System.Windows.RoutedEventHandler(this.ChangeSortBy);
            
            #line default
            #line hidden
            return;
            case 4:
            this.RangeDate = ((VCheckViewer.UserControls.DateRangePicker)(target));
            return;
            case 5:
            this.cboSort = ((System.Windows.Controls.ComboBox)(target));
            
            #line 160 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            this.cboSort.GotFocus += new System.Windows.RoutedEventHandler(this.ChangeSortBy);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnFilter = ((System.Windows.Controls.Button)(target));
            
            #line 170 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            this.btnFilter.Click += new System.Windows.RoutedEventHandler(this.btnFilter_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnDownload = ((System.Windows.Controls.Button)(target));
            
            #line 177 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            this.btnDownload.Click += new System.Windows.RoutedEventHandler(this.btnDownload_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnPrint = ((System.Windows.Controls.Button)(target));
            
            #line 184 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            this.btnPrint.Click += new System.Windows.RoutedEventHandler(this.btnPrint_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.txtTotalCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.dgResult = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 13:
            this.pagination = ((VCheckViewer.UserControls.PagingControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 11:
            
            #line 225 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.menuDownload_Click);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 226 "..\..\..\..\..\..\Views\Pages\Results\ResultPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.menuPrint_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

