using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO3Scraper.Models
{
    internal class AO3ResultPage
    {
        internal int PageIndex { get; set; }
        internal int PageCount { get; set; }
        internal bool IsMaxPageCount { get; set; }
        internal List<AO3FicMiniItem> Items { get; set; } = new List<AO3FicMiniItem>();

    }
    internal class AO3FicMiniItem
    {
        internal int Id { get; set; }
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
        internal DateTime UpdatedDate { get; set; }
    }

}
