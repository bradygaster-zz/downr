using System.Text.RegularExpressions;

namespace downr.Models
{
    public static class ContentExtensions
    {
        // http://stackoverflow.com/questions/19523913/remove-html-tags-from-string-including-nbsp-in-c-sharp
        
        private static Regex StripHtmlRegex = new Regex(@"<[^>]+>|&nbsp;", RegexOptions.Compiled);
        public static Content StripHtml(this Content content)
        {
            return new Content(StripHtmlRegex.Replace(content.Raw, ""), ContentType.Plaintext);
        }

        public static Content Excerpt(this Content content, int length){
            return new Content(content.Raw.Substring(0, length < 0 ? content.Raw.Length : length), content.Type);
        }
    }
}