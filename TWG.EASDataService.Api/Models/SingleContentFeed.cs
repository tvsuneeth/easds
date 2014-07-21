using System;
using System.Collections.Generic;
using System.Linq;
using TWG.EASDataService.Services;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Repository;
using TWG.EASDataService.Api.Helpers;

namespace TWG.EASDataService.Api.Models
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