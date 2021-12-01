using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.UI.Xaml.Controls;
using RentMeRentalSystem.Model;
using RentMeRentalSystem.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RentMeRentalSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionsMenu : Page
    {

        private readonly TransactionMenuViewModel viewModel;

        public TransactionsMenu()
        {
            this.InitializeComponent();
            this.viewModel = new();
            DataContext = this.viewModel;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            Frame.Navigate(typeof(LoginMenu));
        }

        private void ClearSelectionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.TransactionItems = new ObservableCollection<Transaction>(this.viewModel.RentalDataAccess.RetrieveAllRentalTransactions());
        }

        private void ViewSelectedTransactionsFurnitureItemsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RentalTransactionFurnitureMenu));
        }

        private void RetrieveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.viewModel.CustomerId = this.CustomerLookupTextBox.Text;
                this.Customer.Text = this.viewModel.RetrieveCustomer();
                this.CustomerLookupTextBox.IsEnabled = false;
                this.CustomerLookupTextBox.Visibility = Visibility.Collapsed;
                this.RetrieveCustomerButton.IsEnabled = false;
                this.RetrieveCustomerButton.Visibility = Visibility.Collapsed;
                this.ResetCustomerButton.IsEnabled = true;
                this.ResetCustomerButton.Visibility = Visibility.Visible;
                this.ErrorText.Text = string.Empty;
            }
            catch (Exception)
            {
                this.ErrorText.Text = "Invalid search.";
            }
        }

        private void ResetCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            this.helpResetCustomer();
        }

        private void helpResetCustomer()
        {
            this.Customer.Text = "CustomerId:";
            this.CustomerLookupTextBox.Text = string.Empty;
            this.CustomerLookupTextBox.IsEnabled = true;
            this.CustomerLookupTextBox.Visibility = Visibility.Visible;
            this.RetrieveCustomerButton.IsEnabled = true;
            this.RetrieveCustomerButton.Visibility = Visibility.Visible;
            this.ResetCustomerButton.IsEnabled = false;
            this.ResetCustomerButton.Visibility = Visibility.Collapsed;
        }

        private void TransactionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TransactionTypeComboBox.SelectedValue == null)
            {
                this.ErrorText.Text = "A transaction type must be selected";
                return;
            }
            this.ErrorText.Text = string.Empty;
            var type = (TransactionType)Enum.Parse(typeof(TransactionType),
                TransactionTypeComboBox.SelectedValue.ToString());
            if (type.Equals(TransactionType.Rental))
            {
                this.viewModel.TransactionItems = new ObservableCollection<Transaction>(this.viewModel.RentalDataAccess.RetrieveAllRentalTransactions());
            } else if (type.Equals(TransactionType.Return))
            {
               // this.viewModel.TransactionItems = new ObservableCollection<Transaction>(this.viewModel.ReturnDataAccess.RetrieveAllReturnTransactions());
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.TransactionTypeComboBox.SelectedValue == null)
            {
                this.ErrorText.Text = "A transaction type must be selected";
                return;
            }
            this.ErrorText.Text = string.Empty;
            var type = (TransactionType)Enum.Parse(typeof(TransactionType),
                TransactionTypeComboBox.SelectedValue.ToString());

            if (this.Customer.Text != "CustomerId:" && type.Equals(TransactionType.Rental))
            {
                this.viewModel.RetrieveCustomerRentalTransactions();
                this.ErrorText.Text = string.Empty;
            } else if (this.Customer.Text != "CustomerId:" && type.Equals(TransactionType.Return))
            {
                this.viewModel.RetrieveCustomerReturnTransactions();
                this.ErrorText.Text = string.Empty;
            }
            else
            {
                this.ErrorText.Text = "The customer must be set first";
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.TransactionItems = new ObservableCollection<Transaction>(this.viewModel.RentalDataAccess.RetrieveAllRentalTransactions());
            this.helpResetCustomer();
        }
    }
}
