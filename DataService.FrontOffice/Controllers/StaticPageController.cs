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
    [RoutePrefix("content")]
    public class StaticPageController : ApiController
    {
        private IStaticPageService _staticPageService;

        public StaticPageController(IStaticPageService staticPageService)
        {
            _staticPageService = staticPageService;
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
                responseMessage = Request.CreateResponse<StaticPage>(HttpStatusCode.OK, staticPage);
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
                responseMessage = Request.CreateResponse<StaticPage>(HttpStatusCode.OK, staticPage);
            }
            else
            {
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Static page not found.");
            }

            return responseMessage;
        }
    }
}
