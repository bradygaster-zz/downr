using System.Collections.Generic;
using downr.Models;

namespace downr.Services
{
    public interface IYamlIndexer
    {
        IDictionary<string, Metadata> Metadata { get; }
        int Count { get; }

        bool TryGet(string slug, out Metadata Metadata);
        void IndexContentFiles(string contentPath);
    }

    public interface IPagesIndexer : IYamlIndexer
    {

    }

    public interface IPostsIndexer : IYamlIndexer
    {

    }
}
