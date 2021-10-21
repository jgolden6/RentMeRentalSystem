using RentMeRentalSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class MemberRegistration : Page
    {
        bool DataValidated = true;

        public MemberRegistration()
        {
            this.InitializeComponent();
            this.PopulateStateComboBox();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateAllData();

            if (DataValidated)
            {
                this.Frame.Navigate(typeof(MainMenu));
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void PopulateStateComboBox()
        {
            this.StateComboBox.ItemsSource = USStates.States();
        }

        private void ValidateAllData()
        {
            DataValidated = true;
            ValidateFirstName();
            ValidateLastName();
            ValidateGender();
            ValidatePhoneNumber();
            ValidateAddress();
            ValidateCity();
            ValidateState();
            ValidateZipCode();
            ValidateBirthday();
        }

        private void ValidateFirstName()
        {
            if (!Regex.Match(FNameTextBox.Text, "^[A-Za-z]+$").Success)
            {
                FNameTextBox.Text = "";
                FNameTextBox.PlaceholderText = "Invalid first name";
                DataValidated = false;
            }
        }

        private void ValidateLastName()
        {
            if (!Regex.Match(LNameTextBox.Text, "^[A-Za-z]+$").Success)
            {
                LNameTextBox.Text = "";
                LNameTextBox.PlaceholderText = "Invalid last name";
                DataValidated = false;
            }
        }

        private void ValidateGender()
        {
            if (GenderComboBox.SelectedItem == null)
            {
                GenderComboBox.PlaceholderText = "Please select a gender";
                DataValidated = false;
            }
        }

        private void ValidatePhoneNumber()
        {
            if (!Regex.Match(PhoneNumberTextBox.Text, "^[0-9]{10}$").Success)
            {
                PhoneNumberTextBox.Text = "";
                PhoneNumberTextBox.PlaceholderText = "Invalid phone number";
                DataValidated = false;
            }
        }

        private void ValidateAddress()
        {
            if (AddressTextBox.Text.Equals(""))
            {
                AddressTextBox.PlaceholderText = "Please enter an address";
                DataValidated = false;
            }
        }


        private void ValidateCity()
        {
            if (!Regex.Match(CityTextBox.Text, "^[A-Za-z]+$").Success)
            {
                CityTextBox.Text = "";
                CityTextBox.PlaceholderText = "Invalid city";
                DataValidated = false;
            }
        }


        private void ValidateState()
        {
            if (StateComboBox.SelectedItem == null)
            {
                StateComboBox.PlaceholderText = "Please select a state";
                DataValidated = false;
            }
        }

        private void ValidateZipCode()
        {
            if (!Regex.Match(ZipCodeTextBox.Text, "^[0-9]{5}$").Success)
            {
                ZipCodeTextBox.Text = "";
                ZipCodeTextBox.PlaceholderText = "Invalid zip code";
                DataValidated = false;
            }
        }

        private void ValidateBirthday()
        {
            if (BirthdayDatePicker.SelectedDate == null)
            {
                DataValidated = false;
            }
        }
    }
}
