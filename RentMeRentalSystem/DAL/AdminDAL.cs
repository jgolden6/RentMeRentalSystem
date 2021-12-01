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
                da.Fill(table);
            }

            return table;
        }

        public DataTable ViewReportBetweenTwoDates(string startDate, string endDate)
        {
            DataTable table = new DataTable();
            string query = $"select c.customerId, concat(c.fname, ' ', c.lname) as fullName, c.phoneNumber, t.transactionId, " +
                $"rt.rentalDate, t.fee, ri.furnitureId, ri.quantity, f.categoryName, f.styleName, f.daily_rental_rate " +
                $"from customer as c, transaction as t, rental_transaction as rt, rental_item as ri, furniture as f " +
                $"where c.customerId = t.customerId and t.transactionId = rt.rentalId and t.transactionId = ri.rentalId " +
                $"and ri.furnitureId = f.furnitureId and rt.rentalDate between {startDate} and {endDate}";

            using (var da = new MySqlDataAdapter(query, Connection.connectionString))
            {
                da.Fill(table);
            }

            return table;
        }
    }
}
