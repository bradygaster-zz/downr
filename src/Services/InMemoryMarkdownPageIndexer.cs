using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using downr.Models;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;

namespace downr.Services
{
    public class InMemoryMarkdownPageIndexer : InMemoryMarkdownIndexer, IPageIndexer
    {
        public InMemoryMarkdownPageIndexer(IContentLoader markdownLoader, IOptions<DownrOptions> options) :
            base(markdownLoader, options)
        {
        }

        public override IContentIndexer Index(string contentPath)
        {
            var stack = new Stack<string>();
            foreach (var subDir in Directory.GetDirectories(contentPath))
            {
                stack.Push(subDir);
            }

            while (stack.Count > 0)
            {
                var currentDir = stack.Pop();

                // determine all files
                if (IndexContent(contentPath, currentDir, MetadataType.Page, out Metadata metadata))
                {
                    Metadata.Add(metadata.Slug, metadata);
                }

                // support page sub folders
                foreach (var subDir in Directory.GetDirectories(currentDir))
                {
                    if (IndexContent(contentPath, subDir, MetadataType.Page, out Metadata subMetadata))
                    {
                        Metadata.Add(subMetadata.Slug, subMetadata);
                    }
                }
            }

            return this;
        }
    }

}
