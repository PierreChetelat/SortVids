﻿#pragma checksum "..\..\ResearchControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2AD629F39C8DFCD8E9E7E7CEFAFD4555"
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
    /// ResearchControl
    /// </summary>
    public partial class ResearchControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\ResearchControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox uiFolderSrc;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\ResearchControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button uiButtonSrc;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\ResearchControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox uiFolderDest;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\ResearchControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button uiButtonDest;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\ResearchControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox uiListMovie;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\ResearchControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox uiListSortMovie;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\ResearchControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button uiButtonLaunch;
        
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
            System.Uri resourceLocater = new System.Uri("/SortMyVids;component/researchcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ResearchControl.xaml"
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
            this.uiFolderSrc = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.uiButtonSrc = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\ResearchControl.xaml"
            this.uiButtonSrc.Click += new System.Windows.RoutedEventHandler(this.uiButtonSrc_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.uiFolderDest = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.uiButtonDest = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\ResearchControl.xaml"
            this.uiButtonDest.Click += new System.Windows.RoutedEventHandler(this.uiButtonDest_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.uiListMovie = ((System.Windows.Controls.ListBox)(target));
            return;
            case 6:
            this.uiListSortMovie = ((System.Windows.Controls.ListBox)(target));
            return;
            case 7:
            this.uiButtonLaunch = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

