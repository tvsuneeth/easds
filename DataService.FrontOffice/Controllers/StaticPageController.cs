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
    //[RoutePrefix("content")]
    public class StaticPageController : ApiController
    {
        private IStaticPageService _staticPageService;
        private IUrlHelper _urlHelper;
        private IStaticContentLinkService _staticContentLinkService;
        public StaticPageController(IStaticPageService staticPageService, IStaticContentLinkService staticContentLinkService, IUrlHelper urlHelper)
        {
            _staticPageService = staticPageService;
            _urlHelper = urlHelper;
            _staticContentLinkService = staticContentLinkService;
        }

        [HttpGet]
        [Route("staticpage/{id:int}", Name = "GetStaticPageById")]
        [Authorize(Roles = "frontofficegroup")]
       // [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 3600, AnonymousOnly = false)]
        public SingleContentFeed<StaticPage> GetById(int id)
        {
            _urlHelper.RouteHelper = Url;

            var staticPage = _staticPageService.GetById(id);
            if (staticPage != null)
            {
                var contentFeed = new SingleContentFeed<StaticPage>(
                    _urlHelper.GenerateUrl("GetStaticPageById", new { id }),
                    staticPage,
                    _urlHelper,
                    _staticContentLinkService
                );

                return contentFeed;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("staticpage/byname/{name:regex(^[a-zA-Z0-9- ,&]+)}", Name = "GetStaticPageByName")]
        [Authorize(Roles = "frontofficegroup")]
       // [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 3600, AnonymousOnly = false)]
        public SingleContentFeed<StaticPage> GetByName(String name)
        {
            _urlHelper.RouteHelper = Url;

            var staticPage = _staticPageService.GetByName(name);
            if (staticPage != null)
            {
                var contentFeed = new SingleContentFeed<StaticPage>(
                    _urlHelper.GenerateUrl("GetStaticPageByName", staticPage.GetIdentificationElement()),
                    staticPage,
                    _urlHelper,
                    _staticContentLinkService
                );

                return contentFeed;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("staticpages", Name = "GetAllStaticPages")]
        [Authorize(Roles = "frontofficegroup")]
        //[CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 3600, AnonymousOnly = false)]
        public List<StaticPageSummary> GetAllStaticPages()
        {
            var list = _staticPageService.GetAll();
            if (list == null || list.Count == 0)
            {
                return null;
            }
            return list;
        }
    }
}
