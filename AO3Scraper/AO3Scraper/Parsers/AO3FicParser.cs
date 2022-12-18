using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AO3Scraper.Models;
using HtmlAgilityPack;

namespace AO3Scraper.Parsers
{
    internal class AO3FicParser
    {
        public static AO3FicPage ParseFicPage(string RawHTML)
        {


            HtmlDocument hd = new HtmlDocument();
            hd.LoadHtml(RawHTML);
            var FicPage = new AO3FicPage();

            //Get ID
            //var IDString = hd.DocumentNode.SelectSingleNode("//ul[@class='work navigation actions']/li[@class='share']");
            //FicPage.Id = Convert.ToInt32(IDString.Remove(IDString.IndexOf('?')));

            //Get Download Link
            var DownloadLinks = hd.DocumentNode.SelectNodes("//ul[@class='work navigation actions']/li[@class='download']/ul[@class='expandable secondary']/li/a");
            FicPage.DownloadPath = DownloadLinks[1].GetAttributeValue("href", string.Empty); //epub
            FicPage.Id = Convert.ToInt32(DownloadLinks[1].GetAttributeValue("href", string.Empty).Split('/')[2]);
            FicPage.DLTimestamp = FicPage.DownloadPath.Substring(FicPage.DownloadPath.LastIndexOf('=') + 1);

            //Get Rating
            FicPage.Rating = hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='rating tags']/ul/li/a[@href]").InnerText;

            //Parse tags and warnings
            var Warnings = (hd.DocumentNode.SelectNodes("//dl[@class='work meta group']/dd[@class='warning tags']/ul/li"));
            if (Warnings != null)
            {
                foreach (var Warning in Warnings)
                {
                    FicPage.Warnings.Add(Warning.InnerText);
                }
            }

            var Categories = (hd.DocumentNode.SelectNodes("//dl[@class='work meta group']/dd[@class='category tags']/ul/li"));
            if (Categories != null)
            {
                foreach (var Category in Categories)
                {
                    FicPage.Categories.Add(Category.InnerText);
                }
            }

            var Fandoms = (hd.DocumentNode.SelectNodes("//dl[@class='work meta group']/dd[@class='fandom tags']/ul/li"));
            if (Fandoms != null)
            {
                foreach (var Fandom in Fandoms)
                {
                    FicPage.Fandoms.Add(Fandom.InnerText);
                }
            }

            var Relationships = (hd.DocumentNode.SelectNodes("//dl[@class='work meta group']/dd[@class='relationship tags']/ul/li"));
            if (Relationships != null)
            {
                foreach (var Relationship in Relationships)
                {
                    FicPage.Relationships.Add(Relationship.InnerText);
                }
            }

            var Characters = (hd.DocumentNode.SelectNodes("//dl[@class='work meta group']/dd[@class='character tags']/ul/li"));
            if (Characters != null)
            {
                foreach (var Character in Characters)
                {
                    FicPage.Characters.Add(Character.InnerText);
                }
            }

            var Freeforms = (hd.DocumentNode.SelectNodes("//dl[@class='work meta group']/dd[@class='freeform tags']/ul/li"));
            if (Freeforms != null)
            {
                foreach (var Freeform in Freeforms)
                {
                    FicPage.FreeformTags.Add(Freeform.InnerText);
                }
            }

            //Parse fic stats
            var UpdatedDate = hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='status']");
            if(UpdatedDate != null) FicPage.UpdatedDate = DateTime.Parse(UpdatedDate.InnerText);
            FicPage.PublishedDate = DateTime.Parse(hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='published']").InnerText);
            FicPage.Words = Convert.ToInt32(hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='words']").InnerText);
            var Chapters = hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='chapters']").InnerText.Split('/');
            FicPage.CurrentChapters = Convert.ToInt32(Chapters[0]);
            if (Chapters[1] == "?") Chapters[1] = "999999";
            FicPage.MaxChapters = Convert.ToInt32(Chapters[1]);
            FicPage.Hits = Convert.ToInt32(hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='hits']").InnerText);
            var Status = hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dt[@class='status']");
            if (Status != null) FicPage.Status = Status.InnerText.Replace(":","").Trim();

            var Kudos = hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='kudos']");
            if (Kudos != null) FicPage.Kudos = Convert.ToInt32(Kudos.InnerText); else FicPage.Kudos = 0;
            var Bookmarks = hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='bookmarks']");
            if (Bookmarks != null) FicPage.Bookmarks = Convert.ToInt32(Bookmarks.InnerText); else FicPage.Bookmarks = 0;
            var Comments = hd.DocumentNode.SelectSingleNode("//dl[@class='work meta group']/dd[@class='stats']/dl/dd[@class='comments']");
            if (Comments != null) FicPage.Comments = Convert.ToInt32(Comments.InnerText); else FicPage.Comments = 0;

            //Parse title and author
            FicPage.Title = hd.DocumentNode.SelectSingleNode("//div[@class='preface group']/h2[@class='title heading']").InnerText.Trim();
            FicPage.Author = hd.DocumentNode.SelectSingleNode("//div[@class='preface group']/h3[@class='byline heading']/a").InnerText.Trim();
            FicPage.AuthorURI = hd.DocumentNode.SelectSingleNode("//div[@class='preface group']/h3[@class='byline heading']/a").GetAttributeValue("href", string.Empty);



            return FicPage;
        }
    }
}
