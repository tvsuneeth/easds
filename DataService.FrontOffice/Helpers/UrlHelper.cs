using System;
using System.Collections.Generic;
using System.Linq;

namespace twg.chk.DataService.FrontOffice.Helpers
{
    public interface IUrlHelper
    {
        System.Web.Http.Routing.UrlHelper RouteHelper { set; }
        String GenerateUrl(String linkRouteName, Object linkArgument);
    }

    public class UrlHelper : IUrlHelper
    {
        public UrlHelper(System.Web.Http.Routing.UrlHelper urlHelper)
        {
            RouteHelper = urlHelper;
        }

        public string GenerateUrl(String linkRouteName, Object linkArguments)
        {
            if (RouteHelper == null)
            {
                throw new ArgumentNullException("RouteHelper has not been initialized");
            }

            return RouteHelper.Link(linkRouteName, linkArguments).Replace("&", "%26");
        }

        public System.Web.Http.Routing.UrlHelper RouteHelper { set; private get; }
    }
}