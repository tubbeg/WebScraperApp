using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace WebScraperApp
{
    class WebScraper : IWebScraper
    {
        private HttpClient _client;
        private HtmlDocument _htmlDocument;

        public WebScraper()
        {
            _htmlDocument = new HtmlDocument();
            _client = new HttpClient();
        }

        public async Task<string> Scrape(string pageUrl)
        {
            var response = await _client.GetAsync(pageUrl);
            return await response.Content.ReadAsStringAsync();
        }

        public void LoadDocument(string pageContents)
        {
            _htmlDocument.LoadHtml(pageContents);
        }

        private HtmlNode GetNode(string query) 
        {
            var result = _htmlDocument.DocumentNode.SelectSingleNode(query);
            return result;
        }

        //(//div[contains(@class,'pb-f-homepage-hero')]//h3)[1]
        public string GetNodeText(string query)
        {
            return GetNode(query).InnerText;
        }

        public IEnumerable<string> GetAllImageSources()
        {
            return _htmlDocument.DocumentNode.Descendants("img")
                .Select(e => e.GetAttributeValue("src", null))
                .Where(s => !String.IsNullOrEmpty(s));
        }
    }

    public class WebScraperDummy : IWebScraper
    {
        private readonly string _pageContents;
        public WebScraperDummy()
        {
            _pageContents = "<!doctype html><title>Hello from WebScraper</title><h1> Hello, World!</h1>";
        }

        public Task<string> Scrape(string pageUrl)
        {
            return Task.FromResult(_pageContents);
        }

        public void LoadDocument(string pageContents) 
        {
            return;
        }

        public string GetNodeText(string query)
        {
            return "node content return value";
        }

        public IEnumerable<string> GetAllImageSources()
        {
            return new List<string>();
        }
    }

    interface IWebScraper
    {
        public Task<string> Scrape(string pageUrl);

        public void LoadDocument(string pageContents);

        public string GetNodeText(string query);
        public IEnumerable<string> GetAllImageSources();
    }
}