using System;

namespace downr.Models
{
    public class Metadata
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime LastModified { get; set; }
        public string Author { get; set; }
        public string[] Categories { get; set; }
    }
}