using System;
namespace twg.chk.DataService.DbContext
{
    public class Context : System.Data.Entity.DbContext
    {
        public Context(): base("ChkDataServiceContext")
        {
        }
    }
}
