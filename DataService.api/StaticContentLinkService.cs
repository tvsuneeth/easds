using System;
using System.Collections.Generic;

using twg.chk.DataService.Business;
using twg.chk.DataService.DbContext.Repository;

namespace twg.chk.DataService.api
{
    public interface IStaticContentLinkService
    {
        IEnumerable<StaticContentLink> GetStaticContentLinkForSite();
    }

    public class StaticContentLinkService : IStaticContentLinkService
    {
        private IStaticContentLinkRepository _staticContentLinkRepository;
        public StaticContentLinkService(IStaticContentLinkRepository staticContentLinkRepository)
        {
            _staticContentLinkRepository = staticContentLinkRepository;
        }

        public IEnumerable<StaticContentLink> GetStaticContentLinkForSite()
        {
            return _staticContentLinkRepository.GetSiteAll();
        }
    }
}
