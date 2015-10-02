using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TWG.EASDataService.ClientTest.Models
{
    public class EASDataVM
    {
        [Required(ErrorMessage="Enter a method")]
        public string MethodName { get; set; }

        public List<SelectListItem> AvailableInstances { get; set; }

        [Required(ErrorMessage = "Select a service Instance")]
        public string SelectedInstance { get; set; }


        public List<SelectListItem> AvailableMethods { get; set; }

        [Required(ErrorMessage = "Select a Method")]
        public string SelectedMethod { get; set; }



        [Required(ErrorMessage = "Input a value")]
        public string MethodParam1 { get; set; }
    }
}