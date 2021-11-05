using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RentMeRentalSystem.Annotations;
using RentMeRentalSystem.DAL;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.ViewModel
{
    internal class InventoryMenuViewModel : INotifyPropertyChanged
    {
        #region Data members

        private bool selected;

        private ObservableCollection<Furniture> furnitureItems;

        private readonly FurnitureDAL dataAccess = new();

        #endregion

        #region Properties

        public bool Selected
        {
            get => this.selected;
            set
            {
                this.selected = value;
                this.NotifyPropertyChanged(nameof(Selected));
            }
        }

        /// <summary>
        ///     Gets or sets the furniture items.
        /// </summary>
        /// <value>
        ///     The furnitureItems.
        /// </value>
        public ObservableCollection<Furniture> FurnitureItems
        {
            get => this.furnitureItems;

            set
            {
                this.furnitureItems = value;
                this.NotifyPropertyChanged(nameof(FurnitureItems));
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

        #endregion

        #region Constructors

        public InventoryMenuViewModel()
        {
            this.resetFurnitureItems();
            this.Categories = this.dataAccess.RetrieveCategories();
            this.Styles = this.dataAccess.RetrieveStyles();
        }

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged = delegate {  };

        [NotifyPropertyChangedInvocator]
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RetrieveFurnitureById(string furnitureId)
        {
            this.FurnitureItems = new ObservableCollection<Furniture>(this.dataAccess.RetrieveSingleFurnitureItemById(int.Parse(furnitureId)));
        }

        public void RetrieveFurnitureByCategory(string category)
        {
            this.FurnitureItems =  new ObservableCollection<Furniture>(this.dataAccess.RetrieveFurnitureItemsByCategory(category));
        }

        public void RetrieveFurnitureByStyle(string style)
        {
            this.FurnitureItems =  new ObservableCollection<Furniture>(this.dataAccess.RetrieveFurnitureItemsByStyle(style));
        }

        public void resetFurnitureItems()
        {
            this.FurnitureItems = new ObservableCollection<Furniture>(this.dataAccess.RetrieveFurnitureItems());
        }

        #endregion
    }
}