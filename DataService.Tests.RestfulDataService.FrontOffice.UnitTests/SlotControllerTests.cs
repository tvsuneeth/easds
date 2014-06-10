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

  //public class Helpers
  //{
  //      public HttpContextBase CreateMockHttpContext(string requestedUrl = null, string httpMethod = "GET")
  //      {
  //          var httpRequestMock = MockRepository.GenerateMock<HttpRequestBase>();
  //          httpRequestMock.Stub(m => m.AppRelativeCurrentExecutionFilePath).Return(requestedUrl);
  //          httpRequestMock.Stub(m => m.HttpMethod).Return(httpMethod);
  //          var httpResponseMock = MockRepository.GenerateMock<HttpResponseBase>();
  //          var httpContext = MockRepository.GenerateMock<HttpContextBase>();
  //          httpContext.Stub(m => m.Request).Return(httpRequestMock);
  //          httpContext.Stub(m => m.Response).Return(httpResponseMock);
  //          return httpContext;
  //      }
  // }


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

            HttpConfiguration config = new HttpConfiguration();      
            var route = config.Routes.MapHttpRoute
               (
               name: "GetMediaContentById",
               routeTemplate: "slotpage/{id:int}",
               defaults: new { controller = "MediaController", action = "GetMediaContentById" }
               );

            var routeData = new HttpRouteData(route,
                   new HttpRouteValueDictionary 
                     { 
                        { "id", "1" },
                        { "controller", "MediaController" } 
                    }
                );



            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/slotpage/1");                        

            _urlHelper = new twg.chk.DataService.FrontOffice.Helpers.UrlHelper(new System.Web.Http.Routing.UrlHelper(req));

           

            _objectUnderTest = new SlotController(_slotService, _urlHelper);
            _objectUnderTest.Request = req;
            _objectUnderTest.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, config);
            _objectUnderTest.Request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);

                                   

        }

        //public void WhenRootPathIsCallThenHomeIndexIsReturned()
        //{
        //var routes = new RouteCollection();
        //MvcApplication.RegisterRoutes(routes); // In MvcApplication we have set up a default route
        //var help = new Helpers();
        //const string URL_TO_TEST = @”~/”;
        //HttpContextBase mockHttpCOntext = help.CreateMockHttpContext(URL_TO_TEST, “GET”);
        //// is there a route in the collection that matches the route we passed in?
        //RouteData result = routes.GetRouteData(mockHttpCOntext);
        //Assert.IsNotNull(result, “The url requested cannot be used to produce a matching route: url =” + URL_TO_TEST);
        //Assert.IsNotNull(result.Route);
        //}
    



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

        //[TestMethod]
        public void GetSlotPageById_WithSlotHavingAnImage_ReturnedSlotHasImageUrl()
        {
            //var slot = new Slot() { Id = 1, Headline = "test slot", Image = new Image() { Name = "text", Id = 1000, FileExtension = "jpg" } };
            //SlotPage sp = new SlotPage() { Id = 1, PageName = "Test", Slots = { slot } };
            //_slotRepository.Stub(r => r.GetSlotPageById(Arg<int>.Is.Anything)).Return(sp);

            
            //var result = _objectUnderTest.GetSlotPageById(1);
            //Image img = result.Slots.FirstOrDefault().Image;
            //Assert.IsNotNull(img.Url);
            
        }
        
    }
}
