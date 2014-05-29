﻿using System;
using System.Collections.Generic;

namespace twg.chk.DataService.Business
{
    public class Article : ContentBase, IMediaAttachment
    {
        public String Introduction { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }        
        public int NavigationId { get; set; }
        public ArticleTaxonomy Taxonomy { get; set; }         
    }
}
