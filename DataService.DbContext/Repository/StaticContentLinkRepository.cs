using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using twg.chk.DataService.DbContext.Intrastructure;
using twg.chk.DataService.Business;

namespace twg.chk.DataService.DbContext.Repository
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
