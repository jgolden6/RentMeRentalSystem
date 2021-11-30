using System;
using System.Collections.Generic;
using System.Linq;
using DBAccess.DAL;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.DAL
{
    internal class EmployeeDAL
    {
        #region Methods

        public Employee Authenticate(string username, string password) 
        {
            if (!this.CheckPassword(username, password))
            {
                return null;
            }

            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            var query = "select fname, lname, employeeId from employee where username = @username";
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add("@username", MySqlDbType.VarChar);
            cmd.Parameters["@username"].Value = username;

            var retrieved = new List<Employee>();
            using var reader = cmd.ExecuteReader();
            var fnameOrdinal = reader.GetOrdinal("fname");
            var lnameOrdinal = reader.GetOrdinal("lname");
            var idOrdinal = reader.GetOrdinal("employeeId");

            while (reader.Read())
            {
                var fname = reader.GetFieldValueCheckNull<string>(fnameOrdinal);
                var lname = reader.GetFieldValueCheckNull<string>(lnameOrdinal);
                var employeeId = reader.GetFieldValueCheckNull<int>(idOrdinal).ToString();
                retrieved.Add(new Employee { Fname = fname, Lname = lname, IdNumber = employeeId , Username = username});
            }

            conn.Close();
            reader.Close();
            return retrieved.Any() ? retrieved[0] : null;
        }

        private bool CheckPassword(string username, string password)
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            var query = $"select empPassword from employee where username = @username";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@username", MySqlDbType.VarChar);
            cmd.Parameters["@username"].Value = username;

            using var reader = cmd.ExecuteReader();
            var empPasswordOrdinal = reader.GetOrdinal("empPassword");
            var empPassword = "";

            while (reader.Read())
            {
                empPassword = reader.GetFieldValueCheckNull<string>(empPasswordOrdinal);
            }

            return SecurePasswordHasher.Verify(password, empPassword);
        }


        #endregion
    }
}