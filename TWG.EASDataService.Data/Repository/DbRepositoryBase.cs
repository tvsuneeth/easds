using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TWG.EASDataService.Data.Repository
{
    public abstract class DbRepositoryBase
    {

        public SqlConnection CreateConnection()
        {
            SqlConnection conn =  new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString);
            conn.Open();
            return conn;
        }

        public SqlCommand CreateCommand(SqlConnection conn, string commandName)
        {
            var command = new SqlCommand(commandName);
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = conn;
            return command;
        }

        public static void AddCommandParameter(SqlCommand command, string name, object value)
        {           
            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
        }

        public T GetValue<T>(object value)
        {
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

        /// <summary>
        /// checks a column exists in a data reader
        /// </summary>
        /// <param name="reader">data reader instance</param>
        /// <param name="columnName">DB column name</param>
        /// <returns></returns>
        private static bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static List<string> GetMatchingColumnsForType(Type T, IDataReader dr)
        {
            List<string> matchingColumns = new List<string>();
            foreach (var prop in T.GetProperties())
            {
                if (!prop.CanWrite)
                {
                    continue;
                }
                for (int i = 0; i < dr.FieldCount; i++)
                {                    
                    if (string.Equals(dr.GetName(i), prop.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingColumns.Add(prop.Name);
                        break;
                    }

                }
            }
            return matchingColumns;
        }

        /// <summary>
        /// gets the data from database, build s a list of objects
        /// </summary>
        /// <typeparam name="T">Type of object that needs to be built</typeparam>
        /// <param name="commandName">stored procedure name</param>
        /// <param name="parameters">stored procedure parameters</param>
        /// <returns></returns>
        public List<T> FillListWithAutoMapping<T>(string commandName, object parameters)
        {
            List<T> list = new List<T>();
            List<string> matchingColumns = new List<string>();
            using (var connection = CreateConnection())
            {
                using (var cmd = CreateCommand(connection, commandName, parameters))
                {                   
                    using (var dr = cmd.ExecuteReader())
                    {
                        matchingColumns = GetMatchingColumnsForType(typeof(T), dr);
                        while (dr.Read())
                        {
                            T obj = CreateObject<T>(dr,matchingColumns);
                            list.Add(obj);
                        }
                        
                    }
                }
            }
            return list;
        }

        public T FillObjectWithAutoMapping<T>(string commandName, object parameters)
        {
            T obj = default(T);
            List<string> matchingColumns = new List<string>();
            using (var connection = CreateConnection())
            {
                using (var cmd = CreateCommand(connection, commandName, parameters))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        matchingColumns = GetMatchingColumnsForType(typeof(T), dr);
                        while (dr.Read())
                        {
                            obj = CreateObject<T>(dr, matchingColumns);                            
                        }

                    }
                }
            }
            return obj;
        }

        private static T CreateObject<T>(IDataRecord dr, List<string> matchingColumns)
        {
            T obj = default(T);
            obj = Activator.CreateInstance<T>();

            foreach (string column in matchingColumns)
            {
                PropertyInfo pi = obj.GetType().GetProperty(column);
                if (!object.Equals(dr[pi.Name], DBNull.Value))
                {
                    pi.SetValue(obj, dr[pi.Name], null);
                }
            }            
            
            return obj;
        }

        public SqlCommand CreateCommand(SqlConnection connection, string commandName, object parameters)        
        {
            SqlCommand cmd = CreateCommand(connection, commandName);
            if (parameters != null)
            {
                Type t = parameters.GetType();
                var properties = t.GetProperties();
                foreach (var prop in properties)
                {
                    object propVal = prop.GetValue(parameters, null);
                    string propName = prop.Name;
                    if (!propName.StartsWith("@"))
                    {
                        propName = "@" + propName;
                    }
                    AddCommandParameter(cmd, propName, propVal);
                }
            }
            return cmd;
        }
        
                           
   }
    
}
