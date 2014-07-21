using System;
using System.Collections.Generic;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.Api.Models
{
    public class FeedEntry
    {
        public Object Content { get; set; }
        public LinkItem Link { get; set; }
        //public LinkItem ThumbnailImage { get; set; }        
        
    }
}