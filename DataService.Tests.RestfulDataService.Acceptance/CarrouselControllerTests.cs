using System;
using System.Collections.Generic;
using System.Net.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Ninject;

using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice;

namespace twg.chk.DataService.Tests.FrontOffice.Acceptance
{
    [TestClass]
    public class CarrouselControllerTests
    {
        private IKernel _kernel;
        private CarrouselController _objectUnderTest;
        private ICarrouselService _fakeCarrouselService;

        [TestInitialize]
        public void Setup()
        {
            _kernel = new StandardKernel();
            Startup.NinjectConfig.CreateKernel(_kernel);

            _fakeCarrouselService = MockRepository.GenerateMock<ICarrouselService>();
            _objectUnderTest = new CarrouselController(_fakeCarrouselService);
        }
        
        [TestMethod]
        public void GetHomepageCarrousel_RequestCarrousel()
        {
            var httpMessage = _objectUnderTest.Get();
            Assert.IsTrue(httpMessage.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(httpMessage.Content);
            Assert.IsInstanceOfType(httpMessage, typeof(IEnumerable<Carrousel>));
        }
    }
}
