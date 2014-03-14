using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class TaxonomyItem
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int ParentId { get; set; }
    }
}
