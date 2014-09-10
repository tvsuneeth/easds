using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Repository;
using TWG.EASDataService.Services;
using TWG.EASDataService.Api.Controllers;
using TWG.EASDataService.Api.Models;
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.Tests
{
    [TestClass]
    public class StaticPageControllerTests
    {
        private StaticPageController _objectUnderTest;
        private IStaticPageRepository _staticPageRepository;
        private IStaticPageService _staticPageService;
        private IStaticContentLinkService _staticContentLinkService;
        private IUrlHelper _urlHelper;

        [TestInitialize]
        public void Setup()
        {
            _staticPageRepository = MockRepository.GenerateStub<IStaticPageRepository>();
            _urlHelper = MockRepository.GenerateStub<IUrlHelper>();
            _staticContentLinkService = MockRepository.GenerateStub<IStaticContentLinkService>();
            _staticPageService = new StaticPageService(_staticPageRepository);
            _objectUnderTest = new StaticPageController(_staticPageService, _staticContentLinkService, _urlHelper);

            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void GetAllStaticPages_ReturnsAListofStaticPages()
        {
            List<StaticPageSummary> list = new List<StaticPageSummary>()
            {
                {new StaticPageSummary(){ Id=1, PageName="test1"}},
                {new StaticPageSummary(){ Id=2, PageName="test2"}}
            };
            _staticPageRepository.Stub(r => r.GetAll()).Return(list);

            var staticPages = _objectUnderTest.GetAllStaticPages();

            Assert.IsNotNull(staticPages);
        }


        [TestMethod]
        public void Get_RequestExistingStaticPage()
        {
            _staticPageRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new StaticPage());
            _urlHelper.Stub(h => h.GenerateUrl(Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var staticPageFeed = _objectUnderTest.GetById(100);

            Assert.IsNotNull(staticPageFeed);
        }

        [TestMethod]
        public void Get_RequestNonExistingStaticpage()
        {
            _staticPageRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(null);
            _urlHelper.Stub(h => h.GenerateUrl(Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var staticPageFeed = _objectUnderTest.GetById(-1);

            Assert.IsNull(staticPageFeed);
        }
    }
}
