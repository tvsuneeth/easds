using System;
using System.Collections.Generic;

namespace TWG.EASDataService.Business
{
    public interface ITaxonomy
    {
        TaxonomyItem GetParentArticleSection();
        List<TaxonomyItem> GetChildrenArticleSection();
        List<TaxonomyItem> GetArticleSections();
        List<TaxonomyItem> GetSectors();
        List<TaxonomyItem> GetTopics();
        //ArticleTaxonomy GetTaxonomy();
    }
}
