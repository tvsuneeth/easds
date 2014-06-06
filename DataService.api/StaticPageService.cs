using System;
using System.Collections.Generic;
using twg.chk.DataService.Business;
using twg.chk.DataService.chkData.Repository;

namespace twg.chk.DataService.api
{
    public interface IStaticPageService
    {
        StaticPage GetByName(String staticPageName);
        StaticPage GetById(int staticPageId);
        List<StaticPageSummary> GetAll();
    }

    public class StaticPageService : IStaticPageService
    {
        private IStaticPageRepository _staticPageRepository;
        public StaticPageService(IStaticPageRepository staticPageRepository)
        {
            _staticPageRepository = staticPageRepository;
        }

        public StaticPage GetByName(string staticPageName)
        {
            return _staticPageRepository.Get(staticPageName);
        }

        public StaticPage GetById(int staticPageId)
        {
            return _staticPageRepository.Get(staticPageId);
        }

        public List<StaticPageSummary> GetAll()
        {
            return _staticPageRepository.GetAll();
        }

    }
}
