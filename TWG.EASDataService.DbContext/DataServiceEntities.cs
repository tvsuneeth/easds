using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

using TWG.EASDataService.Business;

namespace TWG.EASDataService.DbContext
{
    public class DataServiceEntities : IdentityDbContext<IdentityUser>, IDisposable
    {
        public IDbSet<StaticContentLink> StaticContentLinks { get; set; }

        public DataServiceEntities() : this("ChkDataServiceContext") {}
        public DataServiceEntities(String connectionStringOrConnectionName) : base(connectionStringOrConnectionName) {}

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        void IDisposable.Dispose()
        {
            base.Dispose();
        }
    }
}
