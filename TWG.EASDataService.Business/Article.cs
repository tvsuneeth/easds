using System;
using System.Collections.Generic;

namespace TWG.EASDataService.Business
{
    public class Article : ContentBase
    {
        public String Introduction { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }        
        public int NavigationId { get; set; }
        public ArticleTaxonomy Taxonomy { get; set; }
        public Image ThumbnailImage { get; set; }         
    }
}
