using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public enum TaxonomyCategories { ArticleSection = 1, Sector = 3, Topic = 2 };
    public class TaxonomyItem : IWebIdentifiable
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int? ParentId { get; set; }
        public TaxonomyCategories Category { get; set; }

        public object GetIdentificationElement() { return new { Name }; }
        public String GetIdentificationTitle() { return Name; }
    }
}
