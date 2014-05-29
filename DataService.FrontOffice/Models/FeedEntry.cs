using System;
using System.Collections.Generic;
using twg.chk.DataService.Business;

namespace twg.chk.DataService.FrontOffice.Models
{
    public class FeedEntry
    {
        public Object Content { get; set; }
        public LinkItem Link { get; set; }
        public LinkItem ThumbnailImage { get; set; }        
        
    }
}