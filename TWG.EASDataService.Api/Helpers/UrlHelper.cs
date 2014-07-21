using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TWG.EASDataService.Api.Helpers
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

            /*var regex = new Regex(@"^[0-9]+$");                                 
            if(linkArguments!=null)
            { 
                Type t = linkArguments.GetType();
                var properties = t.GetProperties();          
                foreach (var prop in properties)
                {
                    object v = prop.GetValue(linkArguments, null);
                
                    if(!regex.IsMatch(v.ToString()))
                    {
                        return "";
                    }
                }
            }*/

            if (linkArguments != null)
            {
                Type t = linkArguments.GetType();
                var properties = t.GetProperties();
                foreach (var prop in properties)
                {
                    object propVal = prop.GetValue(linkArguments, null);
                    string val = propVal.ToString();

                  /*  if (!Regex.IsMatch(val, @"^[\w-._~!$&'()* +,;=:@]*$"))
                    {
                        return "invalidurl";
                    }*/

                    if (!Regex.IsMatch(val, @"^[\w-._~$&'()* +,;=:@]*$"))
                    {
                        return "invalidurl";
                    }   
                }
            }
 
            return RouteHelper.Link(linkRouteName, linkArguments).Replace("&", "%26");    

        }

        public System.Web.Http.Routing.UrlHelper RouteHelper { set; private get; }
    }
}