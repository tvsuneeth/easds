using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Data.Extensions
{
    public static class DataReaderExtensions
    {
        public static T GetValue<T>(this SqlDataReader reader, string columnName)
        {
            object value = reader[columnName];

            if (value == DBNull.Value)
            {
                return default(T);
            }
            if (value is T)
            {
                return (T)value;
            }
            else
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }                

        }

        
    }
   
}
