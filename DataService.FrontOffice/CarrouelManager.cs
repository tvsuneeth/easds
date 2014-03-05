using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace twg.chk.DataService.FrontOffice
{
    public interface ICarrouelManager
    {
        String GetHomepageCarrousel();
    }

    public class CarrouselManager : ICarrouelManager
    {
        public string GetHomepageCarrousel()
        {
            return "Message from the Concrete Implementation";
        }
    }
}
