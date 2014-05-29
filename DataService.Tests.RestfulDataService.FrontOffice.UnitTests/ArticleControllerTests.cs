﻿using System;
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
using twg.chk.DataService.DbContext.Repository;
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
        public void Get_GetExistingArticleById()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article());
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyItem>());
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.Get(1);

            Assert.IsNotNull(articleFeed);
        }

        [TestMethod]
        public void Get_GetNonExistingArticleById()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(null);
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.Get(1);

            Assert.IsNull(articleFeed);
        }

        [TestMethod]
        public void Get_ExistingArticleHasTaxonomy()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article());
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyItem> { new TaxonomyItem { Id = 1, Category = TaxonomyCategories.ArticleSection, Name = "sample"}});
            _urlHelper.Stub(h => h.GenerateUrl(Arg<String>.Is.Anything, Arg<Object>.Is.Anything)).Return("http://dummylink.co.uk");
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.Get(1);

            Assert.IsNotNull(articleFeed);

           // Assert.IsNotNull(articleFeed.Parents);
        }

        [TestMethod]
        public void Get_ExistingArticleHasImage()
        {
            _articleRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new Article { AttachedMedia = new MediaContent { Extension = "jpg", FileName = "imagefile.jpg" } });
            _articleTaxonomyRepository.Stub(r => r.Get(Arg<int>.Is.Anything)).Return(new List<TaxonomyItem>());
            _staticContentLinkService.Stub(s => s.GetStaticContentLinkForSite()).Return(new List<StaticContentLink>());

            var articleFeed = _objectUnderTest.Get(1);

            Assert.IsNotNull(articleFeed);
            MediaContent content = null;
            foreach (var item in articleFeed.Entries)
            {
                content = ((IMediaAttachment)item.Content).AttachedMedia;
            }


            Assert.IsNotNull(content);
        }
    }
}
