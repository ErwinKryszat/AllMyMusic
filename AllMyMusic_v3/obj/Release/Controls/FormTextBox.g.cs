﻿#pragma checksum "..\..\..\Controls\FormTextBox.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8D1A5A6E1793D4AC067796D7E19B995E2560997D"
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


namespace AllMyMusic_v3.Controls {
    
    
    /// <summary>
    /// FormTextBox
    /// </summary>
    public partial class FormTextBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\Controls\FormTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AllMyMusic_v3.Controls.FormTextBox formTextBox;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Controls\FormTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Controls\FormTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AllMyMusic_v3;component/controls/formtextbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\FormTextBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.formTextBox = ((AllMyMusic_v3.Controls.FormTextBox)(target));
            return;
            case 2:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.textbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 34 "..\..\..\Controls\FormTextBox.xaml"
            this.textbox.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.textbox_PreviewMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
