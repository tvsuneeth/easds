using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class Category
    {
        public Category()
        {
            Items = new List<CategoryItem>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public List<CategoryItem> Items { get; set; }
    }
}
