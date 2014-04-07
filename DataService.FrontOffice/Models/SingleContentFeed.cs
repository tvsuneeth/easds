using System;
using System.Collections.Generic;
using System.Linq;

using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Models
{
    public class SingleContentFeed<T> : Feed<T> where T : ITaxonomy, IWebIdentifiable
    {
        public SingleContentFeed(String feedUrl, T feedData, IUrlHelper urlHelper)
            : base(feedUrl, feedData, urlHelper) { }

        public override LinkItem Link { get { return new LinkItem { Href = _feedUrl, Title = _feedContent.GetIdentificationTitle(), Rel = "self", Verb = "GET" }; } }
        public override dynamic Entry { get { return (dynamic)_feedContent; } }
    }
}