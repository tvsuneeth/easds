using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using twg.chk.DataService.Business;
using twg.chk.DataService.api;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Models
{
    public interface IFeed
    {
        LinkItem Parent { get; }
        List<LinkItem> Parents { get; }
        List<LinkItem> Children { get; }
        List<LinkItem> Related { get; }
        List<LinkItem> Tags { get; }
        List<LinkItem> Static { get; }
        LinkItem Link { get; }
        Object FeedContent { get; }
        IEnumerable<FeedEntry> Entries { get; }
    }

    public abstract class Feed<tEntity> : IFeed where tEntity : ITaxonomy
    {
        protected tEntity _feedContent;
        protected IUrlHelper _urlHelper;
        protected String _feedUrl;
        private IStaticContentLinkService _staticContentLinkService;

        public Feed(String feedUrl, tEntity data, IUrlHelper urlHelper, IStaticContentLinkService staticContentLinkService)
        {
            _urlHelper = urlHelper;
            FeedContent =_feedContent = data;
            _feedUrl = feedUrl;
            _staticContentLinkService = staticContentLinkService;

            // Set navigations links based on taxonomy items
            SetParent();
            SetParents();
            SetChildren();
            SetRelated();
            SetTags();
            SetStatics();
        }

        private void SetParent()
        {
            var parentItem = _feedContent.GetParentArticleSection();

            if (parentItem != null)
            {
                Parent = new LinkItem
                {
                    Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection = parentItem.Name }),
                    Title = parentItem.Name,
                    Rel = "up",
                    Verb = "GET"
                };
            }
            else
            {
                Parent = new LinkItem
                {
                    Href = _urlHelper.GenerateUrl("GetRoot", null),
                    Title = "Home",
                    Rel = "up",
                    Verb = "GET"
                };
            }
        }
        private void SetParents()
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

                Parents = parentList.OrderBy(l => l.Href).ToList();
            }
            else
            {
                Parents = new List<LinkItem> {
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
        private void SetChildren()
        {
            var childrenArticleSection = _feedContent.GetChildrenArticleSection();
            if (childrenArticleSection != null )
            {
                Children = childrenArticleSection.Select(a =>
                    new LinkItem
                    {
                        Href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection = a.Name }),
                        Title = a.Name,
                        Rel = "subsection",
                        Verb = "GET"
                    }).ToList();
            }
            else
            {
                Children = null;
            }
        }
        private void SetRelated()
        {
            var sectors = _feedContent.GetSectors();
            if (sectors != null)
            {
                Related = sectors.Select(a =>
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
                Related = null;
            }
        }
        private void SetTags()
        {
            var topics = _feedContent.GetTopics();
            if (topics != null)
            {
                Tags = topics.Select(a =>
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
                Tags = null;
            }
        }
        private void SetStatics()
        {
            Static = new List<LinkItem>();

            // Fetch static links
            var staticItems = _staticContentLinkService.GetStaticContentLinkForSite();

            foreach (var staticItem in staticItems)
            {
                var href = String.Empty;
                var rel = String.Empty;
                switch (staticItem.LinkType)
                {
                    case StaticLinkType.Article:
                        href = _urlHelper.GenerateUrl("GetArticleById", new { id = int.Parse(staticItem.IdentificationValue) });
                        rel = "item";
                        break;
                    case StaticLinkType.StaticPage:
                        href = _urlHelper.GenerateUrl("GetStaticPageByName", new { name = staticItem.IdentificationValue });
                        rel = "item";
                        break;
                    case StaticLinkType.ArticleSection:
                        href = _urlHelper.GenerateUrl("GetArticleByArticleSection", new { articleSection = staticItem.IdentificationValue });
                        rel = "chapter";
                        break;
                    case StaticLinkType.Sector:
                        href = _urlHelper.GenerateUrl("GetArticleBySector", new { sector = staticItem.IdentificationValue });
                        rel = "chapter";
                        break;
                    case StaticLinkType.Topic:
                        href = _urlHelper.GenerateUrl("GetArticleByTopic", new { topic = staticItem.IdentificationValue });
                        rel = "chapter";
                        break;
                }

                Static.Add(new LinkItem { Href = href, Title = staticItem.Title, Rel = rel, Verb = "GET" });
            }
        }


        public LinkItem Parent { get; private set; }
        public List<LinkItem> Parents { get; private set; }
        public List<LinkItem> Children { get; private set; }
        public List<LinkItem> Related { get; private set; }
        public List<LinkItem> Tags { get; private set; }
        public List<LinkItem> Static { get; private set; }

        public virtual LinkItem Link { get; protected set; }

        public virtual Object FeedContent { get; protected set; }
        public abstract IEnumerable<FeedEntry> Entries { get; protected set; }
    }
}