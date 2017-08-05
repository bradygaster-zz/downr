namespace downr
{
    public static class StringExtensions
    {
        public static string StripLeading(this string value, string valueToStrip)
        {
            if (value.StartsWith(valueToStrip))
                return value.Substring(valueToStrip.Length);
            return value;
        }
        public static string EnsureTrailing(this string value, string valueToEndWith)
        {
            if (value.EndsWith(valueToEndWith))
                return value;
            return value + valueToEndWith;
        }
    }
}