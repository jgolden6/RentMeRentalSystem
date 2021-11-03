﻿using RentMeRentalSystem.DAL;
using RentMeRentalSystem.Model;
using RentMeRentalSystem.View;
using RentMeRentalSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RentMeRentalSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        CustomerDAL DataAccess = new CustomerDAL();

        public MainMenu()
        {
            this.InitializeComponent();
            CurrentUser.Customers = DataAccess.RetrieveCustomers();
        }

        private void RegisterMemberButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MemberRegistration));
        }

        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            this.Frame.Navigate(typeof(LoginMenu));
        }
    }
}
