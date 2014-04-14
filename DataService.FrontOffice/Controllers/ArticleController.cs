using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Syndication;

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
        public ArticleController(IArticleService articleService, IUrlHelper urlHelper)
        {
            _articleService = articleService;
            _urlHelper = urlHelper;
        }

        [Route("article/{id:int}", Name = "GetArticleById")]
        [Authorize(Roles = "frontofficegroup")]
        public SingleContentFeed<Article> Get(int id)
        {
            _urlHelper.RouteHelper = Url;
            
            var article = _articleService.GetById(id);
            if (article != null)
            {
                var articleFeed = new SingleContentFeed<Article>(
                    _urlHelper.GenerateUrl("GetArticleById", new { id = article.Id }),
                    article,
                    _urlHelper
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
    }
}
