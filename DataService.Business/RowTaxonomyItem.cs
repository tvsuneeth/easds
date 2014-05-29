using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class RowTaxonomyItem
    {
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryItemName { get; set; }                
        public int? ParentId { get; set; }
    }
}
