using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using twg.chk.DataService.api;
using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice.Models;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Controllers
{
    public class TaxonomyController : ApiController
    {
        private IArticleService _articleService;
        private IContentFeedHelper _contentFeedHelper;
        public TaxonomyController(IArticleService articleService, IContentFeedHelper contentFeedHelper)
        {
            _articleService = articleService;
            _contentFeedHelper = contentFeedHelper;
        }

        [HttpGet]
        [Route("topic/{topic:regex(^[a-zA-Z- ]+)}/{page:int?}", Name = "GetArticleByTopic")]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetTopic(String topic, int page = 1)
        {
            HttpResponseMessage responseMessage;

            var articles = _articleService.GetByTopic(topic, page, 20);
            if (articles.Summaries.Count > 0)
            {
                var contentFeed = new ContentFeed<PaginatedArticleSummaries>(Url, articles, _contentFeedHelper);

                responseMessage = Request.CreateResponse<ContentFeed<PaginatedArticleSummaries>>(HttpStatusCode.OK, contentFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No article found.");
            }

            return responseMessage;
        }

        [HttpGet]
        [Route("sector/{sector:regex(^[a-zA-Z- ]+)}/{page:int?}", Name = "GetArticleBySector")]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetSector(String sector, int page = 1)
        {
            HttpResponseMessage responseMessage;

            var articles = _articleService.GetBySector(sector, page, 20);
            if (articles.Summaries.Count > 0)
            {
                var contentFeed = new ContentFeed<PaginatedArticleSummaries>(Url, articles, _contentFeedHelper);

                responseMessage = Request.CreateResponse<ContentFeed<PaginatedArticleSummaries>>(HttpStatusCode.OK, contentFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No article found.");
            }

            return responseMessage;
        }

        [HttpGet]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetArticleSection(String articleSection, int page = 1)
        {
            HttpResponseMessage responseMessage;

            var articles = _articleService.GetByArticleSection(articleSection, page, 20);
            if (articles.Summaries.Count > 0)
            {
                var contentFeed = new ContentFeed<PaginatedArticleSummaries>(Url, articles, _contentFeedHelper);

                responseMessage = Request.CreateResponse<ContentFeed<PaginatedArticleSummaries>>(HttpStatusCode.OK, contentFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No article found.");
            }

            return responseMessage;
        }

        [HttpGet]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetArticleSectionAndSector(String articleSection, String sector, int page = 1)
        {
            HttpResponseMessage responseMessage;

            var articles = _articleService.GetByArticleSectionAndSector(articleSection, sector, page, 20);
            if (articles.Summaries.Count > 0)
            {
                var contentFeed = new ContentFeed<PaginatedArticleSummaries>(Url, articles, _contentFeedHelper);

                responseMessage = Request.CreateResponse<ContentFeed<PaginatedArticleSummaries>>(HttpStatusCode.OK, contentFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No article found.");
            }

            return responseMessage;
        }
    }
}
