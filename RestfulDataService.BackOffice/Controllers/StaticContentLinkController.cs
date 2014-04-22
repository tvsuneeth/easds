using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using PagedList;

using twg.chk.DataService.api;
using twg.chk.DataService.Business;

namespace twg.chk.DataService.BackOffice.Controllers
{
    [RoutePrefix("staticlink")]
    public class StaticContentLinkController : AsyncController
    {
        private IStaticContentLinkService _staticContentLinkService;
        public StaticContentLinkController(IStaticContentLinkService staticContentLinkService)
        {
            _staticContentLinkService = staticContentLinkService;
        }

        [HttpGet]
        [Route("{page:int?}", Name = "GetStaticContentLinkIndex")]
        public ActionResult Index(int page = 1)
        {
            var pagedStaticLinks = _staticContentLinkService.GetPaged(page, 20);

            return View(pagedStaticLinks);
        }
        
        [HttpPost]
        [Route(Name = "CreateStaticContentLink")]
        public JsonResult Create([System.Web.Http.FromBody]StaticContentLink staticContentLink)
        {
            _staticContentLinkService.Create(staticContentLink);
            return null;
        }

        [HttpPut]
        [Route("{id:int}", Name = "UpdateStaticContentLink")]
        public JsonResult Edit(int id, [System.Web.Http.FromBody]StaticContentLink staticContentLink)
        {
            _staticContentLinkService.Update(staticContentLink);
            return null;
        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteStaticContentLink")]
        public JsonResult Delete(int id)
        {
            _staticContentLinkService.Remove(id);

            return null;
        }
	}
}