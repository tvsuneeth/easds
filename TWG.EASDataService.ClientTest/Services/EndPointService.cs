using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TWG.EASDataService.ClientTest.Models;

namespace TWG.EASDataService.ClientTest.Services
{
    public class EndPointService
    {

        public void SetEndPoint(EndPointVM ep)
        {
            HttpContext.Current.Session["ServiceEndPointUrl"] = ep;
        }
        public string GetEndPointUrl()
        {
            string url = string.Empty;
            if(HttpContext.Current.Session["ServiceEndPointUrl"]!=null)
            {
                var obj = (EndPointVM)HttpContext.Current.Session["ServiceEndPointUrl"];
                url = obj.Url;
                url = (!url.EndsWith("/")) ? url + "/" : url;
            }
            return url;
        }

    }
}