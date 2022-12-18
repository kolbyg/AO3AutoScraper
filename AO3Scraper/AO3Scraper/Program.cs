using AO3Scraper.Web;
using AO3Scraper.Parsers;
using System.IO;
using System;
using NLog;

namespace AO3Scraper // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NLog.LogManager.Setup().LoadConfiguration(builder => {
                builder.ForLogger().FilterMinLevel(LogLevel.Trace).WriteToConsole();
                builder.ForLogger().FilterMinLevel(LogLevel.Trace).WriteToFile(fileName: "C:\\logs\\scraper.log");
            });
            Console.WriteLine("AO3 Scraper");
            Console.WriteLine("V0.01");
            Console.WriteLine();
            Console.WriteLine("Begin Fetching URIs to scrape");
            Config Config = new Config();
            Config.LoadConfig();
            Controllers.AO3ScrapeController aO3ScrapeController = new Controllers.AO3ScrapeController();
            //TODO Get URIs from config

            //testing...
            //aO3ScrapeController.ScrapeURI("https://archiveofourown.org/works/42896025","");

            aO3ScrapeController.ScrapeURI("https://archiveofourown.org/tags/No%20Archive%20Warnings%20Apply/works", "?");



            Console.ReadLine();
        }
    }
}