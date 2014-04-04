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

        public LinkItem PreviousLink { get; set; }
        public LinkItem NextLink { get; set; }
        public LinkItem FirstLink { get; set; }
        public LinkItem LastLink { get; set; }

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