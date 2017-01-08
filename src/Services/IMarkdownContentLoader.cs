using System;
using System.IO;
using Markdig;
using Microsoft.AspNetCore.Hosting;
using HtmlAgilityPack;

namespace downr.Services
{
    public interface IMarkdownContentLoader
    {
        string GetContentToRender(string content, string contentFolder);
    }
}