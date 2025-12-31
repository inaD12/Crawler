namespace Crawler.Models
{
	public class PageData
	{
		public string Url { get; set; }
		public List<string> Titles { get; set; } = new List<string>();
		public List<string> Paragraphs { get; set; } = new List<string>();
		public DateTime CrawledAt { get; set; } = DateTime.Now;
	}
}
