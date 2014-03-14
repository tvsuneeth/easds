using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace twg.chk.DataService.FrontOffice.Controllers
{
    public class TaxonomyController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetTopic(String topic, int page = 1)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public HttpResponseMessage GetSector(String sector, int page = 1)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public HttpResponseMessage GetArticleSection(String articleSection, int page = 1)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public HttpResponseMessage GetArticleSectionAndSector(String articleSection, String sector, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}
