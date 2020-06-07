using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace WebScraperApp
{
    class WebScraper : IWebScraper
    {
        private readonly HttpClient _client;
        private readonly HtmlDocument _htmlDocument;

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
            return _htmlDocument.DocumentNode.SelectSingleNode(query);
        }

        //(//div[contains(@class,'pb-f-homepage-hero')]//h3)[1]
        public string GetNodeContent(string query)
        {
            return GetNode(query).InnerText;
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

        public string GetNodeContent(string query)
        {
            return "";
        }
    }

    interface IWebScraper
    {
        public Task<string> Scrape(string pageUrl);

        public void LoadDocument(string pageContents);

        public string GetNodeContent(string query);
    }
}