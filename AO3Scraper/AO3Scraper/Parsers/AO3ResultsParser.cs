using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AO3Scraper.Models;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AO3Scraper.Parsers
{
    internal class AO3ResultsParser
    {
        public static AO3ResultPage ParseResultsPage(string RawHTML)
        {
            var ResultsPage = new AO3ResultPage();
            HtmlDocument hd = new HtmlDocument();
            hd.LoadHtml(RawHTML);
            ResultsPage.PageIndex = Convert.ToInt32(hd.DocumentNode.SelectSingleNode("//ol[@role='navigation']/li/span[@class='current']").InnerText);
            var Navigation = hd.DocumentNode.SelectNodes("//ol[@role='navigation']/li");
            if (Navigation != null)
                ResultsPage.PageCount = Convert.ToInt32(Navigation[Navigation.Count - 2].InnerText);
            else
                ResultsPage.PageCount = 1;
            if(ResultsPage.PageIndex >= ResultsPage.PageCount) ResultsPage.IsMaxPageCount = true;

            var FicNodes = hd.DocumentNode.SelectNodes("//ol[@class='work index group']/li");
            if (FicNodes != null)
            {
                //ResultsPage.Items = new List<AO3FicMiniItem>();
                foreach (var Node in FicNodes)
                {
                    //Prepare Object
                    AO3FicMiniItem Ao3FicMiniItem = new AO3FicMiniItem();
                    //Ao3FicMiniItem.Warnings = new List<string>();

                    //Do some HTML stuff
                    HtmlDocument item = new HtmlDocument();
                    item.LoadHtml(Node.InnerHtml);

                    //Parse fic heading
                    Ao3FicMiniItem.Title = item.DocumentNode.SelectSingleNode("//div[@class='header module']/h4[@class='heading']/a[@href]").InnerText;
                    string[] TitleRelLink = item.DocumentNode.SelectSingleNode("//div[@class='header module']/h4[@class='heading']/a[@href]").GetAttributeValue("href", string.Empty).Split('/');
                    Ao3FicMiniItem.Id = Convert.ToInt32(TitleRelLink[TitleRelLink.Length - 1]);
                    var Author = item.DocumentNode.SelectSingleNode("//div[@class='header module']/h4[@class='heading']/a[@rel='author']");
                    if (Author != null) Ao3FicMiniItem.Author = Author.InnerText; else Ao3FicMiniItem.Author = "Anonymous";

                    //Parse fic date
                    Ao3FicMiniItem.UpdatedDate = DateTime.Parse(item.DocumentNode.SelectSingleNode("//div[@class='header module']/p[@class='datetime']").InnerText);

                    //Parse fic fandoms
                    var Fandoms = item.DocumentNode.SelectNodes("//div[@class='header module']/h5[@class='fandoms heading']/a[@class='tag']");
                    if (Fandoms != null)
                    {
                        foreach (var Fandom in Fandoms)
                        {
                            Ao3FicMiniItem.Fandoms.Add(Fandom.InnerText);
                        }
                    }

                    //Parse tags and warnings
                    var Warnings = (item.DocumentNode.SelectNodes("//ul[@class='tags commas']/li[@class='warnings']/strong/a[@href]"));
                    if (Warnings != null)
                    {
                        foreach (var Warning in Warnings)
                        {
                            Ao3FicMiniItem.Warnings.Add(Warning.InnerText);
                        }
                    }
                    var Relationships = (item.DocumentNode.SelectNodes("//ul[@class='tags commas']/li[@class='relationships']/a[@href]"));
                    if (Relationships != null)
                    {
                        foreach (var Relationship in Relationships)
                        {
                            Ao3FicMiniItem.Relationships.Add(Relationship.InnerText);
                        }
                    }
                    var Characters = (item.DocumentNode.SelectNodes("//ul[@class='tags commas']/li[@class='characters']/a[@href]"));
                    if (Characters != null)
                    {
                        foreach (var Character in Characters)
                        {
                            Ao3FicMiniItem.Characters.Add(Character.InnerText);
                        }
                    }
                    var Freeforms = (item.DocumentNode.SelectNodes("//ul[@class='tags commas']/li[@class='freeforms']/a[@href]"));
                    if (Freeforms != null)
                    {
                        foreach (var Freeform in Freeforms)
                        {
                            Ao3FicMiniItem.FreeformTags.Add(Freeform.InnerText);
                        }
                    }

                    //Parse work info
                    Ao3FicMiniItem.Words = Convert.ToInt32(item.DocumentNode.SelectSingleNode("//dl[@class='stats']/dd[@class='words']").InnerText.Replace(",",""));
                    var Chapters = item.DocumentNode.SelectSingleNode("//dl[@class='stats']/dd[@class='chapters']").InnerText.Split("/");
                    Ao3FicMiniItem.CurrentChapters = Convert.ToInt32(Chapters[0]);
                    if (Chapters[1] == "?") Chapters[1] = "999999";
                    Ao3FicMiniItem.MaxChapters = Convert.ToInt32(Chapters[1]);
                    Ao3FicMiniItem.Hits = Convert.ToInt32(item.DocumentNode.SelectSingleNode("//dl[@class='stats']/dd[@class='hits']").InnerText);


                    ResultsPage.Items.Add(Ao3FicMiniItem);
                }
            }
            return ResultsPage;
        }
    }
}
