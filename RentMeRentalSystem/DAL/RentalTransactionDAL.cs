using System;
using System.Collections.Generic;
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

        public bool CreateRentalTransaction(JsonArray rentalBase, JsonArray rentalItems)
        {
            var success = this.createRentalTransaction(rentalBase, rentalItems);
            return success;
        }

        private bool createRentalTransaction(JsonArray rentalBase, JsonArray rentalItems)
        {
            if (!rentalBase.GetArray().Any() || !rentalItems.GetArray().Any())
            {
                return false;
            }

            using (var conn = new MySqlConnection(Connection.connectionString))
            {
                conn.Open();
                using (var cmd =
                    new MySqlCommand("create_rental_transaction", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rental", rentalBase.GetArray().ToString());
                    cmd.Parameters.AddWithValue("@rental_items", rentalItems.GetArray().ToString());
                    cmd.Parameters.AddWithValue("@rental_date", DateTimeOffset.Now.Date.ToString("yyyy-MM-dd"));
                    var myTrans = conn.BeginTransaction();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        myTrans.Commit();
                        Console.WriteLine("Transaction Succeeded");
                        return true;
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Transaction Failed");
                        Console.WriteLine(ex.Message);
                        try
                        {
                            myTrans.Rollback();
                            Console.WriteLine("Transaction rolled back");
                        }
                        catch (MySqlException exi)
                        {
                            Console.WriteLine("Transaction Not Rolled");
                            Console.WriteLine(exi.Message);
                        }
                    }

                    return false;
                }
            }
        }

        public void UpdateFurnitureQuantities()
        {
            using (var conn = new MySqlConnection(Connection.connectionString))
            {
                conn.Open();
                using (var cmd =
                    new MySqlCommand("call update_furniture_quantities()", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetRentalTransactionInformation()
        {
            var table = new DataTable();
            var query =
                "call retrieve_recent_rental_transaction()";
            using (var da = new MySqlDataAdapter(query, Connection.connectionString))
            {
                da.Fill(table);
            }

            return table;
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
                    new MySqlCommand("call calculate_rental_transaction_cost(@items, @rental_date)", conn))
                {
                    cmd.Parameters.Add("@items", MySqlDbType.Text);
                    cmd.Parameters["@items"].Value = items.GetArray().ToString();
                    cmd.Parameters.Add("@rental_date", MySqlDbType.Date);
                    cmd.Parameters["@rental_date"].Value = DateTimeOffset.Now.Date.ToString("yyyy-MM-dd");
                    using (var reader = cmd.ExecuteReader())
                    {
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
            using (var conn = new MySqlConnection(Connection.connectionString))
            {
                conn.Open();
                var query = "call retrieve_transactions_of_type(@type); ";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add("@type", MySqlDbType.VarChar);
                cmd.Parameters["@type"].Value = TransactionType.Rental.ToString().ToLower();
                using var reader = cmd.ExecuteReader();
                var retrieved = this.helpRetrieveTransactions(reader, TransactionType.Rental);
                return retrieved;
            }
        }

        public List<Transaction> RetrieveCustomerRentalTransactions(string customerId)
        {
            using (var conn = new MySqlConnection(Connection.connectionString))
            {
                conn.Open();
                var query = "call retreive_customer_transactions(@customerId, @type); ";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add("@type", MySqlDbType.VarChar);
                cmd.Parameters["@type"].Value = TransactionType.Rental.ToString().ToLower();
                cmd.Parameters.Add("@customerId", MySqlDbType.Int64);
                cmd.Parameters["@customerId"].Value = int.Parse(customerId);
                using var reader = cmd.ExecuteReader();
                var retrieved = this.helpRetrieveTransactions(reader, TransactionType.Rental);
                return retrieved;
            }
        }

        private List<Transaction> helpRetrieveTransactions(MySqlDataReader reader, TransactionType type)
        {
            var retrieved = new List<Transaction>();
            var transactionIdOrdinal = reader.GetOrdinal("transactionId");
            var employeeIdOrdinal = reader.GetOrdinal("employeeId");
            var customerIdOrdinal = reader.GetOrdinal("customerId");
            var feeOrdinal = reader.GetOrdinal("fee");
            var dueDateOrdinal = reader.GetOrdinal("dueDate");
            var transactionDateOrdinal = reader.GetOrdinal("rentalDate");
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

        #endregion
    }
}