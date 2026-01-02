using Crawler.Models;
using System.Text.Json;

namespace Crawler
{
	public class DataHandler
	{
		private string _folder = "../../../../data";

		public DataHandler()
		{
			if (!Directory.Exists(_folder))
				Directory.CreateDirectory(_folder);
		}

		public void Save(PageData data)
		{
			string filename = Path.Combine(_folder, SanitizeFilename(data.Url) + ".json");
			File.WriteAllText(filename, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
		}

		public List<PageData> LoadAll()
		{
			var pages = new List<PageData>();
			foreach (var file in Directory.GetFiles(_folder, "*.json"))
			{
				string json = File.ReadAllText(file);
				pages.Add(JsonSerializer.Deserialize<PageData>(json));
			}
			return pages;
		}

		private string SanitizeFilename(string url)
		{
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				url = url.Replace(c, '_');
			}
			return url.Replace("://", "_").Replace("/", "_");
		}
	}
}
