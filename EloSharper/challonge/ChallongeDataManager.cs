using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;

namespace EloSharper.challonge
{
	public class ChallongeDataManager
	{
		public static JObject EntireData;
		public static JArray Participants;
		public static JArray Matches;
		public static bool Load(string filelocation)
		{
			if (!File.Exists(filelocation))
			{
				return false;
			}
			EntireData = (JObject)(JsonConvert.DeserializeObject(File.ReadAllText(filelocation)));
			EntireData = (JObject)EntireData["tournament"];

			Participants = (JArray)EntireData["participants"];
			Matches = (JArray)EntireData["matches"];
			return true;
		}
	}
}
