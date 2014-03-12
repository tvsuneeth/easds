using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using twg.chk.DataService.DbContext;

namespace twg.chk.DataService.Tests.FakeDatabase
{
    public class FakeDatabase
    {
        private SqlCeConnectionStringBuilder _connection;
        public FakeDatabase(String databaseFileName)
        {
            _connection = new SqlCeConnectionStringBuilder(String.Format("Datasource={0}", databaseFileName));
        }

        public void Create()
        {
            Remove();
            using (var userContext = new UserContext(_connection.ConnectionString))
            {
                userContext.Database.Create();
            }
        }

        public void Remove()
        {
            using (var userContext = new UserContext(_connection.ConnectionString))
            {
                userContext.Database.Delete();
            }
        }
    }
}
