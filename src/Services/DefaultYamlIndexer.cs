using System;
using System.IO;
using System.Collections.Generic;
using downr.Models;
using YamlDotNet.Serialization;
using System.Text;

namespace downr.Services
{

    public class DefaultYamlIndexer : IYamlIndexer
    {
        IMarkdownContentLoader _markdownLoader;

        public IDictionary<string, Metadata> Metadata { get; set; }
        public int Count => Metadata.Count;

        public DefaultYamlIndexer(IMarkdownContentLoader markdownLoader)
        {
            _markdownLoader = markdownLoader;
            Metadata = new Dictionary<string, Metadata>(StringComparer.OrdinalIgnoreCase);
        }

        public bool TryGet(string slug, out Metadata metadata)
        {
            return Metadata.TryGetValue(slug, out metadata);
        }

        public bool TryReadSiteYaml(StreamReader streamReader, out string yaml)
        {
            var stringBuilder = new StringBuilder();

            string line;
            bool started = false;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line == "---")
                {
                    if (started)
                    {
                        yaml = stringBuilder.ToString();
                        return true;
                    }

                    started = true;
                }
                else if (!started)
                {
                    break;
                }
                else
                {
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

                var yamlDeserializer = new Deserializer();
                var siteConfig = yamlDeserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));

                var content = new Content(_markdownLoader.GetContentToRender(rawContent, relativeContentPath), ContentType.HTML);

                // convert the dictionary into a model
                metadata = new Metadata
                {
                    Slug = siteConfig[Strings.MetadataNames.Slug],
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

        public void IndexContentFiles(string contentPath)
        {
            var subDirectories = Directory.GetDirectories(contentPath);

            foreach (var subDirectory in subDirectories)
            {
                string filePath = Path.Combine(subDirectory, "index.md");
                if (File.Exists(filePath))
                {
                    Metadata metadata;
                    if (TryIndexFile(filePath, contentPath, out metadata))
                    {
                        Metadata.Add(metadata.Slug, metadata);
                    }
                }
            }
        }
    }
}
