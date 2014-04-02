using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;

using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice.Helpers;

namespace twg.chk.DataService.FrontOffice.Models
{
    public class ContentFeed<tEntity> where tEntity : ITaxonomy
    {
        private tEntity _data;
        private UrlHelper _urlHelper;
        private IContentFeedHelper _contentFeedHelper;
        public ContentFeed(UrlHelper urlHelper, tEntity data, IContentFeedHelper contentFeedHelper)
        {
            _urlHelper = urlHelper;
            _contentFeedHelper = contentFeedHelper;
            _data = data;

        }

        public tEntity Item { get { return _data; } }

        public LinkItem Parent
        {
            get
            {
                var parentItem = _data.GetParent();

                if (parentItem != null)
                {
                    return new LinkItem
                    {
                        Href = _contentFeedHelper.GenerateLink(_urlHelper, "GetArticleByArticleSection", new { articleSection = parentItem.Name }),
                        Title = parentItem.Name,
                        Rel = "up",
                        Verb = "GET"
                    };
                }
                else
                {
                    return new LinkItem
                    {
                        Href = _contentFeedHelper.GenerateLink(_urlHelper, "GetRoot", null),
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
                var sections = _data.GetArticleSections();
                if (sections != null)
                {
                    var parentList = _data.GetArticleSections().Select(a =>
                        new LinkItem
                        {
                            Href = _contentFeedHelper.GenerateLink(_urlHelper, "GetArticleByArticleSection", new { articleSection = a.Name }),
                            Title = a.Name,
                            Rel = "up",
                            Verb = "GET"
                        }).ToList();

                    parentList.Add(
                        new LinkItem
                        {
                            Href = _contentFeedHelper.GenerateLink(_urlHelper, "GetRoot", null),
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
                            Href = _contentFeedHelper.GenerateLink(_urlHelper, "GetRoot", null),
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
                var sectors = _data.GetSectors();
                if (sectors != null)
                {
                    return _data.GetSectors().Select(a =>
                        new LinkItem
                        {
                            Href = _contentFeedHelper.GenerateLink(_urlHelper, "GetArticleBySector", new { sector = a.Name }),
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
                var topics = _data.GetTopics();
                if (topics != null)
                {
                    return _data.GetTopics().Select(a =>
                        new LinkItem
                        {
                            Href = _contentFeedHelper.GenerateLink(_urlHelper, "GetArticleByTopic", new { topic = a.Name }),
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
    }
}