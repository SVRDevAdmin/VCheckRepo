﻿#pragma checksum "..\..\..\..\UserControls\DateRangePicker.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "151FE3550CD9D6317D7731B771077787980DD6EA"
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


namespace VCheckViewer.UserControls {
    
    
    /// <summary>
    /// DateRangePicker
    /// </summary>
    public partial class DateRangePicker : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\UserControls\DateRangePicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VCheckViewer.UserControls.DateRangePicker @this;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\UserControls\DateRangePicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DateRangePicker_TextBox;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\UserControls\DateRangePicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup DateRangePicker_Popup;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\UserControls\DateRangePicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewbox DateRangePicker_DropDown;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\..\UserControls\DateRangePicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Calendar DateRangePicker_Calendar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VCheckViewer;component/usercontrols/daterangepicker.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\DateRangePicker.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.@this = ((VCheckViewer.UserControls.DateRangePicker)(target));
            return;
            case 2:
            this.DateRangePicker_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 45 "..\..\..\..\UserControls\DateRangePicker.xaml"
            this.DateRangePicker_TextBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.Handle_Escape_Button);
            
            #line default
            #line hidden
            
            #line 45 "..\..\..\..\UserControls\DateRangePicker.xaml"
            this.DateRangePicker_TextBox.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.DateRangePicker_TextBox_GotMouseCapture);
            
            #line default
            #line hidden
            return;
            case 3:
            this.DateRangePicker_Popup = ((System.Windows.Controls.Primitives.Popup)(target));
            return;
            case 4:
            this.DateRangePicker_DropDown = ((System.Windows.Controls.Viewbox)(target));
            return;
            case 5:
            this.DateRangePicker_Calendar = ((System.Windows.Controls.Calendar)(target));
            
            #line 97 "..\..\..\..\UserControls\DateRangePicker.xaml"
            this.DateRangePicker_Calendar.SelectedDatesChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.DateRangePicker_Calendar_SelectedDatesChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

