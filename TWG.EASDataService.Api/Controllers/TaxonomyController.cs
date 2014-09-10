using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

using TWG.EASDataService.Services;
using TWG.EASDataService.Business;
using TWG.EASDataService.Api.Models;
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.Controllers
{
    public class TaxonomyController : ApiController
    {
        private IArticleService _articleService;
        private IUrlHelper _urlHelper;
        private IStaticContentLinkService _staticContentLinkService;
        public TaxonomyController(IArticleService articleService, IStaticContentLinkService staticContentLinkService, IUrlHelper urlHelper)
        {
            _articleService = articleService;
            _urlHelper = urlHelper;
            _staticContentLinkService = staticContentLinkService;
        }

        [HttpGet]
        [Route("taxonomy", Name = "GetAllTaxonomyCategories")]
        [Authorize(Roles = "frontofficegroup")]
        public List<TaxonomyCategory> GetAllTaxonomyCategories()
        {
            var result = _articleService.GetAllTaxonomyCategoriesAndItems();
            return result;
        }       
     }       
    
}
