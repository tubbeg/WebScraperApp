using System;
using System.Threading.Tasks;

namespace WebScraperApp
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var webScraper = new WebScraper();
            var myResult = await webScraper.Scrape("http://reddit.com/r/all");
            webScraper.LoadDocument(myResult);
            var myList = webScraper.GetAllImageSources();
            foreach(string image in myList)
                Console.WriteLine(image);
        }
    }
}
