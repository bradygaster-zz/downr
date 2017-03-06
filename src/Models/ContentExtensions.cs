using System;
using System.Text.RegularExpressions;

namespace downr.Models
{
    public static class ContentExtensions
    {
        // http://stackoverflow.com/questions/19523913/remove-html-tags-from-string-including-nbsp-in-c-sharp

        private static readonly Regex StripHtmlRegex = new Regex(@"<[^>]+>|&nbsp;", RegexOptions.Compiled);
        public static Content StripHtml(this Content content)
        {
            return new Content(StripHtmlRegex.Replace(content.Raw, ""), ContentType.Plaintext);
        }

        public static Content Excerpt(this Content content, int length)
        {
            length = Math.Min(content.Raw.Length, length);

            return new Content(content.Raw.Substring(0, length - 1), content.Type);
        }
    }
}