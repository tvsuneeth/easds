using System;
using System.Collections.Generic;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Repository;

namespace TWG.EASDataService.Services
{
    public interface IStaticPageService
    {       
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
