using System.Collections.Generic;
using System.Linq;
using DBAccess.DAL;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.DAL
{
    internal class FurnitureDAL
    {
        #region Methods

        public List<Furniture> RetrieveFurnitureItems()
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();

            var query =
                "select * from furniture";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            List<Furniture>  retrieved = this.helpRetrieveFurnitureItems(reader);
            return retrieved;
        }

        public List<Furniture> RetrieveSingleFurnitureItemById(int furnitureIdentification)
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();

            var query =
                "select * from furniture where furnitureId = @furnitureIdentification";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@furnitureIdentification", MySqlDbType.Int32);
            cmd.Parameters["@furnitureIdentification"].Value = furnitureIdentification;
            using var reader = cmd.ExecuteReader();
            List<Furniture>  retrieved = this.helpRetrieveFurnitureItems(reader);
            return retrieved;
        }

        public List<Furniture> RetrieveFurnitureItemsByCategory(string category)
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();

            var query =
                "select * from furniture where categoryName = @category";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@category", MySqlDbType.VarChar);
            cmd.Parameters["@category"].Value = category;
            using var reader = cmd.ExecuteReader();
            List<Furniture>  retrieved = this.helpRetrieveFurnitureItems(reader);
            return retrieved;
        }


        public List<Furniture> RetrieveFurnitureItemsByStyle(string style)
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();

            var query =
                "select * from furniture where styleName = @style";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@style", MySqlDbType.VarChar);
            cmd.Parameters["@style"].Value = style;
            using var reader = cmd.ExecuteReader();
            List<Furniture> retrieved = this.helpRetrieveFurnitureItems(reader);
            return retrieved;
        }

        private List<Furniture> helpRetrieveFurnitureItems(MySqlDataReader reader)
        {
            var retrieved = new List<Furniture>();
            var furnitureIdOrdinal = reader.GetOrdinal("furnitureId");
            var categoryNameOrdinal = reader.GetOrdinal("categoryName");
            var styleNameOrdinal = reader.GetOrdinal("styleName");
            var dailyRentalRateOrdinal = reader.GetOrdinal("daily_rental_rate");
            var quantityOrdinal = reader.GetOrdinal("quantity");
            while (reader.Read())
            {
                var furnitureId = reader.GetFieldValueCheckNull<int>(furnitureIdOrdinal).ToString();
                var categoryName = reader.GetFieldValueCheckNull<string>(categoryNameOrdinal);
                var styleName = reader.GetFieldValueCheckNull<string>(styleNameOrdinal);
                var dailyRentalRate = reader.GetFieldValueCheckNull<decimal>(dailyRentalRateOrdinal);
                var quantity = reader.GetFieldValueCheckNull<int>(quantityOrdinal);
                retrieved.Add(new Furniture {
                    FurnitureId = furnitureId, CategoryName = categoryName, StyleName = styleName,
                    DailyRentalRate = decimal.ToDouble(dailyRentalRate), Quantity = quantity
                });
            }

            return retrieved;
        }

        public List<string> RetrieveCategories()
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            var query =
                "select * from category";
            using var cmd = new MySqlCommand(query, conn);
            var retrieved = new List<string>();
            using var reader = cmd.ExecuteReader();
            var categoryNameOrdinal = reader.GetOrdinal("categoryName");
            while (reader.Read())
            {
                var categoryName = reader.GetFieldValueCheckNull<string>(categoryNameOrdinal);
                retrieved.Add(categoryName);
            }
            return retrieved;
        }

        public List<string> RetrieveStyles()
        {
            using var conn = new MySqlConnection(Connection.connectionString);
            conn.Open();
            var query =
                "select * from style";
            using var cmd = new MySqlCommand(query, conn);
            var retrieved = new List<string>();
            using var reader = cmd.ExecuteReader();
            var styleNameOrdinal = reader.GetOrdinal("styleName");
            while (reader.Read())
            {
                var styleName = reader.GetFieldValueCheckNull<string>(styleNameOrdinal);
                retrieved.Add(styleName);
            }
            return retrieved;
        }

        #endregion
    }
}