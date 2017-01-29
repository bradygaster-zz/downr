using System.Collections.Generic;
using downr.Models;

namespace downr.Services
{
    public interface IFeedBuilder
    {
        string BuildXmlFeed(IEnumerable<Metadata> posts);
    }
}