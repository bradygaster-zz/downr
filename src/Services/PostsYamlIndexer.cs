namespace downr.Services
{
    public class PostsYamlIndexer : DefaultYamlIndexer, IPostsIndexer
    {
        public PostsYamlIndexer(IMarkdownContentLoader markdownLoader)
               : base(markdownLoader)
        {

        }
    }
}
