using System;
using System.Collections.Generic;
using System.Linq;
using twg.chk.DataService.api;
using twg.chk.DataService.Business;
using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Models
{
    public class SingleContentFeed<T> : Feed<T> where T : ITaxonomy, IWebIdentifiable
    {
        public SingleContentFeed(String feedUrl, T feedData, IUrlHelper urlHelper, IStaticContentLinkService staticContentLinkService)
            : base(feedUrl, feedData, urlHelper, staticContentLinkService)
        {
            Link = new LinkItem { Href = _feedUrl, Title = _feedContent.GetIdentificationTitle(), Rel = "self", Verb = "GET" };

           
            var entry = new FeedEntry 
            { 
                Content = feedData, 
                Link = Link,               
            };
            
            // Create thumbnail link item if exist
            //if (feedData.HasAttachedMedia)
            //{
            //    entry.ThumbnailImage = new LinkItem
            //    {
            //        Href = _urlHelper.GenerateUrl("GetMedia",
            //            new
            //            {
            //                id = feedData.AttachedMedia.Id,
            //                fileName = String.Format("{0}.{1}", feedData.AttachedMedia.FileName, feedData.AttachedMedia.Extension)
            //            })
            //    };
            //}
            
            Entries = new List<FeedEntry> { entry };
        }

        public override IEnumerable<FeedEntry> Entries { get; protected set; }
      
    }
}