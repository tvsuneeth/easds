using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TWG.EASDataService.ClientTest.Models
{
    public class EndPointIndexVM
    {
        public ServiceInstanceVM ServiceInstance { get; set; }
                

        public List<ServiceInstanceVM> AvailableInstances { get; set; }
    }
}