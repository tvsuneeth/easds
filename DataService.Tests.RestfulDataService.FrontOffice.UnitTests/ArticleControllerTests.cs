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
        private IUrlHelper _urlHelper;
        private ITaxonomyRepository _taxonomyRepository;

        [TestInitialize]
        public void Setup()
        {
            _articleRepository = MockRepository.GenerateStub<IArticleRepository>();
            _articleTaxonomyRepository = MockRepository.GenerateStub<IArticleTaxonomyRepository>();
            _urlHelper = MockRepository.GenerateStub<IUrlHelper>();
            _taxonomyRepository = MockRepository.GenerateStub<ITaxonomyRepository>();
            _articleService = new ArticleService(_articleRepository, _articleTaxonomyRepository, _taxonomyRepository);
            _objectUnderTest = new ArticleController(_articleService, _urlHelper);

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
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<SingleContentFeed<Article>>));
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
            _urlHelper.Stub(h => h.GenerateUrl(Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);

            var articleContent = httpMessageArticle.Content as ObjectContent<SingleContentFeed<Article>>;
            var articleFeed = articleContent.Value as SingleContentFeed<Article>;
            Assert.IsNotNull(articleFeed.Parents);
        }

        [TestMethod]
        public void Get_ExistingArticleHasImage()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article { ThumbnailImage = new MediaContent { Title = "image file", Extension = "jpg", FileName = "imagefile.jpg" } });
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyItem>());

            var httpMessageArticle = _objectUnderTest.Get(1);

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);

            var articleContent = httpMessageArticle.Content as ObjectContent<SingleContentFeed<Article>>;
            var articleFeed = articleContent.Value as SingleContentFeed<Article>;
            Assert.IsNotNull(articleFeed.Entry.ThumbnailImage);
        }
    }
}
