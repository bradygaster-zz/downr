using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace downr.Extensions
{
    public static class UriExtensions
    {
        public static string Combine(string u1, string u2)
        {
            return $"{u1.TrimEnd('/')}/{u2.TrimStart('/')}";
        }

        public static string Urify(string uri)
        {
            return uri.Replace("\\", "/");
        }
    }
}
