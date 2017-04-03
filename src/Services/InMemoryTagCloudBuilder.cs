using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace downr.Services
{
    /// <summary>
    /// Builds cloud tags by content tags
    /// </summary>
    public class InMemoryTagCloudBuilder : ITagCloudBuilder
    {
        private readonly IPostIndexer _postIndexer;
        private readonly IPageIndexer _pageIndexer;
        private readonly DownrOptions _options;

        public InMemoryTagCloudBuilder(IPostIndexer postIndexer, IPageIndexer pageIndexer, IOptions<DownrOptions> options)
        {
            _postIndexer = postIndexer;
            _pageIndexer = pageIndexer;
            _options = options.Value;
        }

        /// <summary>
        /// Builds the tag cloud from <see cref="IPostIndexer"/> and <see cref="IPageIndexer"/>
        /// </summary>
        public IDictionary<string, int> GetTagCloud()
        {
            IDictionary<string, int> tags = new Dictionary<string, int>();

            IEnumerable<KeyValuePair<string, Models.Metadata>> allSources = _postIndexer.Metadata.Union(_pageIndexer.Metadata);

            foreach (var entry in allSources)
            {
                if (entry.Value.Categories != null)
                {
                    foreach (var category in entry.Value.Categories)
                    {
                        tags.TryGetValue(category, out int count);
                        tags[category] = count + 1;
                    }
                }
            }

            return tags;
        }
    }
}