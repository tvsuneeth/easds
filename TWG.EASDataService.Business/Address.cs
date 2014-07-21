using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Business
{
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}
