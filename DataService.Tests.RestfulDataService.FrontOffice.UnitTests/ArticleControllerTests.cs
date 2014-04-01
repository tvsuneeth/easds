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
    public class ArticleControllerTests
    {
        private ArticleController _objectUnderTest;
        private IArticleRepository _articleRepository;
        private IArticleTaxonomyRepository _articleTaxonomyRepository;
        private IArticleService _articleService;
        private IContentFeedHelper _contentFeedHelper;

        [TestInitialize]
        public void Setup()
        {
            _articleRepository = MockRepository.GenerateStub<IArticleRepository>();
            _articleTaxonomyRepository = MockRepository.GenerateStub<IArticleTaxonomyRepository>();
            _contentFeedHelper = MockRepository.GenerateStub<IContentFeedHelper>();
            _articleService = new ArticleService(_articleRepository, _articleTaxonomyRepository);
            _objectUnderTest = new ArticleController(_articleService, _contentFeedHelper);

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
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyItem>());

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<ContentFeed<Article>>));
        }

        [TestMethod]
        public void Get_GetNonExistingArticleById()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(null);

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NotFound, httpMessageArticle.StatusCode);
        }

        [TestMethod]
        public void Get_ExistingArticleHasTaxonomy()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article());
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyItem> { new TaxonomyItem { Id = 1, Category = TaxonomyCategories.ArticleSection, Name = "sample"}});
            _contentFeedHelper.Stub(h => h.GenerateLink(Arg<System.Web.Http.Routing.UrlHelper>.Is.Anything, Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);

            var articleContent = httpMessageArticle.Content as ObjectContent<ContentFeed<Article>>;
            var articleFeed = articleContent.Value as ContentFeed<Article>;
            Assert.IsNotNull(articleFeed.Parents);
            Assert.AreEqual<int>(1, articleFeed.Parents.Count);
        }

        [TestMethod]
        public void Get_ExistingArticleHasImage()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article { ThumbnailImage = new MediaContent { Title = "image file", Extension = "jpg", FileName = "imagefile.jpg" } });
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyItem>());

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);

            var articleContent = httpMessageArticle.Content as ObjectContent<ContentFeed<Article>>;
            var articleFeed = articleContent.Value as ContentFeed<Article>;
            Assert.IsNotNull(articleFeed.Item.ThumbnailImage);
        }
    }
}
