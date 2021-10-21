using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.DAL
{
    class EmployeeDAL
    {

        public int Authenticate(string username, string empPassword)
        {
            using (MySqlConnection conn = new MySqlConnection(Connection.connectionString))
            {
                conn.Open();
                string query = " select count(*) from employee where username = @username and empPassword = @empPassword ";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", MySqlDbType.VarChar);
                    cmd.Parameters["@username"].Value = username;
                    cmd.Parameters.Add("@empPassword", MySqlDbType.VarChar);
                    cmd.Parameters["@empPassword"].Value = empPassword;
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count;
                }
            }
        }

    }
}
