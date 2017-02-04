using System;
using Markdig;
using HtmlAgilityPack;

namespace downr.Services
{
    public class DefaultMarkdownContentLoader : IMarkdownContentLoader
    {
        public string GetContentToRender(string content, string contentFolder)
        {
            var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
            var html = Markdown.ToHtml(content, pipeline);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            try
            {
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//img[@src]"))
                {
                    var src = node.Attributes["src"].Value;
                    src = src.Replace("media/", string.Format("/{0}/media/", contentFolder));
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