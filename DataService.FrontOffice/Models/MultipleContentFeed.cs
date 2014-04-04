using System;
using System.Collections.Generic;
using System.Linq;

using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Models
{
    public class MultipleContentFeed<L, T> : Feed<L> where L : ITaxonomy, IEnumerable<T> where T : IWebIdentifiable
    {
        private String _feedContentRouteName;
        private String _feedTitle;
        public MultipleContentFeed(String feedUrl, String feedTitle, L feedContent, IUrlHelper urlHelper, String feedContentRouteName)
            : base(feedUrl, feedContent, urlHelper)
        {
            _feedTitle = feedTitle;
            _feedContentRouteName = feedContentRouteName;
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
        internal override L FeedContent() { return _feedContent; }

        public override dynamic Entry
        {
            get
            {
                var contentList = new List<dynamic>();
                foreach (T content in _feedContent)
                {
                    dynamic dynContent = new System.Dynamic.ExpandoObject();
                    dynContent.Content = content;

                    // We add a link property based on the identification element given by the content object (it implement IWebIdentifiable)
                    var link = _urlHelper.GenerateUrl(_feedContentRouteName, content.GetIdentificationElement());
                    dynContent.Link = new LinkItem { Href = link, Title = content.GetIdentificationTitle(), Rel = "self", Verb = "GET" };

                    contentList.Add(dynContent);
                }

                return contentList;
            }
        }
    }
}