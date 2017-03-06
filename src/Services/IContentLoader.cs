namespace downr.Services
{
    public interface IContentLoader
    {
        string GetContentToRender(string content, string contentFolder);
    }
}