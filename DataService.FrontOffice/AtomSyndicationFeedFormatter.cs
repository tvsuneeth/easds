using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;

using twg.chk.DataService.api;
using twg.chk.DataService.FrontOffice.Models;
using twg.chk.DataService.Business;
using System.Xml.Linq;

namespace twg.chk.DataService.FrontOffice
{
    public class AtomSyndicationFeedFormatter: MediaTypeFormatter
    {
        private static readonly Dictionary<String, String> IMAGE_MIME_TYPES = new Dictionary<String, String>
        {
            {"jpg", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"bmp", "image/bmp"},
            {"gif", "image/gif"},
            {"jfif", "image/pjpeg"},
            {"png", "image/png"}
        };

        private static readonly String ATOM_CONTENT_TYPE = "application/atom+xml";
        private static readonly Dictionary<Type, Func<object, LinkItem, LinkItem, SyndicationItem>> _switchOnType = new Dictionary<Type, Func<object, LinkItem, LinkItem, SyndicationItem>>
            {
                {
                    typeof(ArticleSummary),
                    (object item, LinkItem link, LinkItem thumbnailImage) => { return BuildSyndicationItemForArticleSummary((ArticleSummary)item, link, thumbnailImage); }
                },
                {
                    typeof(Article),
                    (object item, LinkItem link, LinkItem thumbnailImage) => { return BuildSyndicationItemForArticle((Article)item, link, thumbnailImage); }
                },
                {
                    typeof(StaticPage),
                    (object item, LinkItem link, LinkItem thumbnailImage) => { return BuildSyndicationItemForStaticPage((StaticPage)item, link, thumbnailImage); }
                }
            };

        private IStaticContentLinkService _staticContentLinkService;

