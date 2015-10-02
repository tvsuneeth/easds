using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TWG.EASDataService.ClientTest.Models;

namespace TWG.EASDataService.ClientTest.Services
{
    public interface IEndPointService
    {
        List<ServiceInstanceVM> GetAllAvailableEndPoints();
        List<string> GetAllAvailableMethods();
    }
    public class EndPointService : IEndPointService
    {

        public void SetEndPoint(ServiceInstanceVM si)
        {
            HttpContext.Current.Session["ServiceEndPointUrl"] = si;
        }
        public string GetCurrentEndPointUrl()
        {
            string url = string.Empty;
            if(HttpContext.Current.Session["ServiceEndPointUrl"]!=null)
            {
                var obj = (ServiceInstanceVM)HttpContext.Current.Session["ServiceEndPointUrl"];
                url = obj.Url;
                url = (!url.EndsWith("/")) ? url + "/" : url;
            }
            return url;
        }

        public List<ServiceInstanceVM> GetAllAvailableEndPoints()
        {
            List<ServiceInstanceVM> list = new List<ServiceInstanceVM>();
            string filePath = HttpContext.Current.Server.MapPath("~")+ "\\" + "serviceInstances.xml";
            XDocument xdoc = XDocument.Load(filePath);
            var instances = xdoc.Element("ServiceInstances").Elements();
            foreach(var instance in instances)
            {
                list.Add(new ServiceInstanceVM() 
                    {
                        Name = instance.Element("Name").Value.Trim() , 
                        Url =  instance.Element("Url").Value.Trim()
                    });
            }
            return list;
        }

        public List<string> GetAllAvailableMethods()
        {
            List<string> list = new List<string>();
            string filePath = HttpContext.Current.Server.MapPath("~") + "\\" + "serviceMethods.xml";
            XDocument xdoc = XDocument.Load(filePath);
            var methods = xdoc.Element("ServiceMethods").Elements();
            list = methods.Select(x => x.Attribute("Name").Value).ToList();
            return list;
        }
    }
}