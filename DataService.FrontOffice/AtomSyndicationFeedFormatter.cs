using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;

using twg.chk.DataService.FrontOffice.Models;

namespace twg.chk.DataService.FrontOffice
{
    public class AtomSyndicationFeedFormatter: MediaTypeFormatter
    {
        private readonly String atom = "application/atom+xml";

        public AtomSyndicationFeedFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(atom));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Uri);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                if (type == typeof(Uri))
                    BuildSyndicationFeed(value, writeStream, content.Headers.ContentType.MediaType);
            });
        }

        private void BuildSyndicationFeed(object uri, Stream stream, String contenttype)
        {
            var url = (uri as Uri).OriginalString;

            var reader = new XmlTextReader(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);

            SyndicationFeedFormatter formatter = new Atom10FeedFormatter(feed);

            using (var writer = XmlWriter.Create(stream))
            {
                formatter.WriteTo(writer);
                writer.Flush();
                writer.Close();
            }
        }
    }
}