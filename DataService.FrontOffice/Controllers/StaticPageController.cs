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
    [RoutePrefix("content")]
    public class StaticPageController : ApiController
    {
        private IStaticPageService _staticPageService;
        private IUrlHelper _contentFeedHelper;
        public StaticPageController(IStaticPageService staticPageService, IUrlHelper contentFeedHelper)
        {
            _staticPageService = staticPageService;
            _contentFeedHelper = contentFeedHelper;
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStaticPageById")]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetById(int id)
        {
            HttpResponseMessage responseMessage;

            var staticPage = _staticPageService.GetById(id);
            if (staticPage != null)
            {
                var contentFeed = new SingleContentFeed<StaticPage>(
                    _contentFeedHelper.GenerateUrl("GetStaticPageById", new { id }),
                    staticPage,
                    _contentFeedHelper
                );
                responseMessage = Request.CreateResponse<SingleContentFeed<StaticPage>>(HttpStatusCode.OK, contentFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Static page not found.");
            }

            return responseMessage;
        }

        [HttpGet]
        [Route("{pageName:regex(^[a-zA-Z-]+)}", Name = "GetStaticPageByName")]
        [Authorize(Roles = "frontofficegroup")]
        public HttpResponseMessage GetByName(String pageName)
        {
            HttpResponseMessage responseMessage;

            var staticPage = _staticPageService.GetByName(pageName);
            if (staticPage != null)
            {
                var contentFeed = new SingleContentFeed<StaticPage>(
                    _contentFeedHelper.GenerateUrl("GetStaticPageByName", new { pageName }),
                    staticPage,
                    _contentFeedHelper
                );
                responseMessage = Request.CreateResponse<SingleContentFeed<StaticPage>>(HttpStatusCode.OK, contentFeed);

                responseMessage = Request.CreateResponse<Feed<StaticPage>>(HttpStatusCode.OK, contentFeed);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Static page not found.");
            }

            return responseMessage;
        }
    }
}
