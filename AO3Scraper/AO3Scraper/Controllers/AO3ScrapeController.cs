using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AO3Scraper.Web;
using AO3Scraper.Parsers;
using AO3Scraper.Models;
using System.Net;
using NLog;

namespace AO3Scraper.Controllers
{
    internal class AO3ScrapeController
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        string AO3BaseURI = "https://archiveofourown.org";
        string TempDir = "C:\\temp";
        HttpClient httpClient = new HttpClient();
        public void ScrapeURI(string URI, string PageSeperator)
        {
            //testing
            //var hHTMLPage = HTTPReader.GetPage(URI);
            //var FicPage = AO3FicParser.ParseFicPage(hHTMLPage);
            //return;

            //Check works
            int Threshold = 5; //the number of fics that must match exactly before the scraper is stopped. Increase this for very active searches
            int CurSuccess = 0;
            
            List<AO3FicMiniItem> FicsToScrape = new List<AO3FicMiniItem>();


            //Begin scrape, grab first page
            logger.Debug("Beginning scrape of " + URI);
            AO3ResultPage ResultsPage = new AO3ResultPage();
            string BaseURI = URI;
            string CurrentURI = BaseURI;
            while (!ResultsPage.IsMaxPageCount)
            {
                logger.Trace("Getting HTML data");
                var HTMLPage = HTTPReader.GetPage(CurrentURI);
                logger.Trace("Parsing results page");
                var CurrentPage = AO3ResultsParser.ParseResultsPage(HTMLPage);

                foreach (var fic in CurrentPage.Items)
                {
                    if (fic == null || fic.Id == 0)
                    {
                        logger.Error("Fic object is null, or ID is missing");
                        return;
                    }
                    logger.Trace("Validating fic ID " + fic.Id);
                    if (!ValidateFic(fic))
                    {
                        logger.Debug("Fic failed to validate, adding it to the list");
                        FicsToScrape.Add(fic);
                        logger.Trace("Resetting success counter to 0");
                        CurSuccess = 0;
                    }
                    else
                    {
                        logger.Debug("Fic validated successfully, might have reached the end of the list to scrape? Current success threshold " + CurSuccess + "/" + Threshold);
                        CurSuccess++;
                    }
                }
                logger.Trace("Finished processling the current list of fics");
                //FicsToScrape.AddRange(CurrentPage.Items);
                ResultsPage.PageCount = CurrentPage.PageCount;
                ResultsPage.PageIndex = CurrentPage.PageIndex;
                ResultsPage.IsMaxPageCount = CurrentPage.IsMaxPageCount;
                logger.Debug("Current page is " + ResultsPage.PageIndex + "/" + ResultsPage.PageCount);
                if (ResultsPage.IsMaxPageCount)
                {
                    logger.Debug("Looks like we've reached the end of the search, returning");
                    return;
                }
                if(CurSuccess > Threshold)
                {
                    logger.Info("Looks like we are all caught up, returning.");
                    return;
                }
                CurrentURI = BaseURI + PageSeperator + "page=" + (ResultsPage.PageIndex + 1).ToString();
                logger.Debug("Prepping next page, new URI is " + CurrentURI);
                logger.Debug("Current queue size is " + FicsToScrape.Count);

                System.Threading.Thread.Sleep(5000);
                break;
            }

            logger.Debug("Finished building the queue, beginning download");
            foreach(var fic in FicsToScrape)
            {
                ScrapeFic(fic);
            }



        }
        public bool ValidateFic(AO3FicMiniItem FicMiniItem)
        {
            //Validate the fic to see if its in the DB, or if the chapter count has changed

            logger.Trace("Begin validation of fic ID " + FicMiniItem.Id);

            logger.Trace("The fic failed ot validate, returning false.");
            return false;
        }
        private void ScrapeFic(AO3FicMiniItem FicMiniItem)
        {
            logger.Debug("Scraping fic ID " + FicMiniItem.Id);
            //scrape fic page, parse metadata, update db, download file
            logger.Trace("Downloading and parsing the fic page");
            AO3FicPage FicPage = AO3FicParser.ParseFicPage(HTTPReader.GetPage(AO3BaseURI + "/works/" + FicMiniItem.Id + "?view_adult=true"));
            logger.Trace("Queueing up the downloader");
            DownloadFic(AO3BaseURI + FicPage.DownloadPath, TempDir + "\\" + FicPage.Id + ".epub").RunSynchronously();
            System.Threading.Thread.Sleep(5000);

        }
        private async Task<bool> DownloadFic(string fileUrl, string pathToSave)
        {
            logger.Trace("Begining fic download from URI " + fileUrl + " and saving it to " + pathToSave);
            var httpResult = await httpClient.GetAsync(fileUrl);
            using var resultStream = await httpResult.Content.ReadAsStreamAsync();
            using var fileStream = File.Create(pathToSave);
            resultStream.CopyTo(fileStream);
            logger.Trace("Download complete.");
            return true;
        }
    }
}
