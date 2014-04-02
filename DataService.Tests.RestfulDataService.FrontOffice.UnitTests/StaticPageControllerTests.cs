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

using twg.chk.DataService.Business;
using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.api;
using twg.chk.DataService.FrontOffice.Controllers;
using twg.chk.DataService.FrontOffice.Models;
using twg.chk.DataService.FrontOffice.Helpers;

namespace DataService.Tests.RestfulDataService.FrontOffice.UnitTests
{
    [TestClass]
    public class StaticPageControllerTests
    {
        private StaticPageController _objectUnderTest;
        private IStaticPageRepository _staticPageRepository;
        private IStaticPageService _staticPageService;
        private IContentFeedHelper _contentFeedHelper;

        [TestInitialize]
        public void Setup()
        {
            _staticPageRepository = MockRepository.GenerateStub<IStaticPageRepository>();
            _contentFeedHelper = MockRepository.GenerateStub<IContentFeedHelper>();
            _staticPageService = new StaticPageService(_staticPageRepository);
            _objectUnderTest = new StaticPageController(_staticPageService, _contentFeedHelper);

            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }


        [TestMethod]
        public void Get_RequestExisting()
        {
            _staticPageRepository.Stub(r => r.Get(Arg<String>.Is.Anything)).Return(new StaticPage());
            _contentFeedHelper.Stub(h => h.GenerateLink(Arg<System.Web.Http.Routing.UrlHelper>.Is.Anything, Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");

            var httpMessageStaticPage = _objectUnderTest.GetByName("existing_page_name");

            Assert.IsNotNull(httpMessageStaticPage);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageStaticPage.StatusCode);
            Assert.IsInstanceOfType(httpMessageStaticPage.Content, typeof(ObjectContent<ContentFeed<StaticPage>>));
        }

        [TestMethod]
        public void Get_RequestNonExisting()
        {
            _staticPageRepository.Stub(r => r.Get(Arg<String>.Is.Anything)).Return(null);
            _contentFeedHelper.Stub(h => h.GenerateLink(Arg<System.Web.Http.Routing.UrlHelper>.Is.Anything, Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");

            var httpMessageStaticPage = _objectUnderTest.GetByName("nonexisting_page_name");

            Assert.IsNotNull(httpMessageStaticPage);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NotFound, httpMessageStaticPage.StatusCode);
        }
    }
}
