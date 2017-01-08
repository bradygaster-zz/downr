namespace downr.Models
{
    public class Content
    {
        public Content(string raw, ContentType type)
        {
            Raw = raw;
            Type = type;
        }



        public ContentType Type { get; set; }
        public string Raw { get; set; }



        public static implicit operator Content(string d)
        {
            return new Content(d, ContentType.Unknown);
        }

        public static implicit operator string(Content content)
        {
            return content.Raw;
        }

        public override string ToString()
        {
            return Raw;
        }
    }


}