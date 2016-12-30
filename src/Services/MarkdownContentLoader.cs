using System;

namespace downr.Services
{
    public interface IMarkdownContentLoader
    {

    }

    public class DefaultMarkdownContentLoader : IMarkdownContentLoader
    {
        static DefaultMarkdownContentLoader()
        {
            Console.WriteLine("Loading Content");
        }
    }
}