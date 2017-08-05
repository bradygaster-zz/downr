using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class FeedController : Controller
    {
        IMarkdownContentLoader _markdownLoader;
        IYamlIndexer _indexer;

        public FeedController(IMarkdownContentLoader markdownLoader,
            IYamlIndexer indexer)
        {
            _markdownLoader = markdownLoader;
            _indexer = indexer;
        }

        public IActionResult Rss()
        {
            // get the last 10 posts
            var last10posts = _indexer.Metadata.Take(10);


            var feed = BuildXmlFeed(last10posts);
            return Content(feed, "text/xml");
        }

        public string BuildXmlFeed(IEnumerable<Metadata> posts)
        {
            StringWriter parent = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(parent, new XmlWriterSettings {
                OmitXmlDeclaration = true
            }))
            {
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("version", "2.0");
                //writer.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");

                // write out 
                writer.WriteStartElement("channel");

                // write out -level elements
                writer.WriteElementString("title", $"bradygaster.com");
                writer.WriteElementString("link", "http://bradygaster.com");
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
                        writer.WriteElementString("link", "http://bradygaster.com/" + article.Slug); // todo build article path
                        writer.WriteElementString("description", article.Content);

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