using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace AO3Scraper.Web
{
    internal static class HTTPReader
    {
        internal static string GetPage(string URI)
        {
            return CallUrl(URI).Result;
        }
        private static async Task<string> CallUrl(string URI)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(URI);
            return response;
        }
    }
}
