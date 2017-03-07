using System.Collections.Generic;

namespace downr.Services
{
    /// <summary>
    /// Builds cloud tags by content tags
    /// </summary>
    public interface ITagCloudBuilder
    {
        /// <summary>
        /// Builds the tag cloud 
        /// </summary>
        IDictionary<string, int> GetTagCloud();
    }
}