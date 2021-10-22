using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace RentMeRentalSystem.Extensions
{
    public static class DataReaderExtensions
    {
        #region Data members

        private static List<int> ordinals;

        #endregion

        #region Methods

        public static List<T> ExecuteReader<T>(this IDataRecord reader, MySqlCommand command, string query, string givenOrdinals,
            Dictionary<MySqlParameter, object> parameters)
        {
            command.setCommandParameters(parameters);
            DataReaderExtensions.setOrdinals(reader, givenOrdinals);
            return null;
        }

        private static void setOrdinals(IDataRecord reader, string givenOrdinals)
        {
            string[] ordinalsSplit = givenOrdinals.Split(",");
            foreach (var ordinal in ordinalsSplit)
            {
                DataReaderExtensions.ordinals.Add(reader.GetOrdinal(ordinal));
            }
        }

        

        #endregion
    }
}