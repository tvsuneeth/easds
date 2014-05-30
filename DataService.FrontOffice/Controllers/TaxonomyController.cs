﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

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
        private IStaticContentLinkService _staticContentLinkService;
        public TaxonomyController(IArticleService articleService, IStaticContentLinkService staticContentLinkService, IUrlHelper urlHelper)
        {
            _articleService = articleService;
            _urlHelper = urlHelper;
            _staticContentLinkService = staticContentLinkService;
        }

        [HttpGet]
        [Route("taxonomy", Name = "GetAlltaxonomyCategories")]
        //[Authorize(Roles = "frontofficegroup")]
        public List<TaxonomyCategory> GetAlltaxonomyCategories()
        {
            var result = _articleService.GetAllTaxonomyCategoriesAndItems();
            return result;
        }

        [HttpGet]
        [Route("topic/{topic:regex(^[a-zA-Z0-9- ,&]+)}/{page:int?}", Name = "GetArticleByTopic")]
        [Authorize(Roles = "frontofficegroup")]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 1200, AnonymousOnly = false)]
        public MultipleContentFeed<ArticleSummary> GetTopic(String topic, int page = 1)
        {
            _urlHelper.RouteHelper = Url;

            var paginatedArticleSummaries = _articleService.GetByTopic(topic, page, 20);
            if (paginatedArticleSummaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleByTopic", new { topic, page }),
                    String.Format("Topic {0} page {1}", topic, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    _staticContentLinkService,
                    "GetArticleById"
                );

                if (paginatedArticleSummaries.HasMultiplePage)
                {
                    if (paginatedArticleSummaries.HasNextPage)
                    {
                        contentFeed.SetNextLink("GetArticleByTopic", String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.NextPage), new { topic, page = paginatedArticleSummaries.NextPage });
                    }
                    if (paginatedArticleSummaries.HasPreviousPage)
                    {
                        contentFeed.SetPreviousLink("GetArticleByTopic", String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.PreviousPage), new { topic, page = paginatedArticleSummaries.PreviousPage });
                    }
                    contentFeed.SetFirstLink("GetArticleByTopic", String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.FirstPage), new { topic, page = paginatedArticleSummaries.FirstPage });
                    contentFeed.SetLastLink("GetArticleByTopic", String.Format("Topic {0} page {1}", topic, paginatedArticleSummaries.LastPage), new { topic, page = paginatedArticleSummaries.LastPage });
                }

                return contentFeed;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("sector/{sector:regex(^[a-zA-Z0-9- ,&]+)}/{page:int?}", Name = "GetArticleBySector")]
        [Authorize(Roles = "frontofficegroup")]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 1200, AnonymousOnly = false)]
        public MultipleContentFeed<ArticleSummary> GetSector(String sector, int page = 1)
        {
            _urlHelper.RouteHelper = Url;

            var paginatedArticleSummaries = _articleService.GetBySector(sector, page, 20);
            if (paginatedArticleSummaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleBySector", new { sector, page }),
                    String.Format("Sector {0} page {1}", sector, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    _staticContentLinkService,
                    "GetArticleById"
                );

                if (paginatedArticleSummaries.HasMultiplePage)
                {
                    if (paginatedArticleSummaries.HasNextPage)
                    {
                        contentFeed.SetNextLink("GetArticleBySector", String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.NextPage), new { sector, page = paginatedArticleSummaries.NextPage });
                    }
                    if (paginatedArticleSummaries.HasPreviousPage)
                    {
                        contentFeed.SetPreviousLink("GetArticleBySector", String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.PreviousPage), new { sector, page = paginatedArticleSummaries.PreviousPage });
                    }
                    contentFeed.SetFirstLink("GetArticleBySector", String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.FirstPage), new { sector, page = paginatedArticleSummaries.FirstPage });
                    contentFeed.SetLastLink("GetArticleBySector", String.Format("Sector {0} page {1}", sector, paginatedArticleSummaries.LastPage), new { sector, page = paginatedArticleSummaries.LastPage });
                }

                return contentFeed;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Authorize(Roles = "frontofficegroup")]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 1200, AnonymousOnly = false)]
        public MultipleContentFeed<ArticleSummary> GetArticleSection(String articleSection, int page = 1)
        {
            _urlHelper.RouteHelper = Url;

            var paginatedArticleSummaries = _articleService.GetByArticleSection(articleSection, page, 20);
            if (paginatedArticleSummaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection, page }),
                    String.Format("Article Section {0} page {1}", articleSection, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    _staticContentLinkService,
                    "GetArticleById"
                );

                if (paginatedArticleSummaries.HasMultiplePage)
                {
                    if (paginatedArticleSummaries.HasNextPage)
                    {
                        contentFeed.SetNextLink("GetArticleByArticleSection", String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.NextPage), new { articleSection, page = paginatedArticleSummaries.NextPage });
                    }
                    if (paginatedArticleSummaries.HasPreviousPage)
                    {
                        contentFeed.SetPreviousLink("GetArticleByArticleSection", String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.PreviousPage), new { articleSection, page = paginatedArticleSummaries.PreviousPage });
                    }
                    contentFeed.SetFirstLink("GetArticleByArticleSection", String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.FirstPage), new { articleSection, page = paginatedArticleSummaries.FirstPage });
                    contentFeed.SetLastLink("GetArticleByArticleSection", String.Format("Article Section {0} page {1}", articleSection, paginatedArticleSummaries.LastPage), new { articleSection, page = paginatedArticleSummaries.LastPage });
                }

                return contentFeed;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Authorize(Roles = "frontofficegroup")]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 1200, AnonymousOnly = false)]
        public MultipleContentFeed<ArticleSummary> GetArticleSectionAndSector(String articleSection, String sector, int page = 1)
        {
            _urlHelper.RouteHelper = Url;

            var paginatedArticleSummaries = _articleService.GetByArticleSectionAndSector(articleSection, sector, page, 20);
            if (paginatedArticleSummaries.Count > 0)
            {
                var contentFeed = new MultipleContentFeed<ArticleSummary>(
                    _urlHelper.GenerateUrl("GetArticleByArticleSectionAndSector", new { articleSection, sector, page }),
                    String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, page), 
                    paginatedArticleSummaries,
                    _urlHelper,
                    _staticContentLinkService,
                    "GetArticleById"
                );

                if (paginatedArticleSummaries.HasMultiplePage)
                {
                    if (paginatedArticleSummaries.HasNextPage)
                    {
                        contentFeed.SetNextLink("GetArticleByArticleSectionAndSector", String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.NextPage), new { articleSection, sector, page = paginatedArticleSummaries.NextPage });
                    }
                    if (paginatedArticleSummaries.HasPreviousPage)
                    {
                        contentFeed.SetPreviousLink("GetArticleByArticleSectionAndSector", String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.PreviousPage), new { articleSection, sector, page = paginatedArticleSummaries.PreviousPage });
                    }
                    contentFeed.SetFirstLink("GetArticleByArticleSectionAndSector", String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.FirstPage), new { articleSection, sector, page = paginatedArticleSummaries.FirstPage });
                    contentFeed.SetLastLink("GetArticleByArticleSectionAndSector", String.Format("Article Section {0} and Sector {1} page {2}", articleSection, sector, paginatedArticleSummaries.LastPage), new { articleSection, sector, page = paginatedArticleSummaries.LastPage });
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
