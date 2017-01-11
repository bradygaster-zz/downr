using System;
using System.IO;
using System.Collections.Generic;
using downr.Models;
using YamlDotNet.Serialization;
using System.Text;
using System.Linq;

namespace downr.Services
{

    public class DefaultYamlIndexer : IYamlIndexer
    {
        IMarkdownContentLoader _markdownLoader;

        public IDictionary<string, Metadata> PostsMetadata { get; set; }
        public IDictionary<string, Metadata> PagesMetadata { get; set; }
        public IDictionary<string, int> TagCloud { get; set; }

        public int PostsCount => PostsMetadata.Count;
        public int PagesCount => PagesMetadata.Count;

        public DefaultYamlIndexer(IMarkdownContentLoader markdownLoader)
        {
            _markdownLoader = markdownLoader;
            PostsMetadata = new Dictionary<string, Metadata>(StringComparer.OrdinalIgnoreCase);
            PagesMetadata = new Dictionary<string, Metadata>(StringComparer.OrdinalIgnoreCase);
            TagCloud = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        public bool TryGetPost(string slug, out Metadata metadata)
        {
            return PostsMetadata.TryGetValue(slug, out metadata);
        }
        public bool TryGetPage(string slug, out Metadata metadata)
        {
            return PagesMetadata.TryGetValue(slug, out metadata);
        }

        public bool TryReadSiteYaml(StreamReader streamReader, out string yaml)
        {
            var stringBuilder = new StringBuilder();

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


        public bool TryIndexFile(string filePath, string contentPath, out Metadata metadata)
        {
            Uri fileDirectoryUri = new Uri(Path.GetDirectoryName(filePath));
            Uri contentPathUri = new Uri(Path.GetDirectoryName(contentPath));
            Uri slugPathUri = new Uri(contentPath);

            Uri relativeContentUri = contentPathUri.MakeRelativeUri(fileDirectoryUri);
            string relativeContentPath = relativeContentUri.OriginalString;

            using (var rdr = File.OpenText(filePath))
            {
                // read head
                string yaml;
                if (!TryReadSiteYaml(rdr, out yaml))
                {
                    // Todo: exception
                    metadata = null;
                    return false;
                }

                // read content
                string rawContent = rdr.ReadToEnd();

                // read yaml
                var yamlDeserializer = new Deserializer();
                var siteConfig = yamlDeserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));

                var content = new Content(_markdownLoader.GetContentToRender(rawContent, relativeContentPath), ContentType.HTML);

                // optional data
                string slug;
                if (!siteConfig.ContainsKey(Strings.MetadataNames.Slug))
                {
                    // use folder name as slug if not present
                    Uri slugUri = slugPathUri.MakeRelativeUri(fileDirectoryUri);
                    slug = slugUri.OriginalString;
                }
                else
                {
                    // use slug if set
                    slug = siteConfig[Strings.MetadataNames.Slug];
                }

                // convert the dictionary into a model
                metadata = new Metadata
                {
                    Slug = slug,
                    Title = siteConfig[Strings.MetadataNames.Title],
                    Author = siteConfig[Strings.MetadataNames.Author],
                    PublicationDate = DateTime.Parse(siteConfig[Strings.MetadataNames.PublicationDate]),
                    LastModified = DateTime.Parse(siteConfig[Strings.MetadataNames.LastModified]),
                    Categories = siteConfig[Strings.MetadataNames.Categories].Split(','),
                    Content = content
                };

                return true;
            }
        }

        public IYamlIndexer IndexPageFiles(string contentPath)
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
                Metadata metadata;
                if (IndexContent(contentPath, currentDir, MetadataType.Page, out metadata))
                {
                    PagesMetadata.Add(metadata.Slug, metadata);
                }

                // support page sub folders
                foreach (var subDir in Directory.GetDirectories(currentDir))
                {
                    Metadata subMetadata;
                    if (IndexContent(contentPath, subDir, MetadataType.Page, out subMetadata))
                    {
                        PagesMetadata.Add(subMetadata.Slug, subMetadata);
                    }
                }


            }

            return this;
        }

        public IYamlIndexer IndexPostFiles(string contentPath)
        {
            foreach (var subDir in Directory.GetDirectories(contentPath))
            {
                Metadata metadata;
                if (IndexContent(contentPath, subDir, MetadataType.Post, out metadata))
                {
                    PostsMetadata.Add(metadata.Slug, metadata);
                }
            }

            return this;
        }


        public bool IndexContent(string contentPath, string path, MetadataType type, out Metadata metadata)
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

        public IYamlIndexer Build()
        {
            SortMetadata();
            BuildTagCloud(PostsMetadata);
            BuildTagCloud(PagesMetadata);
            SortTagCloud();

            return this;
        }

        public void SortMetadata()
        {
            PostsMetadata = PostsMetadata.OrderByDescending(x => x.Value.PublicationDate).ToDictionary(x => x.Key, x => x.Value);
        }

        public void BuildTagCloud(IDictionary<string, Metadata> metadata)
        {        // get all the categories
            foreach (var entry in metadata)
            {
                foreach (var category in entry.Value.Categories)
                {
                    int count;
                    TagCloud.TryGetValue(category, out count);
                    TagCloud[category] = count + 1;
                }
            }
        }

        public void SortTagCloud()
        {
            TagCloud = TagCloud.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}