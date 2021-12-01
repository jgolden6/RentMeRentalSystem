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
    internal class TransactionMenuViewModel : INotifyPropertyChanged
    {
        #region Data members

        private bool selected;

        public static  ObservableCollection<FurnitureListItem> RentalFurnitureItems;

        private  ObservableCollection<Transaction> transactionItems;

        private readonly CustomerDAL customerDataAccess = new();

        public readonly RentalTransactionDAL RentalDataAccess = new();

        public readonly ReturnTransactionDAL ReturnDataAccess = new();

        public ObservableCollection<string> TransactionTypes => new ObservableCollection<string>() {TransactionType.Rental.ToString(), TransactionType.Return.ToString()};

        #endregion

        #region Properties

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
        ///     Gets or sets the transaction items.
        /// </summary>
        /// <value>
        ///     The transactionItems.
        /// </value>
        public ObservableCollection<Transaction> TransactionItems
        {
            get => new(this.transactionItems);

            set
            {
                this.transactionItems = value;
                this.NotifyPropertyChanged(nameof(this.TransactionItems));
            }
        }

        public string CustomerId { get; set; }

        public string Cost { get; set; }


        #endregion

        #region Constructors

        public TransactionMenuViewModel()
        {
            this.transactionItems = new ObservableCollection<Transaction>();
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

        public string RetrieveCustomer()
        {
            DataTable customer = this.customerDataAccess.SearchForCustomer("Member ID", this.CustomerId);
            return $"{customer.Rows[0][0].ToString()} {customer.Rows[0][1].ToString()} {customer.Rows[0][2].ToString()}";
        }

        public void RetrieveCustomerRentalTransactions()
        {
            this.TransactionItems =
                new ObservableCollection<Transaction>(
                    this.RentalDataAccess.RetrieveCustomerRentalTransactions(this.CustomerId));
        }

        public void RetrieveCustomerReturnTransactions()
        {
            this.TransactionItems =
                new ObservableCollection<Transaction>(
                    this.ReturnDataAccess.RetrieveCustomerReturnTransactions(this.CustomerId));
        }

        #endregion
    }
}