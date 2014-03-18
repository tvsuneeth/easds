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
    public class ArticleController : ApiController
    {
        private IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage responseMessage;

            var article = _articleService.GetById(id);
            if (article != null)
            {
                responseMessage = Request.CreateResponse<Article>(HttpStatusCode.OK, article);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Article not found.");
            }

            return responseMessage;
        }
    }
}
