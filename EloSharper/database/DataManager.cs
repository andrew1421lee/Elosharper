using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace EloSharper.database
{
	public class DataManager
	{
		private static string FileLocation = "./data/players.json";
		private static DataManager _instance = new DataManager();

		public static void Load()
		{
			if (!File.Exists(FileLocation))
			{
				throw new FileNotFoundException();
				//File does not exist, handle it!
			}
			_instance = JsonConvert.DeserializeObject<DataManager>(File.ReadAllText(FileLocation));
		}

		public static void Save()
		{
			_instance.db.modified = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			using (var stream = new FileStream(FileLocation, FileMode.Create, FileAccess.Write, FileShare.None))
			using (var writer = new StreamWriter(stream))
				writer.Write(JsonConvert.SerializeObject(_instance, Formatting.Indented));
		}

		public class Database
		{
			[JsonProperty("title")]
			public string title;
			[JsonProperty("created")]
			public ulong created;
			[JsonProperty("modified")]
			public ulong modified;
			[JsonProperty("games")]
			public List<Game> Games;
			[JsonProperty("players")]
			public List<Player> Players;
		}

		public class Player
		{
			[JsonProperty("name")]
			public string name;
			[JsonProperty("aliases")]
			public List<string> aliases;
			[JsonProperty("id")]
			public string id;
			[JsonProperty("rating")]
			public double rating;
			[JsonProperty("index")]
			public int index;
			[JsonProperty("totalgames")]
			public int totalgames;
			[JsonProperty("wins")]
			public int wins;
			[JsonProperty("draws")]
			public int draws;
		}

		public class Game
		{
			[JsonProperty("index")]
			public int index;
			[JsonProperty("id")]
			public string id;
			[JsonProperty("datetime")]
			public string DateTime;
			[JsonProperty("players")]
			public List<string> Players;
			[JsonProperty("winner")]
			public int winner;
		}

		[JsonProperty("database")]
		private Database db = new Database();
		public static Database Data => _instance.db;
	}
}
