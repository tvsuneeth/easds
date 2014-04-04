using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Models
{
    public abstract class Feed<tEntity> where tEntity : ITaxonomy
    {
        protected tEntity _feedContent;
        protected IUrlHelper _urlHelper;
        protected String _feedUrl;

        public Feed(String feedUrl, tEntity data, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            _feedContent = data;
            _feedUrl = feedUrl;
        }

        public LinkItem Parent
        {
            get
            {
                var parentItem = _feedContent.GetParent();

                if (parentItem != null)
                {
                    return new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection = parentItem.Name }),
                        Title = parentItem.Name,
                        Rel = "up",
                        Verb = "GET"
                    };
                }
                else
                {
                    return new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetRoot", null),
                        Title = "Home",
                        Rel = "up",
                        Verb = "GET"
                    };
                }
            }
        }
        public List<LinkItem> Parents
        {
            get
            {
                var sections = _feedContent.GetArticleSections();
                if (sections != null)
                {
                    var parentList = sections.Select(a =>
                        new LinkItem
                        {
                            Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection = a.Name }),
                            Title = a.Name,
                            Rel = "up",
                            Verb = "GET"
                        }).ToList();

                    parentList.Add(
                        new LinkItem
                        {
                            Href = _urlHelper.GenerateUrl("GetRoot", null),
                            Title = "Home",
                            Rel = "up",
                            Verb = "GET"
                        }
                    );

                    return parentList.OrderBy(l => l.Href).ToList();
                }
                else
                {
                    return new List<LinkItem> {
                        new LinkItem
                        {
                            Href = _urlHelper.GenerateUrl("GetRoot", null),
                            Title = "Home",
                            Rel = "up",
                            Verb = "GET"
                        }
                    };
                }
            }
        }
        public List<LinkItem> Related
        {
            get
            {
                var sectors = _feedContent.GetSectors();
                if (sectors != null)
                {
                    return sectors.Select(a =>
                        new LinkItem
                        {
                            Href = _urlHelper.GenerateUrl("GetArticleBySector", new { sector = a.Name }),
                            Title = a.Name,
                            Rel = "related",
                            Verb = "GET"
                        }).ToList();
                }
                else
                {
                    return null;
                }
            }
        }
        public List<LinkItem> Tags
        {
            get
            {
                var topics = _feedContent.GetTopics();
                if (topics != null)
                {
                    return topics.Select(a =>
                        new LinkItem
                        {
                            Href = _urlHelper.GenerateUrl("GetArticleByTopic", new { topic = a.Name }),
                            Title = a.Name,
                            Rel = "tag",
                            Verb = "GET"
                        }).ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public abstract LinkItem Link { get; }
        internal abstract tEntity FeedContent();
        public abstract dynamic Entry { get; }
    }
}