using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO3Scraper.Models
{
    internal class AO3FicPage
    {
        internal string DownloadPath { get; set; } = "";
        internal string DLTimestamp { get; set; } = "";
        internal int Id { get; set; }
        internal string Rating { get; set; } = "";
        internal string Title { get; set; } = "";
        internal string Author { get; set; } = "";
        internal string AuthorURI { get; set; } = "";
        internal List<string> Fandoms { get; set; } = new List<string>();
        internal List<string> Warnings { get; set; } = new List<string>();
        internal List<string> Relationships { get; set; } = new List<string>();
        internal List<string> Characters { get; set; } = new List<string>();
        internal List<string> FreeformTags { get; set; } = new List<string>();
        internal int Words { get; set; }
        internal int CurrentChapters { get; set; }
        internal int MaxChapters { get; set; }
        internal int Hits { get; set; }
        internal int Bookmarks { get; set; }
        internal int Kudos { get; set; }
        internal int Comments { get; set; }
        internal DateTime PublishedDate { get; set; }
        internal DateTime UpdatedDate { get; set; }
        internal string Status { get; set; } = "";
        internal List<string> Categories { get; set; } = new List<string>();

    }
}
