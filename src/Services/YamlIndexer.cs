using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using downr.Models;
using YamlDotNet.Serialization;

namespace downr.Services
{
    public interface IYamlIndexer
    {
        List<Metadata> Metadata { get; set; }
        void IndexContentFiles(string contentPath);
    }

    public class DefaultYamlIndexer : IYamlIndexer
    {
        IMarkdownContentLoader _markdownLoader;

        public DefaultYamlIndexer(IMarkdownContentLoader markdownLoader)
        {
            Metadata = new List<Metadata>();
            _markdownLoader = markdownLoader;
        }

        public List<Metadata> Metadata { get; set; }

        public void IndexContentFiles(string contentPath)
        {
            var subDirectories = Directory.GetDirectories(contentPath);

            foreach (var subDirectory in subDirectories)
            {
                using (var rdr = File.OpenText(
                        Path.Combine(subDirectory, "index.md")
                    ))
                {
                    // make sure the file has the header at the first line
                    var line = rdr.ReadLine();
                    if (line == "---")
                    {
                        line = rdr.ReadLine();

                        var stringBuilder = new StringBuilder();

                        // keep going until we reach the end of the header
                        while (line != "---")
                        {
                            stringBuilder.Append(line);
                            stringBuilder.Append("\n");
                            line = rdr.ReadLine();
                        }

                        var yaml = stringBuilder.ToString();
                        var de = new Deserializer();
                        var result = de.Deserialize<Dictionary<string, string>>(new StringReader(yaml));

                        // convert the dictionary into a model
                        var metadata = new Metadata
                        {
                            Slug = result[Strings.MetadataNames.Slug],
                            Title = result[Strings.MetadataNames.Title],
                            Author = result[Strings.MetadataNames.Author],
                            PublicationDate = DateTime.Parse(result[Strings.MetadataNames.PublicationDate]),
                            LastModified = DateTime.Parse(result[Strings.MetadataNames.LastModified]),
                            Categories = result[Strings.MetadataNames.Categories].Split(','),
                            Content = _markdownLoader.GetContentToRender(result[Strings.MetadataNames.Slug])
                        };

                        Metadata.Add(metadata);
                    }
                }
            }

            Metadata = Metadata.OrderByDescending(x => x.PublicationDate).ToList();
        }
    }
}