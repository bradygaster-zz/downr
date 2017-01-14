using System;
using System.IO;
using Markdig;
using Microsoft.AspNetCore.Hosting;
using HtmlAgilityPack;

namespace downr.Services
{
    public interface IMarkdownContentLoader
    {
        string GetContentToRender(string slug);
    }

    public class DefaultMarkdownContentLoader : IMarkdownContentLoader
    {
        IHostingEnvironment _hostingEnvironment;

        public DefaultMarkdownContentLoader(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
        }
        public string GetContentToRender(string slug)
        {

            string path = Path.Combine(_hostingEnvironment.WebRootPath,"posts",slug,"index.md");

            // string path = string.Format("{0}\\posts\\{1}\\index.md",
            //     _hostingEnvironment.WebRootPath,
            //     slug);

            using (var rdr = File.OpenText(path))
            {
                var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
                var html = Markdig.Markdown.ToHtml(rdr.ReadToEnd(), pipeline);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                try
                {
                    foreach (HtmlNode node in htmlDoc.DocumentNode
                                       .SelectNodes("//img[@src]"))
                    {
                        var src = node.Attributes["src"].Value;
                        src = src.Replace("media/", string.Format("/posts/{0}/media/", slug));
                        node.SetAttributeValue("src", src);
                    }
                }
                catch (NullReferenceException)
                {
                    // no images found, keep going
                }

                html = htmlDoc.DocumentNode.OuterHtml;

                return html;
            }
        }
    }
}