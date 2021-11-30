using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Windows.Data.Json;
using DBAccess.DAL;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.DAL
{
    internal class RentalTransactionDAL
    {
        #region Methods

        public bool CreateRentalTransaction(JsonArray items)
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            var query = "call create_transaction(@items)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@items", MySqlDbType.Blob);
            cmd.Parameters["@items"].Value = items;
            //TODO remember to subtract from quantity 
            var numberOfRowsEffected = cmd.ExecuteNonQuery();
            var causedEffect = numberOfRowsEffected > 0;
            return causedEffect;
        }

        public double CalculateRentalTransactionCost(JsonArray items)
        {
            var cost = 0.0m;
            if (!items.GetArray().Any())
            {
                return 0.0;
            }
            using (var conn = new MySqlConnection(Connection.connectionString))
            {
                conn.Open();
                using (var cmd =
                    new MySqlCommand("call calculate_rental_transaction_cost(@items)", conn))
                {
                    cmd.Parameters.Add("@items", MySqlDbType.Text);
                    cmd.Parameters["@items"].Value = items.GetArray().ToString();
                    using (var reader = cmd.ExecuteReader())
                    {
                        //var costOrdinal = reader.GetOrdinal("cost");
                       
                        while (reader.Read())
                        {
                            cost = reader.GetFieldValueCheckNull<decimal>(0);
                        }
                    }
                    return decimal.ToDouble(cost);
                }
            }
        }

        public List<Transaction> RetrieveAllRentalTransactions()
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            var query = "call retrieve_transactions_of_type(@type); ";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@type", MySqlDbType.VarChar);
            cmd.Parameters["@type"].Value = TransactionType.Rental.ToString().ToLower();
            using var reader = cmd.ExecuteReader();
            var retrieved = this.helpRetrieveTransactions(reader, TransactionType.Rental);
            return retrieved;
        }

        private List<Transaction> helpRetrieveTransactions(MySqlDataReader reader, TransactionType type)
        {
            var retrieved = new List<Transaction>();
            var transactionIdOrdinal = reader.GetOrdinal("transactionId");
            var employeeIdOrdinal = reader.GetOrdinal("employeeId");
            var customerIdOrdinal = reader.GetOrdinal("customerId");
            var feeOrdinal = reader.GetOrdinal("fee");
            var dueDateOrdinal = reader.GetOrdinal("dueDate");
            var transactionDateOrdinal = reader.GetOrdinal(this.helpDetermineTransactionTermDateColumn(type));
            while (reader.Read())
            {
                var transactionId = reader.GetFieldValueCheckNull<int>(transactionIdOrdinal).ToString();
                var employeeId = reader.GetFieldValueCheckNull<int>(employeeIdOrdinal).ToString();
                var customerId = reader.GetFieldValueCheckNull<int>(customerIdOrdinal).ToString();
                var fee = reader.GetFieldValueCheckNull<decimal>(feeOrdinal);
                var dueDate = reader.GetFieldValueCheckNull<DateTime>(dueDateOrdinal);
                var transactionDate = reader.GetFieldValueCheckNull<DateTime>(transactionDateOrdinal);
                retrieved.Add(new Transaction {
                    TransactionId = transactionId, EmployeeId = employeeId, CustomerId = customerId,
                    Fee = decimal.ToDouble(fee), DueDate = dueDate, TransactionDate = transactionDate
                });
            }

            return retrieved;
        }

        private string helpDetermineTransactionTermDateColumn(TransactionType type)
        {
            var transactionDateColumn = type switch {
                TransactionType.Rental => "rentalDate",
                TransactionType.Return => "returnDate",
                _ => string.Empty
            };

            return transactionDateColumn;
        }

        #endregion
    }
}