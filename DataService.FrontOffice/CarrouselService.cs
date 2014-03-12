using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace twg.chk.DataService.FrontOffice
{
    public interface ICarrouselService
    {
        String GetHomepageCarrousel();
    }

    public class CarrouselService : ICarrouselService
    {
        public string GetHomepageCarrousel()
        {
            return "Message from the Concrete Implementation";
        }
    }
}
