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

using twg.chk.DataService.Business;
using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.api;
using twg.chk.DataService.FrontOffice.Controllers;
using twg.chk.DataService.FrontOffice.Models;
using twg.chk.DataService.FrontOffice.Helpers;
using System.Web.Http.Routing;
using System.Web.Http.Hosting;

namespace DataService.Tests.RestfulDataService.FrontOffice.UnitTests
{
    [TestClass]
    public class SlotControllerTests
    {
        private SlotController _objectUnderTest;
        private ISlotRepository _slotRepository;
        private ISlotService _slotService;
        private IStaticContentLinkService _staticContentLinkService;
        private IUrlHelper _urlHelper;

        [TestInitialize]
        public void Setup()
        {
            _slotRepository = MockRepository.GenerateStub<ISlotRepository>();        
            _slotService = new SlotService(_slotRepository);
            _urlHelper = MockRepository.GenerateStub<IUrlHelper>();
            _objectUnderTest = new SlotController(_slotService, _urlHelper);

            _objectUnderTest.Request = new HttpRequestMessage();
            _objectUnderTest.Request.SetConfiguration(new HttpConfiguration());
        }
       

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void GetLisOfSlotPages_returnAllSlotPages()
        {
            List<SlotPageSummary> list = new List<SlotPageSummary>()
            {
                {new SlotPageSummary(){ Id=1, PageName="test1"}},
                {new SlotPageSummary(){ Id=2, PageName="test2"}}
            };
            _slotRepository.Stub(r => r.GetListOfSlotPages()).Return(list);

            var result = _objectUnderTest.GetLisOfSlotPages();

            Assert.IsNotNull(result);
        }



        [TestMethod]
        public void GetSlotPageById_ReturnsASlotPageWithSlots()
        {
            SlotPage sp = new SlotPage() { Id = 1, PageName = "Test", Slots = { new Slot(){ Id=1, Headline="test slot"}  } };
            _slotRepository.Stub(r => r.GetSlotPageById(Arg<int>.Is.Anything)).Return(sp);
            
            var result = _objectUnderTest.GetSlotPageById(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSlotPageById_WithSlotHavingAnImage_ReturnedSlotHasImageUrl()
        {
            //Arrange
            HttpConfiguration config = new HttpConfiguration();
            var route = config.Routes.MapHttpRoute
            (
               name: "GetMediaContentById",
               routeTemplate: "mediacontent/{id}"
            );

            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "id", "1" } });

            
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "http://localhost:80/slotpage/1");
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, config);
            req.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);

            _urlHelper = new twg.chk.DataService.FrontOffice.Helpers.UrlHelper(new System.Web.Http.Routing.UrlHelper(req));
            _objectUnderTest = new SlotController(_slotService, _urlHelper);
            _objectUnderTest.Request = req;

            var slot = new Slot() { Id = 1, Headline = "test slot", Image = new Image() { Name = "dummy image", Id = 1000, FileExtension = "jpg" } };
            SlotPage sp = new SlotPage() { Id = 1, PageName = "Test", Slots = { slot } };
            _slotRepository.Stub(r => r.GetSlotPageById(Arg<int>.Is.Anything)).Return(sp);

            //Act
            var result = _objectUnderTest.GetSlotPageById(1);

            //Assert
            Image img = result.Slots.FirstOrDefault().Image;
            Assert.AreNotEqual(string.Empty,img.Url);
            
        }
        
    }
}
