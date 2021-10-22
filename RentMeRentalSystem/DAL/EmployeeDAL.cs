using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.DAL
{
    internal class EmployeeDAL
    {
        #region Methods

        public Employee Authenticate(string username, string password) 
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            string ordinals = "fname, lname, employeeId";
            var query =
                $"select {ordinals} from employee where username = @username and empPassword = @password";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@username", MySqlDbType.VarChar);
            cmd.Parameters["@username"].Value = username;
            cmd.Parameters.Add("@password", MySqlDbType.VarChar);
            cmd.Parameters["@password"].Value = password;
            var retrieved = new List<Employee>();
            using var reader = cmd.ExecuteReader();
            var fnameOrdinal = reader.GetOrdinal("fname");
            var lnameOrdinal = reader.GetOrdinal("lname");
            var idOrdinal = reader.GetOrdinal("employeeId");
            while (reader.Read())
            {
                var fname = !reader.IsDBNull(fnameOrdinal) ? reader.GetString(fnameOrdinal) : null;
                var lname = !reader.IsDBNull(lnameOrdinal) ? reader.GetString(lnameOrdinal) : null;
                var employeeId = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal).ToString() : null;
                retrieved.Add(new Employee { Fname = fname, Lname = lname, IdNumber = employeeId });
            }

            conn.Close();

            return retrieved.Any() ? retrieved[0] : null;
        }


        #endregion
    }
}