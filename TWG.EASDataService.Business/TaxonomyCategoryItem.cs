using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWG.EASDataService.Business
{
    public class TaxonomyCategoryItem
    {
        public int CategoryItemId { get; set; }
        public string CategoryItemName { get; set; }
        public int? ParentItemId { get; set; }
    }
}
