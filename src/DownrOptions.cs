namespace downr
{
    public class DownrOptions
    {
        public string Title { get; set; }
        public string Url { get; set; }

        // Metadata
        public string Description { get; set; }
        public string Author { get; set; }
        public string Keywords { get; set; }

        // Feed
        public int PostCountFeed { get; set; }
        public string FeedSlug { get; set; }
    }
}