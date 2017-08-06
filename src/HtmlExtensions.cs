using System.Net;
using downr.Models;

namespace downr
{
    public static class HtmlExtensions
    {
        public static string GetHtmlSnippet(this Metadata post, int snippetLength = 250, string continuationText = "...")
        {
            var text = post.Content.RemoveHtmlTags();
            // check whether we need to truncate
            if (snippetLength < text.Length)
            {
                // find first space before snippetLength
                int index = text.LastIndexOfAny(new[] { ' ', '\n', 'r', '\t'}, snippetLength);
                if (index < 0)
                {
                    index = snippetLength; // if no space then just truncate rudely!
                }
                text = text.Substring(0, index).TrimEnd() + continuationText;
            }
            return text;
        }
        public static string RemoveHtmlTags(this string markup)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(markup);
            var html = doc.DocumentNode.InnerText;
            return WebUtility.HtmlDecode(html);
        }
    }
}