using System;
using System.Collections.Generic;

namespace twg.chk.DataService.Business
{
    public interface ITaxonomy
    {
        TaxonomyItem GetParent();
        List<TaxonomyItem> GetArticleSections();
        List<TaxonomyItem> GetSectors();
        List<TaxonomyItem> GetTopics();
    }
}
