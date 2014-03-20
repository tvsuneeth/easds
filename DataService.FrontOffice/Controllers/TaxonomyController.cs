using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using twg.chk.DataService.api;
using twg.chk.DataService.Business;

namespace twg.chk.DataService.FrontOffice.Controllers
{
    public class TaxonomyController : ApiController
    {
        private IArticleService _articleService;
        public TaxonomyController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetTopic(String topic, int page = 1)
        {
            HttpResponseMessage responseMessage;

            int totalNumArticles;
            var articles = _articleService.GetByTopic(topic, page, 20, out totalNumArticles);
            if (articles.Count() > 0)
            {
                responseMessage = Request.CreateResponse<IEnumerable<Article>>(HttpStatusCode.OK, articles);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No article found.");
            }

            return responseMessage;
        }

        [HttpGet]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetSector(String sector, int page = 1)
        {
            HttpResponseMessage responseMessage;

            int totalNumArticles;
            var articles = _articleService.GetBySector(sector, page, 20, out totalNumArticles);
            if (articles.Count() > 0)
            {
                responseMessage = Request.CreateResponse<IEnumerable<Article>>(HttpStatusCode.OK, articles);
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

            int totalNumArticles;
            var articles = _articleService.GetByArticleSection(articleSection, page, 20, out totalNumArticles);
            if (articles.Count() > 0)
            {
                responseMessage = Request.CreateResponse<IEnumerable<Article>>(HttpStatusCode.OK, articles);
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

            int totalNumArticles;
            var articles = _articleService.GetByArticleSectionAndSector(articleSection, sector, page, 20, out totalNumArticles);
            if (articles.Count() > 0)
            {
                responseMessage = Request.CreateResponse<IEnumerable<Article>>(HttpStatusCode.OK, articles);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "No article found.");
            }

            return responseMessage;
        }
    }
}
