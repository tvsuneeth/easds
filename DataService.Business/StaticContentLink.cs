using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class StaticContentLink
    {
        public int Id { get; set; }
        public String PartialUrl { get; set; }
        public String Title { get; set; }
        public String Rel { get; set; }
    }
}
