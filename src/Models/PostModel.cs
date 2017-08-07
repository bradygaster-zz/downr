namespace downr.Models
{
    public class PostModel
    {
        public Metadata Post { get; set; }
        public string NextPostSlug { get; set; }
        public string NextPostTitle { get; set; }
        public string PreviousPostSlug { get; set; }
        public string PreviousPostTitle { get; set; }
    }
}