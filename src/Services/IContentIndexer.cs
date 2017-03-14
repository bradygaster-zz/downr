using System.Collections.Generic;
using downr.Models;

namespace downr.Services
{
    public interface IContentIndexer
    {
        /// <summary>
        /// In memory cache of all indexed content
        /// </summary>
        IDictionary<string, Metadata> Metadata { get; }

        /// <summary>
        /// Returns <see cref="Metadata"/> by given slug. Returns false if slug is not present
        /// </summary>
        bool TryGet(string slug, out Metadata metadata);

        /// <summary>
        /// Implement indexer logic here
        /// </summary>
        IContentIndexer Index(string contentPath, string slugPrefix = "");

        /// <summary>
        /// returns the next entry of given metadata
        /// </summary>
        /// <returns>true if found, false if not</returns>
        bool TryGetNext(Metadata metadata, out Metadata nextMetadata);

        /// <summary>
        /// returns the previous entry of given metadata
        /// </summary>
        /// <returns>true if found, false if not</returns>
        bool TryGetPrevious(Metadata metadata, out Metadata previousMetadata);
    }
}