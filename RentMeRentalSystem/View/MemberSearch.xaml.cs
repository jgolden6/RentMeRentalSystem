﻿using RentMeRentalSystem.DAL;
using RentMeRentalSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RentMeRentalSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberSearch : Page
    {
        CustomerDAL DataAccess = new();

        public MemberSearch()
        {
            this.InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MemberInformationListView.Items.Clear();
            var customerData = DataAccess.SearchForCustomer(SearchCriteriaComboBox.SelectionBoxItem.ToString(), SearchInformationTextBox.Text);
            foreach (string dataLine in customerData)
            {
                MemberInformationListView.Items.Add(dataLine);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            this.Frame.Navigate(typeof(LoginMenu));
        }
    }
}