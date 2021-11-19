using System;

namespace RentMeRentalSystem.Model
{
    public abstract class Transaction
    {
        #region Properties

        public string TransactionId { get; set; }

        public string EmployeeId { get; set; }

        public string CustomerId { get; set; }

        public double Fee { get; set; }

        public DateTime DueDate { get; set; }

        #endregion
    }
}