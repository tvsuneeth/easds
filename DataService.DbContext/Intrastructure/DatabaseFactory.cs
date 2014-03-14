using System;

namespace twg.chk.DataService.DbContext.Intrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private DataServiceEntities dataContext;
        public DataServiceEntities Get()
        {
            return dataContext ?? (dataContext = new DataServiceEntities());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
