﻿#pragma checksum "C:\Users\Administrator\source\repos3\RentMeRentalSystem\View\MainMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B4653A2549ACABDDD37B05A834D00C26"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentMeRentalSystem
{
    partial class MainMenu : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // View\MainMenu.xaml line 18
                {
                    this.CurrentUsername = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3: // View\MainMenu.xaml line 19
                {
                    this.CurrentUserId = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4: // View\MainMenu.xaml line 20
                {
                    this.CurrentUserFullName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // View\MainMenu.xaml line 21
                {
                    this.MemberListView = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 6: // View\MainMenu.xaml line 22
                {
                    this.RegisterMemberButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.RegisterMemberButton).Click += this.RegisterMemberButton_Click;
                }
                break;
            case 7: // View\MainMenu.xaml line 24
                {
                    this.DeleteMemberButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.DeleteMemberButton).Click += this.DeleteMemberButton_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

