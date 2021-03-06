using RentMeRentalSystem.DAL;
using RentMeRentalSystem.Model;
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
    public sealed partial class MemberUpdate : Page
    {
        CustomerDAL DataAccess = new();
        Customer CustomerToUpdate;

        public MemberUpdate()
        {
            InitializeComponent();
            PopulateStateComboBox();
            PopulateData();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCustomerInformation();
            DataAccess.UpdateCustomer(CustomerToUpdate);
            this.Frame.Navigate(typeof(MemberMenu));
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

        private void PopulateData()
        {
            foreach (Customer customer in CurrentUser.Customers)
            {
                if (customer.IdNumber.Equals(CurrentUser.SelectedMemberId))
                {
                    CustomerToUpdate = customer;
                    FNameTextBox.Text = customer.Fname;
                    LNameTextBox.Text = customer.Lname;
                    GenderComboBox.SelectedIndex = GetGenderComboBoxIndex(customer.Gender.ToString());
                    PhoneNumberTextBox.Text = customer.PhoneNumber;
                    AddressTextBox.Text = customer.AddressLine1;
                    AddressLine2TextBox.Text = customer.AddressLine2;
                    CityTextBox.Text = customer.City;
                    StateComboBox.SelectedValue = customer.State;
                    ZipCodeTextBox.Text = customer.Zipcode;
                    BirthdayDatePicker.Date = customer.Birthdate;
                }
            }
        }

        private void UpdateCustomerInformation()
        {
            CustomerToUpdate.Fname = FNameTextBox.Text;
            CustomerToUpdate.Lname = LNameTextBox.Text;
            CustomerToUpdate.Gender = (Gender)Enum.Parse(typeof(Gender), GenderComboBox.SelectionBoxItem.ToString());
            CustomerToUpdate.PhoneNumber = PhoneNumberTextBox.Text;
            CustomerToUpdate.AddressLine1 = AddressTextBox.Text;
            CustomerToUpdate.AddressLine2 = AddressLine2TextBox.Text;
            CustomerToUpdate.City = CityTextBox.Text;
            CustomerToUpdate.State = StateComboBox.SelectionBoxItem.ToString();
            CustomerToUpdate.Zipcode = ZipCodeTextBox.Text;
            CustomerToUpdate.Birthdate = BirthdayDatePicker.Date.DateTime;
        }

        private int GetGenderComboBoxIndex(string gender)
        {
            var genderDict = new Dictionary<string, int>();
            genderDict.Add("Male", 0);
            genderDict.Add("Female", 1);
            genderDict.Add("Other", 2);

            return genderDict[gender];
        }

        private void PopulateStateComboBox()
        {
            this.StateComboBox.ItemsSource = USStates.States();
        }
    }
}
