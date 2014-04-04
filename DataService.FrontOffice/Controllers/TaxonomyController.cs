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
        private IUrlHelper _urlHelper;
        public TaxonomyController(IArticleService articleService, IUrlHelper urlHelper)
        {
            _articleService = articleService;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [Route("topic/{topic:regex(^[a-zA-Z- ]+)}/{page:int?}", Name = "GetArticleByTopic")]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetTopic(String topic, int page = 1)
        {
            _urlHelper.RouteHelper = Url;
            HttpResponseMessage responseMessage;

            var paginatedArticleSummaries = _articleService.GetByTopic(topic, page, 20);
            if (paginatedArticleSummaries.Summaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<PaginatedArticleSummaries, ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleByTopic", new { topic, page }),
                    String.Format("Topic {0} page {1}", topic, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    "GetArticleById"
                );

                contentFeed.NextLink = paginatedArticleSummaries.HasNextPage ? 
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByTopic", new { topic, page = paginatedArticleSummaries.NextPage }), 
                        Rel = "next",
                        Title = String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.NextPage), 
                        Verb = "GET"  } 
                    : null;

                contentFeed.PreviousLink = paginatedArticleSummaries.HasPreviousPage ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByTopic", new { topic, page = paginatedArticleSummaries.PreviousPage }),
                        Rel = "prev",
                        Title = String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.PreviousPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.LastLink = paginatedArticleSummaries.LastPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByTopic", new { topic, page = paginatedArticleSummaries.LastPage }),
                        Rel = "last",
                        Title = String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.LastPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.FirstLink = paginatedArticleSummaries.FirstPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByTopic", new { topic, page = paginatedArticleSummaries.FirstPage }),
                        Rel = "first",
                        Title = String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.FirstPage),
                        Verb = "GET"
                    }
                    : null;

                responseMessage = Request.CreateResponse<Feed<PaginatedArticleSummaries>>(HttpStatusCode.OK, contentFeed);
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
            _urlHelper.RouteHelper = Url;
            HttpResponseMessage responseMessage;

            var paginatedArticleSummaries = _articleService.GetBySector(sector, page, 20);
            if (paginatedArticleSummaries.Summaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<PaginatedArticleSummaries, ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleBySector", new { sector, page }),
                    String.Format("Sector {0} page {1}", sector, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    "GetArticleById"
                );

                contentFeed.NextLink = paginatedArticleSummaries.HasNextPage ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleBySector", new { sector, page = paginatedArticleSummaries.NextPage }),
                        Rel = "next",
                        Title = String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.NextPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.PreviousLink = paginatedArticleSummaries.HasPreviousPage ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleBySector", new { sector, page = paginatedArticleSummaries.PreviousPage }),
                        Rel = "prev",
                        Title = String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.PreviousPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.LastLink = paginatedArticleSummaries.LastPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleBySector", new { sector, page = paginatedArticleSummaries.LastPage }),
                        Rel = "last",
                        Title = String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.LastPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.FirstLink = paginatedArticleSummaries.FirstPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleBySector", new { sector, page = paginatedArticleSummaries.FirstPage }),
                        Rel = "first",
                        Title = String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.FirstPage),
                        Verb = "GET"
                    }
                    : null;


                responseMessage = Request.CreateResponse<MultipleContentFeed<PaginatedArticleSummaries, ArticleSummary>>(HttpStatusCode.OK, contentFeed);
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
            _urlHelper.RouteHelper = Url;
            HttpResponseMessage responseMessage;

            var paginatedArticleSummaries = _articleService.GetByArticleSection(articleSection, page, 20);
            if (paginatedArticleSummaries.Summaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<PaginatedArticleSummaries, ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection, page }),
                    String.Format("Article Section {0} page {1}", articleSection, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    "GetArticleById"
                );

                contentFeed.NextLink = paginatedArticleSummaries.HasNextPage ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection, page = paginatedArticleSummaries.NextPage }),
                        Rel = "next",
                        Title = String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.NextPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.PreviousLink = paginatedArticleSummaries.HasPreviousPage ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection, page = paginatedArticleSummaries.PreviousPage }),
                        Rel = "prev",
                        Title = String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.PreviousPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.LastLink = paginatedArticleSummaries.LastPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection, page = paginatedArticleSummaries.LastPage }),
                        Rel = "last",
                        Title = String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.LastPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.FirstLink = paginatedArticleSummaries.FirstPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection, page = paginatedArticleSummaries.FirstPage }),
                        Rel = "first",
                        Title = String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.FirstPage),
                        Verb = "GET"
                    }
                    : null;

                responseMessage = Request.CreateResponse<MultipleContentFeed<PaginatedArticleSummaries, ArticleSummary>>(HttpStatusCode.OK, contentFeed);
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
            _urlHelper.RouteHelper = Url;
            HttpResponseMessage responseMessage;

            var paginatedArticleSummaries = _articleService.GetByArticleSectionAndSector(articleSection, sector, page, 20);
            if (paginatedArticleSummaries.Summaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<PaginatedArticleSummaries, ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleByArticleSectionAndSector", new { articleSection, sector, page }),
                    String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    "GetArticleById"
                );

                contentFeed.NextLink = paginatedArticleSummaries.HasNextPage ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSectionAndSector", new { articleSection, sector, page = paginatedArticleSummaries.NextPage }),
                        Rel = "next",
                        Title = String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.NextPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.PreviousLink = paginatedArticleSummaries.HasPreviousPage ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSectionAndSector", new { articleSection, sector, page = paginatedArticleSummaries.PreviousPage }),
                        Rel = "prev",
                        Title = String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.PreviousPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.LastLink = paginatedArticleSummaries.LastPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSectionAndSector", new { articleSection, sector, page = paginatedArticleSummaries.LastPage }),
                        Rel = "last",
                        Title = String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.LastPage),
                        Verb = "GET"
                    }
                    : null;

                contentFeed.FirstLink = paginatedArticleSummaries.FirstPage > 0 ?
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSectionAndSector", new { articleSection, sector, page = paginatedArticleSummaries.FirstPage }),
                        Rel = "first",
                        Title = String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.FirstPage),
                        Verb = "GET"
                    }
                    : null;

                responseMessage = Request.CreateResponse<MultipleContentFeed<PaginatedArticleSummaries, ArticleSummary>>(HttpStatusCode.OK, contentFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No article found.");
            }

            return responseMessage;
        }
    }
}
