using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using downr.Models;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;

namespace downr.Services
{
    /// <summary>
    /// Implementation of an in memory yaml indexer
    /// </summary>
    public abstract class InMemoryMarkdownIndexer : IContentIndexer
    {
        private readonly IContentLoader _markdownLoader;
        private readonly DownrOptions _options;

        /// <summary>
        /// Creates an instance of <see cref="InMemoryMarkdownIndexer"/>
        /// </summary>
        protected InMemoryMarkdownIndexer(IContentLoader markdownLoader, IOptions<DownrOptions> options)
        {
            if (markdownLoader == null) throw new ArgumentNullException(nameof(markdownLoader));
            if (options == null) throw new ArgumentNullException(nameof(options));

            _markdownLoader = markdownLoader;
            _options = options.Value;
            Metadata = new ConcurrentDictionary<string, Metadata>(StringComparer.OrdinalIgnoreCase);
            TagCloud = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the tag cloud of all present content
        /// </summary>
        public IDictionary<string, int> TagCloud { get; }

        /// <summary>
        /// Cache for all metadata
        /// </summary>
        public IDictionary<string, Metadata> Metadata { get; }

        /// <summary>
        /// Returns true and sets <see cref="Metadata"/> if <paramref name="slug"/> exists in indexer
        /// </summary>
        public bool TryGet(string slug, out Metadata metadata)
        {
            return Metadata.TryGetValue(slug, out metadata);
        }

        /// <summary>
        /// Indexer logic
        /// </summary>
        public abstract IContentIndexer Index(string contentPath);

        /// <summary>
        /// Indexes defined content
        /// </summary>
        protected bool IndexContent(string contentPath, string path, MetadataType type, out Metadata metadata)
        {
            string filePath = Path.Combine(path, "index.md");
            if (File.Exists(filePath))
            {
                if (TryIndexFile(filePath, contentPath, out metadata))
                {
                    metadata.Type = type;
                    return true;
                }
            }

            metadata = null;
            return false;
        }

        /// <summary>
        /// Indexes a markdown file
        /// </summary>
        public bool TryIndexFile(string filePath, string contentPath, out Metadata metadata)
        {
            string slugFolderPath = Path.GetDirectoryName(filePath);
            string slug = MakeRelativePath(contentPath, slugFolderPath);

            using (var rdr = File.OpenText(filePath))
            {
                // read head
                if (!TryReadSiteYaml(rdr, out string yaml))
                {
                    // Todo: exception
                    metadata = null;
                    return false;
                }

                // read content
                string rawContent = rdr.ReadToEnd();

                // read yaml
                Deserializer yamlDeserializer = new Deserializer();
                Dictionary<string, string> siteConfig = yamlDeserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));
                Content content = new Content(_markdownLoader.GetContentToRender(rawContent, MakeRelativePath(Path.GetDirectoryName(contentPath), slugFolderPath)), ContentType.HTML);

                // optional data

                if (siteConfig.ContainsKey(Strings.MetadataNames.Slug))
                {
                    // use slug if set
                    slug = siteConfig[Strings.MetadataNames.Slug];
                }
                siteConfig.TryGetValue("description", out string description);
                siteConfig.TryGetValue("image", out string image);
                siteConfig.TryGetValue("keywords", out string keywords);

                // convert the dictionary into a model
                metadata = new Metadata
                {
                    Slug = slug,

                    Title = siteConfig[Strings.MetadataNames.Title],
                    Author = siteConfig[Strings.MetadataNames.Author],
                    PublicationDate = DateTime.Parse(siteConfig[Strings.MetadataNames.PublicationDate]),
                    LastModified = DateTime.Parse(siteConfig[Strings.MetadataNames.LastModified]),
                    Categories = siteConfig[Strings.MetadataNames.Categories].Split(',').Select(c => c.Trim()).ToArray(),
                    Content = content,

                    // Seo
                    Description = description,
                    Image = image,
                    Keywords = keywords
                };

                return true;
            }
        }

        public static string MakeRelativePath(string fromPath, string toPath) => Path.GetFullPath(toPath).Substring(Path.GetFullPath(fromPath).Length + 1);

        /// <summary>
        /// Reads all head information of given yaml content
        /// </summary>
        public bool TryReadSiteYaml(StreamReader streamReader, out string yaml)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string line;
            if ((line = streamReader.ReadLine()) == "---")
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line == "---")
                    {
                        yaml = stringBuilder.ToString();
                        return true;
                    }

                    stringBuilder.AppendLine(line);
                }
            }

            yaml = null;
            return false;
        }
    }
}