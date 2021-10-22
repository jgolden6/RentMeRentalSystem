using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.DAL
{
    internal class CustomerDAL
    {
        public List<Customer> RetrieveCustomers()
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            var query = $"select * from customer";
            using var cmd = new MySqlCommand(query, conn);
            var retrieved = new List<Customer>();
            using var reader = cmd.ExecuteReader();
            var fnameOrdinal = reader.GetOrdinal("fname");
            var lnameOrdinal = reader.GetOrdinal("lname");
            var idOrindal = reader.GetOrdinal("customerId");
            var genderOrdinal = reader.GetOrdinal("gender");
            var birthdateOrindal = reader.GetOrdinal("birthdate");
            var phoneNumberOrdinal = reader.GetOrdinal("phoneNumber");
            var address1Ordinal = reader.GetOrdinal("addressLine1");
            var address2Ordinal = reader.GetOrdinal("addressLine2");
            var zipcodeOrdinal = reader.GetOrdinal("zipcode");
            var cityOrdinal = reader.GetOrdinal("city");
            var stateOrdinal = reader.GetOrdinal("state");
            var registrationDateOrdinal = reader.GetOrdinal("registrationDate");

            while (reader.Read())
            {
                var fname = !reader.IsDBNull(fnameOrdinal) ? reader.GetString(fnameOrdinal) : null;
                var lname = !reader.IsDBNull(lnameOrdinal) ? reader.GetString(lnameOrdinal) : null;
                var customerId = !reader.IsDBNull(idOrindal) ? reader.GetString(idOrindal) : null;
                var gender = !reader.IsDBNull(genderOrdinal) ? Enum.Parse(typeof(Gender), reader.GetString(genderOrdinal)) : null;
                var birthdate = !reader.IsDBNull(birthdateOrindal) ? reader.GetString(birthdateOrindal) : null;
                var phoneNumber = !reader.IsDBNull(phoneNumberOrdinal) ? reader.GetString(phoneNumberOrdinal) : null;
                var addressLine1 = !reader.IsDBNull(address1Ordinal) ? reader.GetString(address1Ordinal) : null;
                var addressLine2 = !reader.IsDBNull(address2Ordinal) ? reader.GetString(address2Ordinal) : null;
                var zipcode = !reader.IsDBNull(zipcodeOrdinal) ? reader.GetString(zipcodeOrdinal) : null;
                var city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                var state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;
                var registrationDate = !reader.IsDBNull(registrationDateOrdinal) ? reader.GetString(registrationDateOrdinal) : null;

                retrieved.Add(new Customer
                {
                    Fname = fname, Lname = lname, IdNumber = customerId, Gender = (Gender)gender, Birthdate = DateTime.Parse(birthdate),
                    PhoneNumber = phoneNumber, AddressLine1 = addressLine1, AddressLine2 = addressLine2, Zipcode = zipcode,
                    City = city, State = state, RegistrationDate = DateTime.Parse(registrationDate)
                });
            }

            conn.Close();
            return retrieved;
        }
    }
}
