using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using Windows.Data.Json;
using RentMeRentalSystem.Annotations;
using RentMeRentalSystem.DAL;
using RentMeRentalSystem.Model;
using RentMeRentalSystem.View;

namespace RentMeRentalSystem.ViewModel
{
    internal class InventoryMenuViewModel : INotifyPropertyChanged
    {
        #region Data members

        private bool selected;

        private double costFee;

        private Dictionary<string, FurnitureListItem> furnitureItems;

        private readonly FurnitureDAL furnitureDataAccess = new();

        private readonly CustomerDAL customerDataAccess = new();

        private readonly RentalTransactionDAL rentalDataAccess = new();

        #endregion

        #region Properties

        public DataTable TransactionInformation { get; set; }

        public bool Selected
        {
            get => this.selected;
            set
            {
                this.selected = value;
                this.NotifyPropertyChanged(nameof(this.Selected));
            }
        }

        /// <summary>
        ///     Gets or sets the furniture items.
        /// </summary>
        /// <value>
        ///     The furnitureItems.
        /// </value>
        public ObservableCollection<FurnitureListItem> FurnitureItems
        {
            get => new(this.furnitureItems.Values);

            set
            {
                this.furnitureItems = this.convertObservableCollectionToFurnitureDictionary(value);
                this.NotifyPropertyChanged(nameof(this.FurnitureItems));
            }
        }

        /// <summary>
        ///     Gets or sets the categories.
        /// </summary>
        /// <value>
        ///     The categories.
        /// </value>
        public List<string> Categories { get; set; }

        /// <summary>
        ///     Gets or sets the styles.
        /// </summary>
        /// <value>
        ///     The styles.
        /// </value>
        public List<string> Styles { get; set; }

        public string CustomerId { get; set; }

        public string Cost { get; set; }

        public DateTimeOffset? DueDate { get; set; }

        #endregion

        #region Constructors

        public InventoryMenuViewModel()
        {
            this.ResetFurnitureItems();
            this.Categories = this.furnitureDataAccess.RetrieveCategories();
            this.Styles = this.furnitureDataAccess.RetrieveStyles();
            this.Cost = "Cost: $0.00";
        }

        #endregion

        #region

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [NotifyPropertyChangedInvocator]
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool CreateRentalTransaction(string employeeId)
        {
            var rentalItems = new JsonArray();
            foreach (var item in this.FurnitureItems)
            {
                if (item.SelectedQuantity != 0 && item.IsChecked)
                {
                    var newItem = new JsonObject();
                    newItem.Add("furnitureId", JsonValue.CreateNumberValue(int.Parse(item.FurnitureId)));
                    newItem.Add("qty", JsonValue.CreateNumberValue(item.SelectedQuantity));

                    rentalItems.Add(newItem);
                }
            }
            var rental = new JsonArray();
            var rentalTransaction = new JsonObject();
            rentalTransaction.Add("employeeId",  JsonValue.CreateNumberValue(int.Parse(employeeId)));
            rentalTransaction.Add("customerId",  JsonValue.CreateNumberValue(int.Parse(this.CustomerId)));
            rentalTransaction.Add("fee",  JsonValue.CreateNumberValue(this.costFee));
            this.DueDate ??= DateTimeOffset.Now.AddDays(2);
            rentalTransaction.Add("transactionDueDate", JsonValue.CreateStringValue(this.DueDate.Value.Date.ToString("yyyy-MM-dd")));
            rental.Add(rentalTransaction);
            var result = this.rentalDataAccess.CreateRentalTransaction(rental, rentalItems);
            if (result)
            {
                this.TransactionInformation = this.rentalDataAccess.GetRentalTransactionInformation();
                this.rentalDataAccess.UpdateFurnitureQuantities();
            }
            return result;
        }


        public string RetrieveCustomer()
        {
            DataTable customer = this.customerDataAccess.SearchForCustomer("Member ID", this.CustomerId);
            return $"{customer.Rows[0][0].ToString()} {customer.Rows[0][1].ToString()} {customer.Rows[0][2].ToString()}";
        }

        public void CalculateTransactionCost()
        {
            var items = this.groupFurnitureItemsForTransaction();
            this.costFee = this.rentalDataAccess.CalculateRentalTransactionCost(items);
            this.Cost = $"Cost: " + Math.Round(this.costFee, 2, MidpointRounding.AwayFromZero).ToString("C2");
        }

