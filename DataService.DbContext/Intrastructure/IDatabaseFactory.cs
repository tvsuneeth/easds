using System;

namespace twg.chk.DataService.DbContext.Intrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DataServiceEntities Get();
    }
}
