using Crawler;

class Program
{
	static async Task Main(string[] args)
	{
		Console.WriteLine("Enter the URL to crawl:");
		string url = Console.ReadLine();

		var crawler = new CrawlerImplementation();
		await crawler.Crawl(url, 0, 2);

		DataHandler dataHandler = new DataHandler();
		var pages = dataHandler.LoadAll();

		Console.WriteLine("\nCrawled Pages:");
		foreach (var page in pages)
		{
			Console.WriteLine($"\nURL: {page.Url}");
			Console.WriteLine($"Crawled At: {page.CrawledAt}");
			Console.WriteLine($"Titles: {string.Join(", ", page.Titles)}");
			Console.WriteLine($"Paragraphs: {page.Paragraphs.Count} paragraphs");
		}

		Console.WriteLine("\nDone!");
	}
}
