using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using TWG.EASDataService.DbContext.Intrastructure;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.DbContext.Repository
{
    public interface IStaticContentLinkRepository : IRepository<StaticContentLink>
    {
        IEnumerable<StaticContentLink> GetSiteAll();
    }

    public class StaticContentLinkRepository : RepositoryBase<StaticContentLink>, IStaticContentLinkRepository
    {
        public StaticContentLinkRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) {}

        public IEnumerable<StaticContentLink> GetSiteAll()
        {
            return GetAll();
        }
    }
}
