using System;
using System.Collections.Generic;
using System.Linq;
using downr.Models;

namespace downr.Services
{
    public class PostService
    {
        private readonly IYamlIndexer _indexer;

        public PostService(IYamlIndexer indexer)
        {
            _indexer = indexer;
        }

        public Metadata[] GetPosts(int offset = 0, int count = -1, string category = null)
        {
            var posts = (IEnumerable<Metadata>)_indexer.Metadata;

            if (category != null)
            {
                posts = posts.Where(p => p.Categories.Contains(category));
            }

            posts = posts.Skip(offset);

            if (count > 0)
            {
                posts = posts.Take(count);
            }
            
            return posts.ToArray();
        }

        public int GetNumberOfPosts(string category = null)
        {
            if (category != null)
            {
                return _indexer.Metadata.Count(p => p.Categories.Contains(category));
            }
            return _indexer.Metadata.Count;
        }

        public Metadata GetLatestPost()
        {
            return _indexer.Metadata[0];
        }

        public Metadata GetPostBySlug(string slug)
        {
            return _indexer.Metadata.FirstOrDefault(p => p.Slug == slug);
        }
        public (Metadata previous, Metadata next) GetPreviousAndNextPosts(string slug)
        {
            (Metadata previous, Metadata next) result = (null, null);

            int index = _indexer.Metadata.FindIndex(x => x.Slug == slug);
            if (index != 0)
            {
                result.next = _indexer.Metadata[index - 1];
            }
            if (index != GetNumberOfPosts() - 1)
            {
                result.previous = _indexer.Metadata[index + 1];
            }
            return result;
        }
    }
}