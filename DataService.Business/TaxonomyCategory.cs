using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{

    public class TaxonomyCategory
    {               
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public List<TaxonomyCategoryItem> CategoryItems { get; set; }
    }
   
}
