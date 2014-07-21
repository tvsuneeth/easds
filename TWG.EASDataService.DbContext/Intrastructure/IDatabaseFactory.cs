using System;

namespace TWG.EASDataService.DbContext.Intrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DataServiceEntities Get();
    }
}
