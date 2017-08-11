using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace downr.Controllers
{
    public class FeedController : Controller
    {
        private readonly PostService _postService;
        private readonly DownrOptions _options;

        public FeedController(PostService postService,
            IOptions<DownrOptions> options)
        {
            _postService = postService;
            _options = options.Value;
        }

        public IActionResult Rss()
        {
            // get the last 10 posts
            var last10posts = _postService.GetPosts(count: 10);

            var feed = BuildXmlFeed(last10posts);
            return Content(feed, "text/xml");
        }

        public string BuildXmlFeed(IEnumerable<Metadata> posts)
        {
            StringWriter parent = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(parent, new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            }))
            {
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("version", "2.0");
                //writer.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");

                // write out 
                writer.WriteStartElement("channel");

                var rootUri = new Uri(_options.RootUrl);
                // write out -level elements
                writer.WriteElementString("title", _options.Title);
                writer.WriteElementString("link", rootUri.ToString());
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
                        var relativeUrl = Url.Action("Post", "Posts", new {article.Slug});
                        writer.WriteElementString("link", new Uri(rootUri, relativeUrl).ToString());
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