using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

using twg.chk.DataService.Business;

namespace twg.chk.DataService.DbContext
{
    public class DataServiceEntities : IdentityDbContext<IdentityUser>, IDisposable
    {
        public IDbSet<StaticContentLink> StaticContentLinks { get; set; }

        static DataServiceEntities()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataServiceEntities, Migrations.Configuration>());
        }

        public DataServiceEntities() : this("ChkDataServiceContext") {}
        public DataServiceEntities(String connectionStringOrConnectionName) : base(connectionStringOrConnectionName) {}

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }

        void IDisposable.Dispose()
        {
            base.Dispose();
        }
    }
}
