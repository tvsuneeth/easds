using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;

namespace twg.chk.DataService.FrontOffice.Helpers
{
    public interface IContentFeedHelper
    {
        String GenerateLink(UrlHelper urlHelper, String linkRouteName, Object linkArgument);
    }

    public class ContentFeedHelper : IContentFeedHelper
    {
        public string GenerateLink(UrlHelper urlHelper, String linkRouteName, Object linkArguments)
        {
            return urlHelper.Link(linkRouteName, linkArguments);
        }
    }
}