using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TWG.EASDataService.ClientTest.Models;
using TWG.EASDataService.ClientTest.Services;

namespace TWG.EASDataService.ClientTest.Controllers
{
    public class EASDataController : Controller
    {
        //
        // GET: /EASDATA/
        
        public IEndPointService endPointService;
        IServiceRequester requester;

        public EASDataController()
        {
            endPointService = new EndPointService();
            requester = new ServiceRequester();
        }

        public ActionResult Index()
        {
            var vm = new EASDataVM()
            { 
                AvailableInstances = PopulateInstanceDropDownList(), 
                AvailableMethods = PopulateMethodsDropDownList() 
            };            
            return View(vm);
        }



        [HttpPost]
        public ActionResult GetService(string instanceUrl,string methodName)
        {            
            try
            {
                var obj = requester.GetService(instanceUrl, methodName);
                if(obj==null)
                {
                    return Json("oops!!!An Error occured.", JsonRequestBehavior.AllowGet);
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("oops!!!An Error occured.", JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public ActionResult GetMediaContent(string instanceUrl, string methodName)
        {
            try
            {
                var response = requester.GetMediaContent(instanceUrl, methodName);
                Stream respStr = response.GetResponseStream();
                MemoryStream stream = new MemoryStream();
                respStr.CopyTo(stream);
                //return File(stream.ToArray(), response.ContentType);
                byte[] imageBytes = stream.ToArray();

                // Convert byte[] to Base64 String
               string  base64String = Convert.ToBase64String(imageBytes);

               //var anonym = 
               //return Content(base64String);
               return Json(new { itype = response.ContentType, idata = base64String }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Ooops!!! an eror occured. are you sure the requested media item exists??", JsonRequestBehavior.AllowGet);
            }
        }

        public List<SelectListItem> PopulateInstanceDropDownList()
        {
            var list = endPointService.GetAllAvailableEndPoints();
            var dropdownlistItems =list.Select(x => new SelectListItem() { Text = x.Name, Value = x.Url }).ToList();            
            return dropdownlistItems;
        }

        public List<SelectListItem> PopulateMethodsDropDownList()
        {          
            var list = endPointService.GetAllAvailableMethods();            
            list.Sort();
            var dropdownlistItems = list.Select(x => new SelectListItem() { Text = x, Value = x }).ToList();
            return dropdownlistItems;
        }


    }
}
