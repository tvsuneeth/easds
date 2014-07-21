using System;
using System.Collections.Generic;
using System.Linq;

using TWG.EASDataService.Business;
using TWG.EASDataService.Services;
using TWG.EASDataService.Api.Helpers;
using TWG.EASDataService.Data.Repository;

namespace TWG.EASDataService.Api.Models
{
    public class MultipleContentFeed<T> : Feed<PagedResult<T>>, IPaginatedFeed where T : IWebIdentifiable, IMediaAttachment
    {
        private String _feedEntriesRouteName;
        private String _feedTitle;
        public MultipleContentFeed(String feedUrl, String feedTitle, PagedResult<T> feedContent, IUrlHelper urlHelper, IStaticContentLinkService staticContentLinkService, String feedEntriesRouteName)
            : base(feedUrl, feedContent, urlHelper, staticContentLinkService)
        {
            _feedTitle = feedTitle;
            _feedEntriesRouteName = feedEntriesRouteName;

            var contentList = new List<FeedEntry>();
            foreach (T content in feedContent)
            {

                var link = _urlHelper.GenerateUrl(_feedEntriesRouteName, content.GetIdentificationElement());
                var entry = new FeedEntry
                {
                    Content = content,
                    Link = new LinkItem { Href = link, Title = content.GetIdentificationTitle(), Rel = "via", Verb = "GET" }
                };
                //if (content.HasAttachedMedia)
                //{
                //    entry.ThumbnailImage = new LinkItem
                //    {
                //        Href = _urlHelper.GenerateUrl("GetMedia", 
                //            new
                //            {
                //                id = content.AttachedMedia.Id,
                //                fileName = String.Format("{0}.{1}", content.AttachedMedia.FileName, content.AttachedMedia.Extension)
                //            })
                //    };
                //}
              
                
                // We add a link property based on the identification element given by the content object (it implement IWebIdentifiable)
                contentList.Add(entry);
            }

            Entries = contentList;
        }

        public LinkItem PreviousLink { get; private set; }
        public LinkItem NextLink { get; private set; }
        public LinkItem FirstLink { get; private set; }
        public LinkItem LastLink { get; private set; }

        public void SetPreviousLink(String routeName, String linkTitle, dynamic routeAttributes)
        {
            PreviousLink = new LinkItem
            {
                Href = _urlHelper.GenerateUrl(routeName, routeAttributes),
                Rel = "prev",
                Title = linkTitle,
                Verb = "GET"
            };
        }

        public void SetNextLink(String routeName, String linkTitle, dynamic routeAttributes)
        {
            NextLink = new LinkItem
            {
                Href = _urlHelper.GenerateUrl(routeName, routeAttributes),
                Rel = "next",
                Title = linkTitle,
                Verb = "GET"
            };
        }

        public void SetFirstLink(String routeName, String linkTitle, dynamic routeAttributes)
        {
            FirstLink = new LinkItem
            {
                Href = _urlHelper.GenerateUrl(routeName, routeAttributes),
                Rel = "first",
                Title = linkTitle,
                Verb = "GET"
            };
        }

        public void SetLastLink(String routeName, String linkTitle, dynamic routeAttributes)
        {
            LastLink = new LinkItem
            {
                Href = _urlHelper.GenerateUrl(routeName, routeAttributes),
                Rel = "last",
                Title = linkTitle,
                Verb = "GET"
            };
        }

        public override LinkItem Link { get { return new LinkItem { Href = _feedUrl, Title = _feedTitle, Rel = "self", Verb = "GET" }; } }

        public override IEnumerable<FeedEntry> Entries { get; protected set; }
    }
}