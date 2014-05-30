using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{

    public class TaxonomyCategory
    {
        public TaxonomyCategory()
        {            
        }
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public List<TaxonomyCategoryItem> CategoryItems { get; private set; }

        public void AddItem(TaxonomyCategoryItem item)
        {
            if (CategoryItems == null)
            {
                CategoryItems = new List<TaxonomyCategoryItem>();
            }
            CategoryItems.Add(item);
        }
    }
   
}
