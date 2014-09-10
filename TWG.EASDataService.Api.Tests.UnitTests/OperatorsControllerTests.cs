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
using TWG.EASDataService.Services;
using TWG.EASDataService.Api.Controllers;
using TWG.EASDataService.Api.Helpers;


namespace TWG.EASDataService.Api.Tests.UnitTests
{
    [TestClass]
    public class OperatorsControllerTests
    {
        private OperatorsController _objectUnderTest;
        private IOperatorRepository _operatorRepository;
        private IOperatorService _operatorService;
        private IUrlHelper _urlHelper;

        [TestInitialize]
        public void Setup()
        {
            _operatorRepository = MockRepository.GenerateStub<IOperatorRepository>();                                                
            _operatorService = new OperatorService(_operatorRepository);           
            _urlHelper = MockRepository.GenerateStub<IUrlHelper>();
            _objectUnderTest = new OperatorsController(_operatorService,_urlHelper);
            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void OperatorsController_Index_ReturnsListOfOperators()
        {            
            List<Operator> inputlist = new List<Operator>() 
            {
                {new Operator(){ Id=1, Name="Operator1" }},
                {new Operator(){ Id=2, Name="Operator2" }}
            };
            _operatorRepository.Stub(s => s.GetAll()).Return(inputlist);

            var list = _objectUnderTest.Index();
            Assert.IsTrue(list.Count==2);
        }

    }
}
