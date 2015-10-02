using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TWG.EASDataService.ClientTest.Core
{
    public class AuthToken
    {        
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}