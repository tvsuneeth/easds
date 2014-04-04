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
    [RoutePrefix("article")]
    public class ArticleController : ApiController
    {
        private IArticleService _articleService;
        private IUrlHelper _urlHelper;
        public ArticleController(IArticleService articleService, IUrlHelper urlHelper)
        {
            _articleService = articleService;
            _urlHelper = urlHelper;
        }

        [Route("{id:int}", Name = "GetArticleById")]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage Get(int id)
        {
            _urlHelper.RouteHelper = Url;

            HttpResponseMessage responseMessage;
            
            var article = _articleService.GetById(id);
            if (article != null)
            {
                var articleFeed = new SingleContentFeed<Article>(
                    _urlHelper.GenerateUrl("GetArticleById", new { id = article.Id }),
                    article,
                    _urlHelper
                );

                responseMessage = Request.CreateResponse<SingleContentFeed<Article>>(HttpStatusCode.OK, articleFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Article not found.");
            }

            return responseMessage;
        }
    }
}
