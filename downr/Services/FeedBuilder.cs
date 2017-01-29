using System.Collections.Generic;
using System.IO;
using System.Xml;
using downr.Models;
using Microsoft.Extensions.Options;

namespace downr.Services
{
    public class FeedBuilder : IFeedBuilder
    {
        private readonly DownrOptions _options;
        public FeedBuilder(IOptions<DownrOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public string BuildXmlFeed(IEnumerable<Metadata> posts)
        {
            StringWriter parent = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(parent, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("version", "2.0");
                //writer.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");

                // write out 
                writer.WriteStartElement("channel");

                // write out -level elements
                writer.WriteElementString("title", _options.Title);
                writer.WriteElementString("link", _options.Url);
                writer.WriteElementString("ttl", "60");

                //writer.WriteStartElement("atom:link");
                //writer.WriteAttributeString("rel", "self");
                //writer.WriteAttributeString("type", "application/rss+xml");
                //writer.WriteEndElement();

                if (posts != null)
                {
                    foreach (var article in posts)
                    {
                        writer.WriteStartElement("item");

                        writer.WriteElementString("title", article.Title);
                        writer.WriteElementString("link", _options.Url + article.Slug); // todo build article path
                        writer.WriteElementString("description", article.Content.StripHtml().Excerpt(150)); // show just an excerpt without html stuff

                        writer.WriteEndElement();
                    }
                }

                // write out 
                writer.WriteEndElement();

                // write out 
                writer.WriteEndElement();
            }

            return parent.ToString();
        }
    }
}
