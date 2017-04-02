using System.IO;
using downr.Models;
using Microsoft.Extensions.Options;

namespace downr.Services
{
    public class InMemoryMarkdownPostIndexer : InMemoryMarkdownIndexer, IPostIndexer
    {
        public InMemoryMarkdownPostIndexer(IContentLoader markdownLoader, IOptions<DownrOptions> options)
            : base(markdownLoader, options)
        {
        }

        public override IContentIndexer Index(string contentPath, string slugPrefix = null)
        {
            slugPrefix = slugPrefix ?? "/";

            foreach (var subDir in Directory.GetDirectories(contentPath))
            {
                if (IndexContent(contentPath, subDir, MetadataType.Post, slugPrefix, out Metadata metadata))
                {
                    Metadata.Add(metadata.Slug, metadata);
                }
            }

            return this;
        }
    }
}