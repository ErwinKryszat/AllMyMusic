﻿#pragma checksum "..\..\..\Controls\KnobControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "66735F0A2C1E3400E82AE835D84E1089654B3BA9221DC55CFF878D730065307A"
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


namespace AllMyMusic.Controls {
    
    
    /// <summary>
    /// KnobControl
    /// </summary>
    public partial class KnobControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Controls\KnobControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AllMyMusic.Controls.KnobControl knobControl;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Controls\KnobControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid mainGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/AllMyMusic;component/controls/knobcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\KnobControl.xaml"
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
            this.knobControl = ((AllMyMusic.Controls.KnobControl)(target));
            
            #line 10 "..\..\..\Controls\KnobControl.xaml"
            this.knobControl.Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.mainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            
            #line 61 "..\..\..\Controls\KnobControl.xaml"
            ((System.Windows.Shapes.Ellipse)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.My_MouseMove);
            
            #line default
            #line hidden
            
            #line 62 "..\..\..\Controls\KnobControl.xaml"
            ((System.Windows.Shapes.Ellipse)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.My_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 63 "..\..\..\Controls\KnobControl.xaml"
            ((System.Windows.Shapes.Ellipse)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.My_PreviewMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 79 "..\..\..\Controls\KnobControl.xaml"
            ((System.Windows.Shapes.Ellipse)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.My_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 80 "..\..\..\Controls\KnobControl.xaml"
            ((System.Windows.Shapes.Ellipse)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.My_PreviewMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

