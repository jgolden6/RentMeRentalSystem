using RentMeRentalSystem.DAL;
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
            ResetDataRows();
            var customerData = DataAccess.SearchForCustomer(SearchCriteriaComboBox.SelectionBoxItem.ToString(), SearchInformationTextBox.Text);
            foreach (string dataLine in customerData)
            {
                MemberInformationGridView.Items.Add(dataLine);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MemberMenu));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            this.Frame.Navigate(typeof(LoginMenu));
        }

        private void ResetDataRows()
        {
            MemberInformationGridView.Items.Clear();
            MemberInformationGridView.Items.Add("First Name");
            MemberInformationGridView.Items.Add("Last Name");
            MemberInformationGridView.Items.Add("ID");
            MemberInformationGridView.Items.Add("Gender");
            MemberInformationGridView.Items.Add("Birthdate");
            MemberInformationGridView.Items.Add("Phone Number");
            MemberInformationGridView.Items.Add("Address 1");
            MemberInformationGridView.Items.Add("Address 2");
            MemberInformationGridView.Items.Add("Zip Code");
            MemberInformationGridView.Items.Add("City");
            MemberInformationGridView.Items.Add("State");
            MemberInformationGridView.Items.Add("Registration Date");
        }
    }
}
