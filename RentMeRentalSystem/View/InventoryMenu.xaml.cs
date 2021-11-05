using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using RentMeRentalSystem.DAL;
using RentMeRentalSystem.Model;
using RentMeRentalSystem.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RentMeRentalSystem.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InventoryMenu : Page
    {
        #region Data members

        private InventoryMenuViewModel viewModel;

        #endregion

        #region Constructors

        public InventoryMenu()
        {
            this.InitializeComponent();
            this.viewModel = new InventoryMenuViewModel();
            this.DataContext = this.viewModel;
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

        private void CreateRentalTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.validateSearch())
            {
                this.ErrorText.Text = string.Empty;
                if (this.FurnitureIdTextBox.IsEnabled && this.validateFurnitureId())
                {
                    this.viewModel.RetrieveFurnitureById(this.FurnitureIdTextBox.Text);
                } else if (this.CategoryComboBox.IsEnabled && this.CategoryComboBox.SelectionBoxItem != null)

                {
                    this.viewModel.RetrieveFurnitureByCategory(this.CategoryComboBox.SelectionBoxItem.ToString());
                } else if (this.StyleComboBox.IsEnabled && this.StyleComboBox.SelectionBoxItem != null)
                {
                    this.viewModel.RetrieveFurnitureByStyle(this.StyleComboBox.SelectionBoxItem.ToString());
                }
            }
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.resetFurnitureItems();
            this.FurnitureIdTextBox.Text = string.Empty;
            this.CategoryComboBox.SelectedItem = null;
            this.StyleComboBox.SelectedItem = null;
            this.FurnitureIdTextBox.IsEnabled = true;
            this.CategoryComboBox.IsEnabled = true;
            this.StyleComboBox.IsEnabled = true;
            this.ErrorText.Text = string.Empty;
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
            this.Frame.Navigate(typeof(LoginMenu));
        }

        #endregion
    }
}