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

namespace DataService.Tests.RestfulDataService.FrontOffice.UnitTests
{
    [TestClass]
    public class ArticleControllerTests
    {
        private ArticleController _objectUnderTest;
        private IArticleRepository _articleRepository;
        private IArticleService _articleService;

        [TestInitialize]
        public void Setup()
        {
            _articleRepository = MockRepository.GenerateStub<IArticleRepository>();
            _articleService = new ArticleService(_articleRepository);
            _objectUnderTest = new ArticleController(_articleService);

            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void Get_GetExistingArticleById()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article());

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<Article>));
        }

        [TestMethod]
        public void Get_GetNonExistingArticleById()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(null);

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NotFound, httpMessageArticle.StatusCode);
        }
    }
}
