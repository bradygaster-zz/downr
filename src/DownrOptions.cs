namespace downr
{
    public class DownrOptions
    {
        // The title for the blog (used in feed)
        public string Title { get; set; }

        // The external root of the blog (e.g. http://example.com/blog)
        public string RootUrl { get; set; }

        // The url stem for blog content (e.g. /blog )
        public string Stem { get; set; }

        // Show a list of summary posts on blog homepage
        public HomePageStyle HomePageStyle { get; set; }
    }

    public enum HomePageStyle
    {
        LatestPost,
        SummaryList
    }
}