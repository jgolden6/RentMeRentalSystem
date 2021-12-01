using System;

namespace RentMeRentalSystem.Model
{
    public class Transaction
    {
        #region Properties

        public string TransactionId { get; set; }

        public string EmployeeId { get; set; }

        public string CustomerId { get; set; }

        public double Fee { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime TransactionDate { get; set; }

        public bool IsChecked { get; set; }

        public string StringValue => $"{TransactionId}, {EmployeeId}, {CustomerId}, {Fee}, {DueDate.ToShortDateString()}, {TransactionDate.ToShortDateString()}";
        #endregion
    }
}