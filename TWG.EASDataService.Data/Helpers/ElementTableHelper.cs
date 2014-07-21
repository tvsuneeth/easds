using System;
using System.Data;
using System.Data.SqlClient;

namespace TWG.EASDataService.Data.Helpers
{
    public class ElementTableHelper
    {
        public static DataTable BuidTable(String[] nameArray)
        {
            var table = new DataTable();
            table.Columns.Add("Name", typeof(String));

            foreach(var el in nameArray ?? new String[0])
            {
                table.Rows.Add(el);
            }

            return table;
        }
    }
}
