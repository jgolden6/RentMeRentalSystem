using MySql.Data.MySqlClient;
using RentMeRentalSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentMeRentalSystem.Model
{
    class AdminDAL
    {
        public DataTable PoseQuery(string query)
        {
            DataTable table = new DataTable();

            using (var da = new MySqlDataAdapter(query, Connection.connectionString))
            {
                try
                {
                    da.Fill(table);
                }
                catch (MySqlException e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }

            return table;
        }
    }
}
