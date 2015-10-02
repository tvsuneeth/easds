using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TWG.EASDataService.ClientTest.Models;
using TWG.EASDataService.ClientTest.Services;

namespace TWG.EASDataService.ClientTest.Controllers
{
    public class EndPointController : Controller
    {
        //
        // GET: /EndPoint/

        public EndPointService endpointSrvc = new EndPointService();

        [HttpGet]
        public ActionResult Index()
        {                     
            EndPointIndexVM vm = new EndPointIndexVM();
            vm.ServiceInstance = new ServiceInstanceVM();            
            vm.AvailableInstances = endpointSrvc.GetAllAvailableEndPoints();
            return View(vm); ;
        }

        [HttpPost]
        public ActionResult submit(EndPointIndexVM vm)
        {
                        
            if (ModelState.IsValid)
            {                
                EndPointService endpointSrvc = new EndPointService();
                ServiceInstanceVM sivm = endpointSrvc.GetAllAvailableEndPoints().Where(x => x.Url == vm.ServiceInstance.Url).First();
                endpointSrvc.SetEndPoint(sivm);                

                if (!Request.IsAjaxRequest())
                {
                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    return Content("success");
                }
            }           
            return View("Index",vm);
        }
    }
}
