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

        private readonly FurnitureDAL dataAccess = new();

        #endregion

        #region Constructors

        public InventoryMenu()
        {
            this.InitializeComponent();
            CurrentUser.FurnitureItems = this.dataAccess.RetrieveFurnitureItems();
            CurrentUser.Categories = this.dataAccess.RetrieveCategories();
            CurrentUser.Styles = this.dataAccess.RetrieveStyles();
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
                this.retrieveFurnitureById();
                this.retrieveFurnitureByCategory();
                this.retrieveFurnitureByStyle();
            }

            this.FurnitureListView.ItemsSource = CurrentUser.FurnitureItems;
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.FurnitureItems = this.dataAccess.RetrieveFurnitureItems();
            this.FurnitureListView.ItemsSource = CurrentUser.FurnitureItems;
            this.FurnitureIdTextBox.Text = string.Empty;
            this.CategoryComboBox.SelectedItem = null;
            this.StyleComboBox.SelectedItem = null;
            this.FurnitureIdTextBox.IsEnabled = true;
            this.CategoryComboBox.IsEnabled = true;
            this.StyleComboBox.IsEnabled = true;
        }


        private void retrieveFurnitureById()
        {

            if (this.FurnitureIdTextBox.IsEnabled && this.validateFurnitureId())
            {
                CurrentUser.FurnitureItems = this.dataAccess.RetrieveSingleFurnitureItemById(int.Parse(this.FurnitureIdTextBox.Text));
            }

        }

        private void retrieveFurnitureByCategory()
        {

            if (this.CategoryComboBox.IsEnabled && this.CategoryComboBox.SelectionBoxItem != null)
            {
                CurrentUser.FurnitureItems = this.dataAccess.RetrieveFurnitureItemsByCategory(this.CategoryComboBox.SelectionBoxItem.ToString());
            }

        }

        private void retrieveFurnitureByStyle()
        {

            if (this.StyleComboBox.IsEnabled && this.StyleComboBox.SelectionBoxItem != null)
            {
                CurrentUser.FurnitureItems = this.dataAccess.RetrieveFurnitureItemsByStyle(this.StyleComboBox.SelectionBoxItem.ToString());
            }

        }


        private bool validateFurnitureId()
        {
            var dataValidated = true;
            if (!Regex.Match(this.FurnitureIdTextBox.Text, "^[0-9]{5}$").Success)
            {
                this.ErrorText.Text = "Incorrect Furniture ID. Please input a five digit number";
                dataValidated = false;
            }

            return dataValidated;
        }

        private bool validateSearch()
        {
            var searchValidated = true;
            if (!this.FurnitureIdTextBox.Text.Any() && this.CategoryComboBox.SelectionBoxItem == null &&
                this.StyleComboBox.SelectionBoxItem == null)
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