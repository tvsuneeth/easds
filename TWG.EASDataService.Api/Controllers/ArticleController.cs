using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Syndication;
using WebApi.OutputCache.V2;
using TWG.EASDataService.Api.Extensions;
using TWG.EASDataService.Services;
using TWG.EASDataService.Business;
using TWG.EASDataService.Api.Models;
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.Controllers
{
    public class ArticleController : ApiController
    {
        private IArticleService _articleService;
        private IUrlHelper _urlHelper;
        private IStaticContentLinkService _staticContentLinkService;
        public ArticleController(IArticleService articleService, IStaticContentLinkService staticContentLinkService, IUrlHelper urlHelper)
        {
            _articleService = articleService;
            _urlHelper = urlHelper;
            _staticContentLinkService = staticContentLinkService;
        }

        [HttpGet]
        [Route("article/{id:int}", Name = "GetArticleById")]
        [Authorize(Roles = "frontofficegroup")]
       // [CacheOutput(ClientTimeSpan=600, ServerTimeSpan=3600, AnonymousOnly=false)]
        public SingleContentFeed<Article> Get(int id)
        {
            
            _urlHelper.RouteHelper = Url;
            
            var article = _articleService.GetById(id);            
            if (article != null)
            {
                if (article.ThumbnailImage != null)
                {
                    article.ThumbnailImage.Url = _urlHelper.GenerateUrl("GetMediaContentById", new { id = article.ThumbnailImage.Id });
                }
                var articleFeed = new SingleContentFeed<Article>(
                    _urlHelper.GenerateUrl("GetArticleById", new { id = article.Id }),
                    article,
                    _urlHelper,
                    _staticContentLinkService                  
                );

                return articleFeed;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("{page:int?}", Name = "GetRoot")]
        [Authorize(Roles = "frontofficegroup")]
       // [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 1200, AnonymousOnly = false)]
        public MultipleContentFeed<ArticleSummary> GetAll(int page = 1)
        
        {
            _urlHelper.RouteHelper = Url;

            var paginatedArticleSummaries = _articleService.GetAll(page, 20);
            if (paginatedArticleSummaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<ArticleSummary>(
                    _urlHelper.GenerateUrl("GetRoot", new { page }),
                    String.Format("All page {0}", page),
                    paginatedArticleSummaries,
                    _urlHelper,
                    _staticContentLinkService,
                    "GetArticleById"
                );

                if (paginatedArticleSummaries.HasMultiplePage)
                {
                    if (paginatedArticleSummaries.HasNextPage)
                    {
                        contentFeed.SetNextLink("GetRoot", String.Format("All page {0}", paginatedArticleSummaries.NextPage), new { page = paginatedArticleSummaries.NextPage });
                    }
                    if (paginatedArticleSummaries.HasPreviousPage)
                    {
                        contentFeed.SetPreviousLink("GetRoot", String.Format("All page {0}", paginatedArticleSummaries.PreviousPage), new { page = paginatedArticleSummaries.PreviousPage });
                    }
                    contentFeed.SetFirstLink("GetRoot", String.Format("All page {0}", paginatedArticleSummaries.FirstPage), new { page = paginatedArticleSummaries.FirstPage });
                    contentFeed.SetLastLink("GetRoot", String.Format("All page {0}", paginatedArticleSummaries.LastPage), new { page = paginatedArticleSummaries.LastPage });
                }

                return contentFeed;
            }
            else
            {
                return null;
            }
        }

        
        [HttpGet]
        [Route("secret/allarticles/{page:int?}", Name = "Secret-GetAllArticles")]
        public  MultipleContentFeed<ArticleSummary> GetAllArticlesTest(int page = 1)
        {
            return GetAll(page);
        }


        [HttpGet]
        [Route("articles/changedsince/{dateString:regex(\\d{6}_\\d{6})}", Name = "GetArticlesChangedSince")]
        [Authorize(Roles = "frontofficegroup")]        
        //[CacheOutput(NoCache=true  )]
        public List<ArticleModificationSummary> GetChangedArticles(string dateString)
        {
            //date format should be yyyymmdd_hhmmss
            DateTime dt = dateString.GetDateFromString();
            return _articleService.GetChangedArticles(dt);            
        }
            

    }
}
