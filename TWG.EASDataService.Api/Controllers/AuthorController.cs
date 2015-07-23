using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TWG.EASDataService.Business;
using TWG.EASDataService.Services;

namespace TWG.EASDataService.Api.Controllers
{
    public class AuthorController : ApiController
    {
        IAuthorService authorService;
        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet]
        [Route("Authors", Name = "Authors")]
        [Authorize(Roles = "frontofficegroup")]
        public List<Author> GetAllAuthors()
        {                        
            return authorService.GetAllAuthors();
        }        

    }
}
