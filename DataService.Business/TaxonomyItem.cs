using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public enum TaxonomyCategories { ArticleSection = 1, Sector = 3, Topic = 2 };
    public class TaxonomyItem
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int? ParentId { get; set; }
        public TaxonomyCategories Category { get; set; }
    }
}
