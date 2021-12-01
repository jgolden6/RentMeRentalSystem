using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using RentMeRentalSystem.Model;
using RentMeRentalSystem.ViewModel;
using System.Text.RegularExpressions;

namespace RentMeRentalSystem.View
{
    /// <summary>
    ///     The furniture inventory page.
    /// </summary>
    public sealed partial class InventoryMenu : Page
    {
        #region Data members

        private readonly InventoryMenuViewModel viewModel;

        private ObservableCollection<FurnitureListItem> checkedItems;

        private ObservableCollection<FurnitureListItem> checkedItemsPreSearch;

        private static DateTimeOffset DEFAULT_DATE = new DateTimeOffset(DateTime.Now.AddDays(2));

        #endregion

        #region Constructors

        public InventoryMenu()
        {
            this.InitializeComponent();
            this.viewModel = new InventoryMenuViewModel();
            DataContext = this.viewModel;
            this.Cost.Text = this.viewModel.Cost;
            this.RentalDatePicker.Date = InventoryMenu.DEFAULT_DATE;
            this.RentalDatePicker.MinDate = InventoryMenu.DEFAULT_DATE;
        }

        #endregion

        #region Methods

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CategoryComboBox.SelectedItem != null)
            {
                this.StyleComboBox.IsEnabled = false;
                this.FurnitureIdTextBox.IsEnabled = false;
            }
            else
            {
                this.StyleComboBox.IsEnabled = true;
                this.FurnitureIdTextBox.IsEnabled = true;
            }
        }

        private void StyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.StyleComboBox.SelectedItem != null)
            {
                this.CategoryComboBox.IsEnabled = false;
                this.FurnitureIdTextBox.IsEnabled = false;
            }
            else
            {
                this.CategoryComboBox.IsEnabled = true;
                this.FurnitureIdTextBox.IsEnabled = true;
            }
        }

        private void FurnitureIdTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (this.FurnitureIdTextBox.Text.Any())
            {
                this.CategoryComboBox.IsEnabled = false;
                this.StyleComboBox.IsEnabled = false;
            }
            else
            {
                this.CategoryComboBox.IsEnabled = true;
                this.StyleComboBox.IsEnabled = true;
            }
        }

        private void FurnitureTextBox_BeforeTextChanging(TextBox sender,
            TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private async void CreateRentalTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.viewModel.CreateRentalTransaction(this.CurrentUserId.Text))
            {
                var success = new RentalTransactionConfirmationDialog();
                DataGridFiller.FillDataGrid(this.viewModel.TransactionInformation, success.TransactionInfoGrid);
                success.TotalCost.Text = this.viewModel.Cost;
                await success.ShowAsync();
                this.viewModel.ResetFurnitureItems();
                this.viewModel.ClearItemSelectionsAndQuantities(this.checkedItems);
                this.viewModel.Cost = "Cost: $0.00";
                this.ErrorText.Text = string.Empty;
                this.helpResetCustomer();
                this.RentalDatePicker.Date = InventoryMenu.DEFAULT_DATE;
            }
            else
            {
                this.ErrorText.Text = "Transaction Denied";
            }

        }

        private void ListItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.setPreviewCost();
            this.checkedItems = this.viewModel.FurnitureItems;
        }

        private void ListItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.setPreviewCost();
            this.checkedItems = this.viewModel.FurnitureItems;
        }

        private void ListItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.setPreviewCost();
        }

        private void setPreviewCost()
        {
            this.viewModel.CalculateTransactionCost();
            this.Cost.Text = this.viewModel.Cost;
            if (this.Cost.Text.Equals("Cost: $0.00") || this.Customer.Text.Equals("CustomerId:"))
            {
                this.CreateRentalTransactionButton.IsEnabled = false;
            }
            else
            {
                this.CreateRentalTransactionButton.IsEnabled = true;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.validateSearch())
            {
                this.checkedItemsPreSearch = this.viewModel.FurnitureItems;
                this.ErrorText.Text = string.Empty;
                if (this.FurnitureIdTextBox.IsEnabled && this.validateFurnitureId())
                {
                    this.viewModel.RetrieveFurnitureById(this.FurnitureIdTextBox.Text);
                }
                else if (this.CategoryComboBox.IsEnabled && this.CategoryComboBox.SelectionBoxItem != null)
                {
                    this.viewModel.RetrieveFurnitureByCategory(this.CategoryComboBox.SelectionBoxItem.ToString());
                }
                else if (this.StyleComboBox.IsEnabled && this.StyleComboBox.SelectionBoxItem != null)
                {
                    this.viewModel.RetrieveFurnitureByStyle(this.StyleComboBox.SelectionBoxItem.ToString());
                }
            }
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.ResetFurnitureItems();
            this.FurnitureIdTextBox.Text = string.Empty;
            this.CategoryComboBox.SelectedItem = null;
            this.StyleComboBox.SelectedItem = null;
            this.FurnitureIdTextBox.IsEnabled = true;
            this.CategoryComboBox.IsEnabled = true;
            this.StyleComboBox.IsEnabled = true;
            this.ErrorText.Text = string.Empty;
            this.viewModel.ResolveCheckedItemsWhenSearching(this.checkedItemsPreSearch);
        }

        private void RentalDatePicker_DateChanged(CalendarDatePicker sender,
            CalendarDatePickerDateChangedEventArgs args)
        {
            var hasItems = this.FurnitureListView.Items != null;
            if (this.RentalDatePicker.Date < DateTime.Now.Date)
            {
                this.ErrorText.Text = "Invalid Due Date. Due Date cannot be before today.";
                this.CreateRentalTransactionButton.IsEnabled = false;
                this.FurnitureListView.IsEnabled = false;
            }
            else
            {
                this.ErrorText.Text = string.Empty;
                this.viewModel.DueDate = this.RentalDatePicker.Date;
                this.setPreviewCost();
                this.FurnitureListView.IsEnabled = true;
            }
        }

        private bool validateFurnitureId()
        {
            var dataValidated = true;
            if (!Regex.Match(this.FurnitureIdTextBox.Text, "^[0-9]{5}$").Success)
            {
                this.ErrorText.Text = "Incorrect Furniture ID. Please input a five digit number";
                this.FurnitureIdTextBox.Text = string.Empty;
                dataValidated = false;
            }

            return dataValidated;
        }

        private bool validateSearch()
        {
            var searchValidated = true;
            if (!this.FurnitureIdTextBox.Text.Any() && this.CategoryComboBox.SelectedItem == null &&
                this.StyleComboBox.SelectedItem == null)
            {
                this.ErrorText.Text =
                    "Please enter a Furniture ID, select a category, or select a style to search the inventory.";
                searchValidated = false;
            }

            return searchValidated;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            Frame.Navigate(typeof(LoginMenu));
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
                this.setPreviewCost();
            }
            catch (Exception)
            {
                this.ErrorText.Text = "Invalid search.";
            }
        }

        private void ResetCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            this.helpResetCustomer();
            this.setPreviewCost();
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

        private void ClearSelectionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.ResetFurnitureItems();
            this.viewModel.ClearItemSelectionsAndQuantities(this.checkedItems);
            this.viewModel.Cost = "Cost: $0.00";
        }

        #endregion
    }
}