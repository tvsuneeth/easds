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
        private IUrlHelper _urlHelper;
        public StaticPageController(IStaticPageService staticPageService, IUrlHelper urlHelper)
        {
            _staticPageService = staticPageService;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStaticPageById")]
        [Authorize(Roles = "frontofficegroup")]
        public SingleContentFeed<StaticPage> GetById(int id)
        {
            _urlHelper.RouteHelper = Url;

            var staticPage = _staticPageService.GetById(id);
            if (staticPage != null)
            {
                var contentFeed = new SingleContentFeed<StaticPage>(
                    _urlHelper.GenerateUrl("GetStaticPageById", new { id }),
                    staticPage,
                    _urlHelper
                );

                return contentFeed;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("{name:regex(^[a-zA-Z-]+)}", Name = "GetStaticPageByName")]
        [Authorize(Roles = "frontofficegroup")]
        public SingleContentFeed<StaticPage> GetByName(String name)
        {
            _urlHelper.RouteHelper = Url;

            var staticPage = _staticPageService.GetByName(name);
            if (staticPage != null)
            {
                var contentFeed = new SingleContentFeed<StaticPage>(
                    _urlHelper.GenerateUrl("GetStaticPageByName", staticPage.GetIdentificationElement()),
                    staticPage,
                    _urlHelper
                );

                return contentFeed;
            }
            else
            {
                return null;
            }
        }
    }
}
