using System;
using System.IO;
using Markdig;
using Microsoft.AspNetCore.Hosting;

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
            string path = string.Format("{0}\\posts\\{1}\\index.md",
                _hostingEnvironment.WebRootPath,
                slug);

            using(var rdr = File.OpenText(path))
            {
                var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
                var html = Markdig.Markdown.ToHtml(rdr.ReadToEnd(), pipeline);
                return html;
            }
        }
    }
}