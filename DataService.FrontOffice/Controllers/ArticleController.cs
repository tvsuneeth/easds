using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Syndication;
using WebApi.OutputCache.V2;

using twg.chk.DataService.api;
using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice.Models;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Controllers
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
        [CacheOutput(ClientTimeSpan=600, ServerTimeSpan=3600, AnonymousOnly=false)]
        public SingleContentFeed<Article> Get(int id)
        {
            _urlHelper.RouteHelper = Url;
            
            var article = _articleService.GetById(id);
            if (article != null)
            {
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
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 1200, AnonymousOnly = false)]
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
        [Route("articles/modifiedsince/{date:regex(\\d{4}-\\d{2}-\\d{2}_\\d{6})}", Name = "GetArticleModifiedSince")]
        // [Authorize(Roles = "frontofficegroup")]
        // [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 1200, AnonymousOnly = false)]
        public List<ModifiedArticle> GetModifiedArticles(string date)
        {
            //date format should be yyyymmdd_hhmmss

            int year = Convert.ToInt32(date.Substring(0, 4));
            int month = Convert.ToInt32(date.Substring(5, 2));
            int day = Convert.ToInt32(date.Substring(8, 2));
            int hour = Convert.ToInt32(date.Substring(11, 2));
            int minute = Convert.ToInt32(date.Substring(13, 2));
            int sec = Convert.ToInt32(date.Substring(15, 2));
            DateTime dt = new DateTime(year, month, day, hour, minute, sec);

            return _articleService.GetModifiedArticles(dt);            
        }

    }
}
