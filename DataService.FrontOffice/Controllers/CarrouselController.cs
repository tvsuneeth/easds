using System;
using System.Net.Http;
using System.Web.Http;

namespace twg.chk.DataService.FrontOffice
{
    public class CarrouselController: ApiController
    {
        private ICarrouselService _carrouselManager;
        public CarrouselController(ICarrouselService carrouselManager)
        {
            _carrouselManager = carrouselManager;
        }


        public HttpResponseMessage Get()
        {
            throw new NotImplementedException();
        }
    }
}