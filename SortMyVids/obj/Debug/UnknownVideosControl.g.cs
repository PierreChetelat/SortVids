﻿#pragma checksum "..\..\UnknownVideosControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E3D65F93EAD57F809F7108C579C0EEF5"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18408
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace SortMyVids {
    
    
    /// <summary>
    /// UnknownVideosControl
    /// </summary>
    public partial class UnknownVideosControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\UnknownVideosControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton uiRadioChoice;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\UnknownVideosControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox uiComboBoxChoice;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\UnknownVideosControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button uiButtonValidateChoice;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\UnknownVideosControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton uiRadioEdit;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\UnknownVideosControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox uiTextTitle;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\UnknownVideosControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox uiComboBoxEdit;
        
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
            System.Uri resourceLocater = new System.Uri("/SortMyVids;component/unknownvideoscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\UnknownVideosControl.xaml"
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
            this.uiRadioChoice = ((System.Windows.Controls.RadioButton)(target));
            
            #line 26 "..\..\UnknownVideosControl.xaml"
            this.uiRadioChoice.Checked += new System.Windows.RoutedEventHandler(this.uiRadio_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            this.uiComboBoxChoice = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.uiButtonValidateChoice = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.uiRadioEdit = ((System.Windows.Controls.RadioButton)(target));
            
            #line 30 "..\..\UnknownVideosControl.xaml"
            this.uiRadioEdit.Checked += new System.Windows.RoutedEventHandler(this.uiRadio_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.uiTextTitle = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.uiComboBoxEdit = ((System.Windows.Controls.ComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

