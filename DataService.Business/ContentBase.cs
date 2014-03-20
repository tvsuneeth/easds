using System;
using System.Collections.Generic;

namespace twg.chk.DataService.Business
{
    public abstract class ContentBase
    {
        public virtual int Id { get; set; }
        public virtual String Title { get; set; }
        public virtual String Body { get; set; }
        public virtual DateTime LastModified { get; set; }
        public virtual List<TaxonomyItem> TaxonomyList { get; set; }
        public String MetaDescription { get; set; }
        public String MetaKeywords { get; set; }
    }
}
