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
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.UnitTests
{
    [TestClass]
    public class TaxonomyControllerTests
    {
        private TaxonomyController _objectUnderTest;
        private IArticleRepository _articleRepository;
        private IArticleService _articleService;
        private IStaticContentLinkService _staticContentLinkService;
        private IArticleTaxonomyRepository _articleTaxonomyRepository;
        private ITaxonomyRepository _taxonomyRepository;
        private IUrlHelper _urlHelper;

        [TestInitialize]
        public void Setup()
        {
            _articleRepository = MockRepository.GenerateStub<IArticleRepository>();
             _staticContentLinkService = MockRepository.GenerateStub<IStaticContentLinkService>();
             _articleTaxonomyRepository = MockRepository.GenerateStub<IArticleTaxonomyRepository>();
            _taxonomyRepository = MockRepository.GenerateStub<ITaxonomyRepository>();
            _urlHelper = MockRepository.GenerateStub<IUrlHelper>();
            _articleService = new ArticleService(_articleRepository,_articleTaxonomyRepository,_taxonomyRepository );
            _objectUnderTest = new TaxonomyController(_articleService, _staticContentLinkService, _urlHelper);

            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void TaxonomyController_GetAllTaxonomyCategories_ReturnsListOfTaxonomyCategoriesAndItems()
        {
            TaxonomyCategory category = new TaxonomyCategory() { CategoryId = 1, CategoryName = "ArticleSection" };
            category.AddItem(new TaxonomyCategoryItem() { CategoryItemId = 1, CategoryItemName = "test", ParentItemId = null });
            List<TaxonomyCategory> categories = new List<TaxonomyCategory>() { category };
            _taxonomyRepository.Stub(s => s.GetAllTaxonomyCategoriesAndItems()).Return(categories);

            var list = _objectUnderTest.GetAllTaxonomyCategories();
            Assert.IsNotNull(list);
        }

       // [TestMethod]
        public void Get_GetBySector()
        {
           /* _articleRepository.Stub(r => r.GetBySector(Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetSector("dummy_sector");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
            * */
        }

        //[TestMethod]
        public void Get_GetByTopic()
        {
            /*
            _articleRepository.Stub(r => r.GetByTopic(Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetTopic("dummy_topics");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
             */
        }

        //[TestMethod]
        public void Get_GetByArticleSection()
        {
          /*
            _articleRepository.Stub(r => r.GetByArticleSection(Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetArticleSection("dummy_article-section");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
           * */
        }

        //[TestMethod]
        public void Get_GetByArticleSectionAndSector()
        {
            /*
            _articleRepository.Stub(r => r.GetByArticleSectionAndSector(Arg<String[]>.Is.Anything, Arg<String[]>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything, out Arg<int>.Out(10).Dummy))
                .Return(new List<Article> { new Article() });

            var httpMessageArticle = _objectUnderTest.GetArticleSectionAndSector("dummy_article-section", "dummy_sector");

            Assert.IsNotNull(httpMessageArticle);
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, httpMessageArticle.StatusCode);
            Assert.IsInstanceOfType(httpMessageArticle.Content, typeof(ObjectContent<IEnumerable<Article>>));
             */
        }
    }
}
