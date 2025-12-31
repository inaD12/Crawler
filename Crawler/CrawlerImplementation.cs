using Crawler.Models;
using HtmlAgilityPack;

namespace Crawler
{
	public class CrawlerImplementation
	{
		private HttpClient _client = new HttpClient();
		private RobotsChecker _robotsChecker = new RobotsChecker();
		private HashSet<string> _visited = new HashSet<string>();
		private DataHandler _dataHandler = new DataHandler();

		public async Task Crawl(string url, int depth = 0, int maxDepth = 2)
		{
			if (depth > maxDepth || _visited.Contains(url)) return;
			_visited.Add(url);

			if (!await _robotsChecker.CanCrawl(url))
			{
				Console.WriteLine($"Skipping {url} (blocked by robots.txt)");
				return;
			}

			try
			{
				var response = await _client.GetAsync(url);
				if (!response.IsSuccessStatusCode) return;

				var html = await response.Content.ReadAsStringAsync();
				HtmlDocument doc = new HtmlDocument();
				doc.LoadHtml(html);

				var pageData = new PageData { Url = url };

				var titles = doc.DocumentNode.SelectNodes("//h1") ?? new HtmlNodeCollection(null);
				foreach (var t in titles)
					pageData.Titles.Add(t.InnerText.Trim());

				var paragraphs = doc.DocumentNode.SelectNodes("//p") ?? new HtmlNodeCollection(null);
				foreach (var p in paragraphs)
					pageData.Paragraphs.Add(p.InnerText.Trim());

				_dataHandler.Save(pageData);

				Console.WriteLine($"Crawled {url} | Titles: {pageData.Titles.Count} | Paragraphs: {pageData.Paragraphs.Count}");

				var links = doc.DocumentNode.SelectNodes("//a[@href]");
				if (links != null)
				{
					foreach (var link in links)
					{
						string href = link.GetAttributeValue("href", "");
						if (Uri.TryCreate(href, UriKind.Absolute, out Uri result))
						{
							await Crawl(result.ToString(), depth + 1, maxDepth);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error crawling {url}: {ex.Message}");
			}
		}
	}
}
