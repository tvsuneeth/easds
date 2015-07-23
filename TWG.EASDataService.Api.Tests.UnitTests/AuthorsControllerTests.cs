using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Repository;
using TWG.EASDataService.DbContext.Repository;
using TWG.EASDataService.Services;
using TWG.EASDataService.Api.Controllers;
using TWG.EASDataService.Api.Models;
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.Tests.UnitTests
{
    [TestClass]
    public class AuthorsControllerTests
    {
        private AuthorController _sut;        
        private IAuthorService _authorService;                

        [TestInitialize]
        public void Setup()
        {            
            _authorService = MockRepository.GenerateStub<IAuthorService>();
            _sut = new AuthorController(_authorService);
            _sut.Request = new HttpRequestMessage();
            _sut.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestMethod]
        public void Index_ShouldReturn_ListOfAllAuthors()
        {
            _authorService.Stub(r => r.GetAllAuthors()).Return(new List<Author>() { new Author() { ID=1 }, new Author() { ID=2 } });
            var authors = _sut.GetAllAuthors();
            Assert.AreEqual(authors.Count, 2);
        }
    }
}
