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

        [HttpGet]
        public ActionResult Index()
        {
            EndPointVM obj = new EndPointVM();
            return View(obj); ;
        }

        [HttpPost]
        public ActionResult submit(EndPointVM ep)
        {
            if (ModelState.IsValid)
            {
                //Session["ServiceEndPointUrl"] = ep.Url;
                EndPointService endpointSrvc = new EndPointService();
                endpointSrvc.SetEndPoint(ep);
                return RedirectToAction("Index", "Home");
            }
            return View("Index",ep);
        }
    }
}
