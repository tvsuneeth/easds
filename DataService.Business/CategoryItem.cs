using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class CategoryItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
       // public List<CategoryItem> Items { get; set; }

       /* public void AddChild(CategoryItem child)
        {
            if (Items == null)
            {
                Items = new List<CategoryItem>();
            }
            Items.Add(child);
        }*/
    }
}