        private JsonArray groupFurnitureItemsForTransaction()
        {
            var items = new JsonArray();
            foreach (var item in this.FurnitureItems)
            {
                if (item.SelectedQuantity != 0 && item.IsChecked)
                {
                    var newItem = new JsonObject();
                    newItem.Add("id", JsonValue.CreateNumberValue(int.Parse(item.FurnitureId)));
                    newItem.Add("qty", JsonValue.CreateNumberValue(item.SelectedQuantity));
                    this.DueDate ??= DateTimeOffset.Now.AddDays(2);
                    newItem.Add("dueDate", JsonValue.CreateStringValue(this.DueDate.Value.Date.ToString("yyyy-MM-dd")));

                    items.Add(newItem);
                }
            }

            return items;
        }

        public void RetrieveFurnitureById(string furnitureId)
        {
            var retrievedFurnitureItems = this.convertListFurnitureItemsToListFurnitureListItems(
                this.furnitureDataAccess.RetrieveSingleFurnitureItemById(int.Parse(furnitureId)));
            this.FurnitureItems = retrievedFurnitureItems;
        }

        public void RetrieveFurnitureByCategory(string category)
        {
            var retrievedFurnitureItems =
                this.convertListFurnitureItemsToListFurnitureListItems(
                    this.furnitureDataAccess.RetrieveFurnitureItemsByCategory(category));
            this.FurnitureItems = retrievedFurnitureItems;
        }

        public void ResolveCheckedItemsWhenSearching(ObservableCollection<FurnitureListItem> checkedItems)
        {
            foreach(var item in checkedItems) {
                foreach(var item2 in this.FurnitureItems) {
                    if (item.FurnitureId.Equals(item2.FurnitureId) && item.IsChecked)
                    {
                        item2.SelectedQuantity = item.SelectedQuantity;
                        item2.IsChecked = item.IsChecked;
                    }
                }
            }
        }

        public void ClearItemSelectionsAndQuantities(ObservableCollection<FurnitureListItem> checkedItems)
        {
            foreach(var item in checkedItems) {
                foreach(var item2 in this.FurnitureItems) {
                    if (item.FurnitureId.Equals(item2.FurnitureId) && item.IsChecked)
                    {
                        item2.SelectedQuantity = 0;
                        item2.IsChecked = false;
                    }
                }
            }
        }

        /// <summary>
        ///     Retrieves the furniture by style.
        /// </summary>
        /// <param name="style">The style.</param>
        public void RetrieveFurnitureByStyle(string style)
        {
            var retrievedFurnitureItems =
                this.convertListFurnitureItemsToListFurnitureListItems(
                    this.furnitureDataAccess.RetrieveFurnitureItemsByStyle(style));
            this.FurnitureItems = retrievedFurnitureItems;
        }

        /// <summary>
        ///     Resets the furniture items.
        /// </summary>
        public void ResetFurnitureItems()
        {
            var retrievedFurnitureItems =
                this.convertListFurnitureItemsToListFurnitureListItems(this.furnitureDataAccess.RetrieveFurnitureItems());
            this.FurnitureItems = retrievedFurnitureItems;
        }

        private ObservableCollection<FurnitureListItem> convertListFurnitureItemsToListFurnitureListItems(
            List<Furniture> items)
        {
            var convertedItems = new ObservableCollection<FurnitureListItem>();
            foreach (var item in items)
            {
                FurnitureListItem existingItem = null;
                var itemExists = this.furnitureItems != null &&
                                 this.furnitureItems.TryGetValue(item.FurnitureId, out existingItem);
                var isChecked = false;
                var selectedQuantity = 0;
                if (itemExists)
                {
                    isChecked = existingItem.IsChecked;
                    selectedQuantity = existingItem.SelectedQuantity;
                }

                var listItem = new FurnitureListItem {
                    FurnitureId = item.FurnitureId, CategoryName = item.CategoryName, StyleName = item.StyleName,
                    DailyRentalRate = item.DailyRentalRate, Quantity = item.Quantity, IsChecked = isChecked,
                    SelectedQuantity = selectedQuantity
                };
                convertedItems.Add(listItem);
            }

            return convertedItems;
        }

        private Dictionary<string, FurnitureListItem> convertObservableCollectionToFurnitureDictionary(
            ObservableCollection<FurnitureListItem> items)
        {
            var convertedItems = new Dictionary<string, FurnitureListItem>();
            foreach (var item in items)
            {
                convertedItems.Add(item.FurnitureId, item);
            }

            return convertedItems;
        }

        #endregion
    }
}