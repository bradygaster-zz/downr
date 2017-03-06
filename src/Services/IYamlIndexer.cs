//using System.Collections.Generic;
//using downr.Models;

//namespace downr
//{
//    public interface IYamlIndexer
//    {
//        IDictionary<string, Metadata> PostsMetadata { get; }
//        IDictionary<string, Metadata> PagesMetadata { get; }
//        IDictionary<string, int> TagCloud { get; }

//        int PostsCount { get; }
//        int PagesCount { get; }

//        bool TryGetPost(string slug, out Metadata metadata);
//        bool TryGetPage(string slug, out Metadata metadata);

//        IYamlIndexer IndexPageFiles(string contentPath);
//        IYamlIndexer IndexPostFiles(string contentPath);

//        IYamlIndexer Build();
//    }
//}