        public AtomSyndicationFeedFormatter(IStaticContentLinkService staticContentLinkService)
        {
            _staticContentLinkService = staticContentLinkService;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ATOM_CONTENT_TYPE));
        }

        public override bool CanReadType(Type type) { return false; }   //we allow only atom+xml document to be created

        public override bool CanWriteType(Type type)
        {
            return typeof(IFeed).IsAssignableFrom(type);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                if (typeof(IFeed).IsAssignableFrom(type))
                {
                    BuildSyndicationFeed((IFeed)value, writeStream, content.Headers.ContentType.MediaType);
                }
            });
        }

        private void BuildSyndicationFeed(IFeed feed, Stream stream, String contenttype)
        {
            var atomFeed = new SyndicationFeed()
            {
                Title = new TextSyndicationContent(feed.Link.Title),
                Id = feed.Link.Href,
                LastUpdatedTime = new DateTimeOffset(DateTime.UtcNow)
            };

            var feedUri = new Uri(feed.Link.Href);
            atomFeed.Links.Add(
            new SyndicationLink
            {
                Title = feed.Link.Title,
                Uri = feedUri,
                RelationshipType = feed.Link.Rel,
                MediaType = ATOM_CONTENT_TYPE
            });

            // Add parent, related, tag links to AtomFeed
            var navigationLinks = new List<LinkItem>();

           // if (feed.Parent != null) { navigationLinks.Add(feed.Parent); }
           // if (feed.Related != null) { navigationLinks.AddRange(feed.Related); }
           // if (feed.Tags != null) { navigationLinks.AddRange(feed.Tags); }
          //  if (feed.Children != null) { navigationLinks.AddRange(feed.Children); }
          //  if (feed.Static != null) { navigationLinks.AddRange(feed.Static); }
            
            // Add pagination information if exist
            if (feed is IPaginatedFeed)
            {
                var paginatedFeed = feed as IPaginatedFeed;
                if (paginatedFeed.FirstLink != null) { navigationLinks.Add(paginatedFeed.FirstLink); }
                if (paginatedFeed.LastLink != null) { navigationLinks.Add(paginatedFeed.LastLink); }
                if (paginatedFeed.NextLink != null) { navigationLinks.Add(paginatedFeed.NextLink); }
                if (paginatedFeed.PreviousLink != null) { navigationLinks.Add(paginatedFeed.PreviousLink); }
            }
            
            foreach (var link in navigationLinks)
            {
                atomFeed.Links.Add(
                new SyndicationLink
                {
                    Title = link.Title,
                    Uri = new Uri(link.Href),
                    RelationshipType = link.Rel,
                    MediaType = ATOM_CONTENT_TYPE
                });
            }
            
            var items = new List<SyndicationItem>();
            if (feed.Entries is IEnumerable)
            {
                foreach (var item in feed.Entries)
                {
                    var syndicationItem = _switchOnType[item.Content.GetType()](item.Content, item.Link, null);
                    items.Add(syndicationItem);
                }
            }
            else
            {
                var syndicationItem = _switchOnType[feed.Entries.GetType()](feed.Entries, feed.Link, null);
                items.Add(syndicationItem);
            }
            atomFeed.Items = items;

            using (var writer = XmlWriter.Create(stream))
            {
                Atom10FeedFormatter atomformatter = new Atom10FeedFormatter(atomFeed);
                atomformatter.WriteTo(writer);
            }
        }

        private static SyndicationItem BuildSyndicationItemForArticleSummary(ArticleSummary articleSummary, LinkItem link, LinkItem thumbnailImage)
        {
            var item = new SyndicationItem()
            {
                Id = link.Href,
                Title = new TextSyndicationContent(articleSummary.Title),
                LastUpdatedTime = new DateTimeOffset(articleSummary.LastModified),
                PublishDate = new DateTimeOffset(articleSummary.PublishedDate),
                Summary = new TextSyndicationContent(articleSummary.Introduction),
                Content = SyndicationContent.CreateUrlContent(new Uri(link.Href), ATOM_CONTENT_TYPE)
            };
            //item.Links.Add(new SyndicationLink { Title = link.Title, Uri = new Uri(link.Href), RelationshipType = link.Rel, MediaType =  });

            if (articleSummary.Author == null)
            {
                item.Authors.Add(new SyndicationPerson { Name = "anonymous" });
            }
            else
            {
                item.Authors.Add(new SyndicationPerson
                {
                    Name = articleSummary.Author.Names,
                    Email = articleSummary.Author.Email
                });
            }

            //Thumbnail image
            if (thumbnailImage != null)
            {
                var thumbnailLink = GetThumbnail(thumbnailImage.Href);
                if (thumbnailLink != null)
                {
                    item.Links.Add(thumbnailLink);
                }
            }

            return item;
        }

        private static SyndicationItem BuildSyndicationItemForArticle(Article article, LinkItem link, LinkItem thumbnailImage)
        {
            var item = new SyndicationItem()
            {
                Id = link.Href,
                Title = new TextSyndicationContent(article.Title),
                LastUpdatedTime = new DateTimeOffset(article.LastModified),
                PublishDate = new DateTimeOffset(article.PublishedDate),
                Summary = new TextSyndicationContent(article.Introduction),
                Content = SyndicationContent.CreateHtmlContent(article.Body)
            };
            item.Links.Add(new SyndicationLink { Title = link.Title, Uri = new Uri(link.Href), RelationshipType = link.Rel, MediaType = ATOM_CONTENT_TYPE });

            if (article.Author == null)
            {
                item.Authors.Add(new SyndicationPerson { Name = "anonymous" });
            }
            else
            {
                item.Authors.Add(new SyndicationPerson
                {
                    Name = article.Author.Names,
                    Email = article.Author.Email
                });
            }

            //Thumbnail image
            if (thumbnailImage != null)
            {
                var thumbnailLink = GetThumbnail(thumbnailImage.Href);
                if (thumbnailLink != null)
                {
                    item.Links.Add(thumbnailLink);
                }
            }

            return item;
        }

        private static SyndicationItem BuildSyndicationItemForStaticPage(StaticPage staticPage, LinkItem link, LinkItem thumbnailImage)
        {
            var item = new SyndicationItem()
            {
                Id = link.Href,
                Title = new TextSyndicationContent(staticPage.Title),
                LastUpdatedTime = new DateTimeOffset(staticPage.LastModified),
                Content = SyndicationContent.CreateHtmlContent(staticPage.Body)
            };
            item.Links.Add(new SyndicationLink { Title = link.Title, Uri = new Uri(link.Href), RelationshipType = link.Rel, MediaType = ATOM_CONTENT_TYPE });
            
            //Thumbnail image
            if (thumbnailImage != null)
            {
                var thumbnailLink = GetThumbnail(thumbnailImage.Href);
                if (thumbnailLink != null)
                {
                    item.Links.Add(thumbnailLink);
                }
            }

            return item;
        }

        private static SyndicationLink GetThumbnail(String imageUrl)
        {
            var fileExtension = Path.GetExtension(imageUrl).Replace(".", "").ToLower();
            if (IMAGE_MIME_TYPES.ContainsKey(fileExtension))
            {
                return new SyndicationLink { Uri = new Uri(imageUrl), RelationshipType = "enclosure", MediaType = IMAGE_MIME_TYPES[fileExtension] };
            }
            else
            {
                return null;
            }
        }
    }
}