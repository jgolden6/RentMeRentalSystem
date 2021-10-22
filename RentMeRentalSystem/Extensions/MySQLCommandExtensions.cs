using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RentMeRentalSystem.Extensions
{
    public static class MySQLCommandExtensions
    {
        public static void setCommandParameters(this MySqlCommand command, Dictionary<MySqlParameter, object> parameters)
        {
            command.Parameters.AddRange(parameters.Keys.ToArray());
            foreach (var parameter in parameters.Keys)
            {
                command.Parameters[parameter.ParameterName].Value = parameters[parameter];
            }
        }
    }
}
