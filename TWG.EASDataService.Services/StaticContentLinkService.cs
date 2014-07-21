using System;
using System.Collections.Generic;
using PagedList;

using TWG.EASDataService.Business;
using TWG.EASDataService.DbContext.Repository;
using TWG.EASDataService.DbContext.Intrastructure;

namespace TWG.EASDataService.Services
{
    public interface IStaticContentLinkService
    {
        IEnumerable<StaticContentLink> GetStaticContentLinkForSite();
        IPagedList<StaticContentLink> GetPaged(int page, int pageSize);
        void Create(StaticContentLink staticContentLink);
        void Update(StaticContentLink staticContentLink);
        void Remove(int staticContentLinkId);
    }

    public class StaticContentLinkService : IStaticContentLinkService
    {
        private IStaticContentLinkRepository _staticContentLinkRepository;
        private IUnitOfWork _unitOfWork;
        public StaticContentLinkService(IStaticContentLinkRepository staticContentLinkRepository, IUnitOfWork unitOfWork)
        {
            _staticContentLinkRepository = staticContentLinkRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<StaticContentLink> GetStaticContentLinkForSite()
        {
            return _staticContentLinkRepository.GetSiteAll();
        }

        public IPagedList<StaticContentLink> GetPaged(int page, int pageSize)
        {
            return _staticContentLinkRepository.GetPage(new Page(page, pageSize), s => true, order => order.LinkType);
        }

        public void Create(StaticContentLink staticContentLink)
        {
            _staticContentLinkRepository.Add(staticContentLink);
            _unitOfWork.Commit();
        }

        public void Update(StaticContentLink staticContentLink)
        {
            _staticContentLinkRepository.Update(staticContentLink);
            _unitOfWork.Commit();
        }

        public void Remove(int staticContentLinkId)
        {
            var staticContentLink = _staticContentLinkRepository.GetById(staticContentLinkId);
            _staticContentLinkRepository.Delete(staticContentLink);
            _unitOfWork.Commit();
        }
    }
}
