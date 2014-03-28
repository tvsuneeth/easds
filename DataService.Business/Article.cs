using System;

namespace twg.chk.DataService.Business
{
    public class Article : ContentBase
    {
        public String Introduction { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public MediaContent ThumbnailImage { get; set; }
    }
}
