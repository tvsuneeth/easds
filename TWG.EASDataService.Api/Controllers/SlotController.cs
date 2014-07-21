using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TWG.EASDataService.Services;
using TWG.EASDataService.Business;
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.Controllers
{
    public class SlotController : ApiController
    {
        private ISlotService _slotService;
        private IUrlHelper _urlHelper;
        public SlotController(ISlotService slotService, IUrlHelper urlHelper)
        {
            _slotService = slotService;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [Route("slotpage/{id:int}", Name = "GetSlotPageById")]
        [Authorize(Roles = "frontofficegroup")]
        public SlotPage GetSlotPageById(int id)
        {
            
            _urlHelper.RouteHelper = Url;
            var sp = _slotService.GetSlotPageById(id);

            if (sp == null)
            {
                return null;
            }
            foreach (var item in sp.Slots)
            {
                var image = item.Image;
                if (image != null)
                {
                    image.Url = _urlHelper.GenerateUrl("GetMediaContentById", new { id = image.Id });
                }
            }
            return sp;
        }

        [HttpGet]
        [Route("slotpages", Name = "GetLisOfSlotPages")]
        [Authorize(Roles = "frontofficegroup")]
        public List<SlotPageSummary> GetLisOfSlotPages()
        {
            var list = _slotService.GetListOfSlotPages();
            if (list == null || list.Count() == 0)
            { 
                return null; 
            }
            
            return list;

        }
    }
}
