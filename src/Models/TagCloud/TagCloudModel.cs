namespace downr.Models.TagCloud
{
    public class TagCloudModel
    {
        public Tag[] Tags { get; set; }
    }
    public class Tag
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}