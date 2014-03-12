using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace twg.chk.DataService.DbContext
{
    public class UserContext : IdentityDbContext<IdentityUser>, IDisposable
    {
        public UserContext() : this("ChkDataServiceContext") { }
        public UserContext(String connectionStringOrConnectionName) : base(connectionStringOrConnectionName) { }

        void IDisposable.Dispose()
        {
            base.Dispose();
        }
    }
}
