using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;

using twg.chk.DataService.Business;

namespace twg.chk.DataService.FrontOffice.Models
{
    public class ContentFeed<tEntity> where tEntity : ITaxonomy
    {
        private tEntity _data;
        private UrlHelper _urlHelper;
        public ContentFeed(UrlHelper urlHelper, tEntity data)
        {
            _urlHelper = urlHelper;
            _data = data;

        }

        public tEntity Item { get { return _data; } }

        public LinkItem Parent
        {
            get
            {
                var parentItem = _data.GetParent();

                return new LinkItem
                {
                    Href = _urlHelper.Link("GetArticleByArticleSection", new { articleSection = parentItem.Name }),
                    Title = parentItem.Name,
                    Rel = "up",
                    Verb = "GET"
                };
            }
        }
        public List<LinkItem> Parents
        {
            get
            {
                return _data.GetArticleSections().Select(a =>
                    new LinkItem
                    {
                        Href = _urlHelper.Link("GetArticleByArticleSection", new { articleSection = a.Name }),
                        Title = a.Name,
                        Rel = "up",
                        Verb = "GET"
                    }).ToList();
            }
        }
        public List<LinkItem> Related
        {
            get
            {
                return _data.GetSectors().Select(a =>
                    new LinkItem
                    {
                        Href = _urlHelper.Link("GetArticleBySector", new { sector = a.Name }),
                        Title = a.Name,
                        Rel = "related",
                        Verb = "GET"
                    }).ToList();
            }
        }
        public List<LinkItem> Tags
        {
            get
            {
                return _data.GetTopics().Select(a =>
                    new LinkItem
                    {
                        Href = _urlHelper.Link("GetArticleByTopic", new { topic = a.Name }),
                        Title = a.Name,
                        Rel = "tag",
                        Verb = "GET"
                    }).ToList();
            }
        }
    }
}