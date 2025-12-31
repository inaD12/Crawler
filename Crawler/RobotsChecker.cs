namespace Crawler
{
	public class RobotsChecker
	{
		private HttpClient _client = new HttpClient();

		public async Task<bool> CanCrawl(string url)
		{
			try
			{
				Uri uri = new Uri(url);
				string robotsUrl = uri.Scheme + "://" + uri.Host + "/robots.txt";
				var response = await _client.GetAsync(robotsUrl);
				if (!response.IsSuccessStatusCode) return true;

				string robotsContent = await response.Content.ReadAsStringAsync();
				string[] lines = robotsContent.Split('\n');
				foreach (var line in lines)
				{
					if (line.StartsWith("Disallow:", StringComparison.OrdinalIgnoreCase))
					{
						string path = line.Substring("Disallow:".Length).Trim();
						if (uri.AbsolutePath.StartsWith(path)) return false;
					}
				}
				return true;
			}
			catch
			{
				return true;
			}
		}
	}
}
