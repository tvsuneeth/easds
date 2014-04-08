using System;

namespace twg.chk.DataService.Business
{
    public class Article : ContentBase, IMediaAttachment
    {
        public String Introduction { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
