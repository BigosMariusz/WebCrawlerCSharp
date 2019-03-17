using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Linq;
using WebCrawler.Data;
using System.Threading.Tasks;
using System.Net.Http;
using WebCrawler.Services;
using System.Threading;

namespace WebCrawler
{
    class Crawler
    {
        public async Task<int> CrawlAsync(string url, int hopSize, int pauseLength)
        {
            int counterOfVisits = 0;
            Console.Clear();
            Console.WriteLine($"Przeszukiwanie linków strony: {url} na głębokość {hopSize} skoków.");
            await DbService.SaveFirstLinkAsync(url);

            for (int tier = 0; tier < hopSize; tier++)
            {
                var tierLength = await DbService.CountTierItemsAsync(tier);

                for (int position = 0; position < tierLength; position += 100)
                {
                    var linksAndHtml = await RunDownloadAsync(DbService.Get100Links(tier, position, tierLength));
                    var tempList = new List<DbLink>();

                    foreach (var linkWithHtml in linksAndHtml)
                    {
                        try
                        {

                            if (!String.IsNullOrEmpty(linkWithHtml.Content))
                            {
                                var aTags = GetHtmlTags(linkWithHtml);

                                Parallel.ForEach(aTags, (tag) =>
                                {
                                    if (LinkService.IsValidLink(tag?.ChildAttributes("href")?.FirstOrDefault()?.Value))
                                    {
                                        var newLink = CreateNewLink(tag, linkWithHtml.Url, linkWithHtml.Id, tier);
                                        tempList.Add(newLink);
                                    }
                                });
                            }
                        }
                        catch (Exception) { }
                    }
                    Console.WriteLine($"Odwiedzono około {counterOfVisits} stron.");
                    counterOfVisits += 100;
                    DbService.SaveLinksInDb(tempList);
                    Thread.Sleep(pauseLength*1000);
                }
            }
            return await DbService.CountAllLinksAsync();
        }

        private DbLink CreateNewLink(HtmlNode tag, string parentUrl, Guid Id, int actualTier)
        {
            string url = tag?.ChildAttributes("href")?.FirstOrDefault()?.Value;

            var isFullPath = LinkService.IsFullPath(url);
            if (!isFullPath)
            {
                url = LinkService.GetEalierPath(parentUrl) + LinkService.PrepareUrl(url);
            }

            var newLink = new DbLink
            {           
                Url = url,
                ParentId = Id,
                Domain = LinkService.GetDomain(url),
                IsInternal = LinkService.IsInternalLink(url, parentUrl),
                Tier = actualTier + 1
            };
            return newLink;
        }

        private IEnumerable<HtmlNode> GetHtmlTags(WebFormat linkWithHtml)
        {
            var htmlDoc = new HtmlDocument();
            var html = linkWithHtml.Content;
            htmlDoc.LoadHtml(html);
            var aTags = htmlDoc.DocumentNode.Descendants("a");
            return aTags;
            
        }

        private async Task<IEnumerable<WebFormat>> RunDownloadAsync(List<DbLink> websites)
        {
            var tasks = new List<Task<WebFormat>>();

            foreach (var site in websites)
            {
                tasks.Add(DownloadAsync(site.Url, site.Id));
            }

            var result = await Task.WhenAll(tasks);

            return result;
        }

        private async Task<WebFormat> DownloadAsync(string url, Guid id)
        {
            var webContainer = new WebFormat();
            webContainer.Url = url;
            webContainer.Id = id;
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(12000);
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                try
                {
                    webContainer.Content = await httpClient.GetStringAsync(url);
                }
                catch (Exception) { }
            }

            return webContainer;
        }

        private int GetTierLength(int tier, List<DbLink> listLinks)
        {
            var length = listLinks
                .Where(link => link.Tier == tier)
                .Count();

            return length;
        }
    }
}
