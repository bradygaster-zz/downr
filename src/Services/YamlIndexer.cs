using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using System.Collections.Generic;

namespace downr.Services
{
    public interface IYamlIndexer
    {
        List<Dictionary<string, string>> Metadata { get; set; }
        void IndexContentFiles(string contentPath);
    }

    public class DefaultYamlIndexer : IYamlIndexer
    {
        public DefaultYamlIndexer()
        {
            Metadata = new List<Dictionary<string, string>>();
        }

        public List<Dictionary<string, string>> Metadata { get; set; }

        public void IndexContentFiles(string contentPath)
        {
            var subDirectories = Directory.GetDirectories(contentPath);

            foreach (var subDirectory in subDirectories)
            {
                using (var rdr = File.OpenText(
                    string.Format("{0}\\index.md", subDirectory)
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
                        
                        Metadata.Add(result);
                    }
                }
            }
        }
    }
}