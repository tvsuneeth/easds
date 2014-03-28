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

namespace twg.chk.DataService.FrontOffice.Controllers
{
    [RoutePrefix("article")]
    public class ArticleController : ApiController
    {
        private IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [Route("{id:int}", Name = "GetArticleById")]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage responseMessage;
            
            var article = _articleService.GetById(id);
            if (article != null)
            {
                var articleFeed = new ContentFeed<Article>(Url, article);

                responseMessage = Request.CreateResponse<ContentFeed<Article>>(HttpStatusCode.OK, articleFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Article not found.");
            }

            return responseMessage;
        }
    }
}
