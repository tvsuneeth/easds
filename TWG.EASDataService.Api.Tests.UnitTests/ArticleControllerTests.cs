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
using TWG.EASDataService.DbContext.Repository;
using TWG.EASDataService.Services;
using TWG.EASDataService.Api.Controllers;
using TWG.EASDataService.Api.Models;
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.Tests.UnitTests
{
    [TestClass]
    public class ArticleControllerTests
    {
        private ArticleController _objectUnderTest;
        private IArticleRepository _articleRepository;
        private IArticleTaxonomyRepository _articleTaxonomyRepository;
        private IArticleService _articleService;
        private IStaticContentLinkService _staticContentLinkService;
        private IUrlHelper _urlHelper;
        private ITaxonomyRepository _taxonomyRepository;

        [TestInitialize]
        public void Setup()
        {
            _articleRepository = MockRepository.GenerateStub<IArticleRepository>();
            _articleTaxonomyRepository = MockRepository.GenerateStub<IArticleTaxonomyRepository>();
            _staticContentLinkService = MockRepository.GenerateStub<IStaticContentLinkService>();
            _urlHelper = MockRepository.GenerateStub<IUrlHelper>();
            _taxonomyRepository = MockRepository.GenerateStub<ITaxonomyRepository>();
            _articleService = new ArticleService(_articleRepository, _articleTaxonomyRepository, _taxonomyRepository);
            _objectUnderTest = new ArticleController(_articleService, _staticContentLinkService, _urlHelper);

            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void  ArticleController_Get_ReturnsFeedWithRequestedArticle()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article() { Id=1 });
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyCategory>());
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.Get(1);
            
            var article = (Article)articleFeed.Entries.FirstOrDefault().Content;           
            Assert.AreEqual(1, article.Id);
        }

        [TestMethod]
        public void ArticleController_GetAll_ReturnsFeedWithTwentyArticles()
        {
            PagedResult<ArticleSummary> paginatedArticleSummaries = new PagedResult<ArticleSummary>(1, 20, 21);
            List<ArticleSummary> lists = new List<ArticleSummary>() 
            {
                { new ArticleSummary(){ Id=1} },
                { new ArticleSummary(){ Id=2} },
                { new ArticleSummary(){ Id=3} },
                { new ArticleSummary(){ Id=4} },
                { new ArticleSummary(){ Id=5} },
                { new ArticleSummary(){ Id=6} },
                { new ArticleSummary(){ Id=7} },
                { new ArticleSummary(){ Id=8} },
                { new ArticleSummary(){ Id=9} },
                { new ArticleSummary(){ Id=10} },
                { new ArticleSummary(){ Id=11} },
                { new ArticleSummary(){ Id=12} },
                { new ArticleSummary(){ Id=13} },
                { new ArticleSummary(){ Id=14} },
                { new ArticleSummary(){ Id=15} },
                { new ArticleSummary(){ Id=16} },
                { new ArticleSummary(){ Id=17} },
                { new ArticleSummary(){ Id=18} },
                { new ArticleSummary(){ Id=19} },
                { new ArticleSummary(){ Id=20} }
                

            };
            paginatedArticleSummaries.AddRange(lists);
            

            _articleRepository.Stub(r => r.GetAll(null,null,null,1,20)).Return(paginatedArticleSummaries);           
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.GetAll(1);

            var articleCount = articleFeed.Entries.Count();
            Assert.AreEqual(20, articleCount);
        }

        [TestMethod]
        public void ArticleController_GetModifiedArticles_ReturnsListOfModifiedArticles()
        {
            List<ArticleModificationSummary> modifiedArticleList = new List<ArticleModificationSummary>() 
            {
                {new ArticleModificationSummary() { Id = 1, LastModified = DateTime.Now }},
                {new ArticleModificationSummary() { Id = 2, LastModified = DateTime.Now }}
            };

            _articleRepository.Stub(r => r.GetChangedArticles(Arg<DateTime>.Is.Anything)).Return(modifiedArticleList);

            string input = "20140101_000000";
            var articles = _objectUnderTest.GetChangedArticles(input);

            Assert.IsNotNull(articles.Count==2);
        }
       


        [TestMethod]
        public void ArticleController_Get_ArticleWithTaxonomy_ReturnedArticleHasTaxonomyInfo()
        {
            TaxonomyCategory category =   new TaxonomyCategory() {  CategoryId=1, CategoryName="ArticleSection"  };            
            category.AddItem(new TaxonomyCategoryItem() { CategoryItemId = 1, CategoryItemName = "test", ParentItemId = null });
            List<TaxonomyCategory> categories = new List<TaxonomyCategory>() {category };     

            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(categories);
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article());            
            _urlHelper.Stub(h => h.GenerateUrl(Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.Get(1);

            var article = (Article)articleFeed.Entries.FirstOrDefault().Content;                        
            Assert.IsNotNull(article.Taxonomy);
        }

        [TestMethod]
        public void ArticleController_Get_ArticleWithImage_ReturnedArticleHasImage()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article { ThumbnailImage = new Image { Extension = "jpg", Name = "imagefile.jpg" } });
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyCategory>());
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.Get(1);

            Assert.IsNotNull(articleFeed);
            Article article = (Article)articleFeed.Entries.FirstOrDefault().Content;

            Assert.IsNotNull(article.ThumbnailImage);
        }
    }
}
