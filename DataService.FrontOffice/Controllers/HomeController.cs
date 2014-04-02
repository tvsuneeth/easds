using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace twg.chk.DataService.FrontOffice.Controllers
{
    public class HomeController : ApiController
    {
        /// <summary>
        /// Content to show on the homepage
        /// 
        /// Feed that contains a list of article summaries with a list of links
        /// </summary>
        /// <returns></returns>
        [Route("", Name="GetRoot")]
        public HttpResponseMessage GetHomePageContent()
        {
            throw new NotImplementedException();
        }
    }
}
