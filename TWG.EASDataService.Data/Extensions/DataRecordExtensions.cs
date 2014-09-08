using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Data.Extensions
{    
    public static class DataRecordExtensions
    {
        public static T GetValue<T>(this IDataRecord reader, string columnName)
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
