using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TWG.EASDataService.ClientTest.Models
{
    public class ServiceInstanceVM
    {
        [Required(ErrorMessage="Enter an endpoint Url")]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Enter a valid endpoint Url")]
        public string Url {get; set; }

        public string Name { get; set; }
    }
}