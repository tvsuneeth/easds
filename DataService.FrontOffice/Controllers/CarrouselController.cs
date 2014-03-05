using System;
using System.Web.Http;

namespace twg.chk.DataService.FrontOffice
{
    public class CarrouselController: ApiController
    {
        private ICarrouelManager _carrouselManager;
        public CarrouselController(ICarrouelManager carrouselManager)
        {
            _carrouselManager = carrouselManager;
        }


        public String Get()
        {
            return _carrouselManager.GetHomepageCarrousel();
        }

        public String Get(int id)
        {
            return _carrouselManager.GetHomepageCarrousel() + " : " + id.ToString();
        }
    }
}