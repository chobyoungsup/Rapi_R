﻿#pragma checksum "..\..\08_PasswordPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4685977C7053C8EB2C7641498845F432B8998BC2"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using QuickChargeDisplay.Control;
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


namespace QuickChargeDisplay {
    
    
    /// <summary>
    /// PasswordPage
    /// </summary>
    public partial class PasswordPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\08_PasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image logOut;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\08_PasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbAdminSelect;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\08_PasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle passwordBoxRect;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\08_PasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordBox;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\08_PasswordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image adminPwConfirm;
        
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
            System.Uri resourceLocater = new System.Uri("/QuickChargeDisplay;component/08_passwordpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\08_PasswordPage.xaml"
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
            
            #line 5 "..\..\08_PasswordPage.xaml"
            ((QuickChargeDisplay.PasswordPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            
            #line 5 "..\..\08_PasswordPage.xaml"
            ((QuickChargeDisplay.PasswordPage)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.Page_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.logOut = ((System.Windows.Controls.Image)(target));
            
            #line 14 "..\..\08_PasswordPage.xaml"
            this.logOut.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.logOut_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cbAdminSelect = ((System.Windows.Controls.ComboBox)(target));
            
            #line 18 "..\..\08_PasswordPage.xaml"
            this.cbAdminSelect.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbAdminSelect_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.passwordBoxRect = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 23 "..\..\08_PasswordPage.xaml"
            this.passwordBoxRect.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.passwordBox_MouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.passwordBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 6:
            this.adminPwConfirm = ((System.Windows.Controls.Image)(target));
            
            #line 27 "..\..\08_PasswordPage.xaml"
            this.adminPwConfirm.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.adminPwConfirm_MouseDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

