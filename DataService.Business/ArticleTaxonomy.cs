using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twg.chk.DataService.Business
{
    public class ArticleTaxonomy 
    {                   
           public IList<TaxonomyCategory> CategoryAssignments { get; set; }
           public ArticleSection ParentSection { get; set; }           
    }
}