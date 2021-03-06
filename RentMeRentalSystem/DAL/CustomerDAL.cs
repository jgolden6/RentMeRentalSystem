using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAccess.DAL;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.DAL
{
    internal class CustomerDAL
    {
        public List<Customer> RetrieveCustomers()
        {
            var customerList = new List<Customer>();

            using (var conn = new MySqlConnection(Connection.connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("uspGetCustomers", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var fname = reader.GetString(reader.GetOrdinal("fname"));
                            var lname = reader.GetString(reader.GetOrdinal("lname"));
                            var customerId = reader.GetString(reader.GetOrdinal("customerId"));
                            var gender = Enum.Parse(typeof(Gender), reader.GetString(reader.GetOrdinal("gender")));
                            var birthdate = reader.GetString(reader.GetOrdinal("birthdate"));
                            var phoneNumber = reader.GetString(reader.GetOrdinal("phoneNumber"));
                            var addressLine1 = reader.GetString(reader.GetOrdinal("addressLine1"));
                            var addressLine2 = reader.GetString(reader.GetOrdinal("addressLine2"));
                            var zipcode = reader.GetString(reader.GetOrdinal("zipcode"));
                            var city = reader.GetString(reader.GetOrdinal("city"));
                            var state = reader.GetString(reader.GetOrdinal("state"));
                            var registrationDate = reader.GetString(reader.GetOrdinal("registrationDate"));

                            customerList.Add(new Customer
                            {
                                Fname = fname,
                                Lname = lname,
                                IdNumber = customerId,
                                Gender = (Gender)gender,
                                Birthdate = DateTime.Parse(birthdate),
                                PhoneNumber = phoneNumber,
                                AddressLine1 = addressLine1,
                                AddressLine2 = addressLine2,
                                Zipcode = zipcode,
                                City = city,
                                State = state,
                                RegistrationDate = DateTime.Parse(registrationDate)
                            });
                        }
                    }
                }
            }

            return customerList;
        }

        public void RegisterCustomer(Customer customer)
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO customer(fname,lname,gender,birthdate,phoneNumber,addressLine1,addressLine2,zipcode,city,state,registrationDate) " +
                "VALUES(@fname, @lname, @gender, @birthdate, @phoneNumber, @addressLine1, @addressLine2, @zipcode, @city, @state, @registrationDate)";
            cmd.Parameters.AddWithValue("@fname", customer.Fname);
            cmd.Parameters.AddWithValue("@lname", customer.Lname);
            cmd.Parameters.AddWithValue("@gender", customer.Gender.ToString());
            cmd.Parameters.AddWithValue("@birthdate", customer.Birthdate);
            cmd.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            cmd.Parameters.AddWithValue("@addressLine1", customer.AddressLine1);
            cmd.Parameters.AddWithValue("@addressLine2", customer.AddressLine2);
            cmd.Parameters.AddWithValue("@zipcode", customer.Zipcode);
            cmd.Parameters.AddWithValue("@city", customer.City);
            cmd.Parameters.AddWithValue("@state", customer.State.ToString());
            cmd.Parameters.AddWithValue("@registrationDate", DateTime.Now.Date);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateCustomer(Customer customer)
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE customer SET fname = @fname, lname = @lname, gender = @gender, birthdate = @birthdate, " +
                "phoneNumber = @phoneNumber, addressLine1 = @addressLine1, " +
                "addressLine2 = @addressLine2, zipcode = @zipcode, city = @city, state = @state " +
                "WHERE customerId = @customerToUpdateId";
            cmd.Parameters.AddWithValue("@fname", customer.Fname);
            cmd.Parameters.AddWithValue("@lname", customer.Lname);
            cmd.Parameters.AddWithValue("@gender", customer.Gender.ToString());
            cmd.Parameters.AddWithValue("@birthdate", customer.Birthdate);
            cmd.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            cmd.Parameters.AddWithValue("@addressLine1", customer.AddressLine1);
            cmd.Parameters.AddWithValue("@addressLine2", customer.AddressLine2);
            cmd.Parameters.AddWithValue("@zipcode", customer.Zipcode);
            cmd.Parameters.AddWithValue("@city", customer.City);
            cmd.Parameters.AddWithValue("@state", customer.State.ToString());
            cmd.Parameters.AddWithValue("@customerToUpdateId", customer.IdNumber);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public DataTable SearchForCustomer(string searchCriteria, string searchInformation)
        {
            DataTable table = new DataTable();
            string query = "";

            switch (searchCriteria)
            {
                case "Full Name":
                    var fname = searchInformation.Split(' ')[0].Trim();
                    var lname = searchInformation.Split(' ')[1].Trim();
                    query = $"SELECT * FROM customer WHERE fname = '{fname}' AND lname = '{lname}'";
                    break;
                case "Member ID":
                    query = $"SELECT * FROM customer WHERE customerId = {searchInformation}";
                    break;
                case "Phone Number":
                    query = $"SELECT * FROM customer WHERE phoneNumber = {searchInformation}";
                    break;
            }

            using (var da = new MySqlDataAdapter(query, Connection.connectionString))
            {
                da.Fill(table);
            }

            return table;
        }
    }
}
