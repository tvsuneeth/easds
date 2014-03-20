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
    public class TaxonomyControllerTests
    {
        private TaxonomyController _objectUnderTest;
        private IArticleRepository _articleRepository;
        private IArticleService _articleService;

        [TestInitialize]
        public void Setup()
        {
            _articleRepository = MockRepository.GenerateStub<IArticleRepository>();
            _articleService = new ArticleService(_articleRepository);
            _objectUnderTest = new TaxonomyController(_articleService);

            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void Get_GetBySector()
        {
            _articleRepository.Stub(r => r.GetBySector(Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetSector("dummy_sector");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
        }

        [TestMethod]
        public void Get_GetByTopic()
        {
            _articleRepository.Stub(r => r.GetByTopic(Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetTopic("dummy_topics");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
        }

        [TestMethod]
        public void Get_GetByArticleSection()
        {
            _articleRepository.Stub(r => r.GetByArticleSection(Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetArticleSection("dummy_article-section");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
        }

        [TestMethod]
        public void Get_GetByArticleSectionAndSector()
        {
            _articleRepository.Stub(r => r.GetByArticleSectionAndSector(Arg<String[]>.Is.Anything, Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetArticleSectionAndSector("dummy_article-section", "dummy_sector");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
        }
    }
}
