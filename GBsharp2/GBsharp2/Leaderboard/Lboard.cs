using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace GBsharp2.Leaderboard
{
	class Lboard
	{
		public List<ExPlayer> Players { get; set; }
		public static string PathToFile { get; } = @"..\..\Leaderboard\ldb.json";

		public Lboard()
		{
			Players = new List<ExPlayer>();
		}

		public static Lboard Load()
		{
			string data = null;
			using (StreamReader file = new StreamReader(PathToFile))
			{
				data = file.ReadToEnd();
			}
			try
			{
				if (data == null || data.Length == 0)
					throw new Exception();
				return JsonConvert.DeserializeObject(data, typeof(Lboard),
				new JsonSerializerSettings()
				{
					TypeNameHandling = TypeNameHandling.All
				}) as Lboard;
			}
			catch
			{
				return new Lboard();
			}
		}

		public void Save()
		{
			var jsonString = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings());
			using (StreamWriter file = new StreamWriter(PathToFile))
			{
				file.Write(jsonString);
			}
		}

		public string GetLeaders(int count = 10)
		{
			string result = "";
			var list = Players.OrderBy(e => -e.Score).ToList();
			if (list.Count() < count)
				count = list.Count();
			for (int i = 0; i < count; i++)
			{
				result += $"{i + 1}. {list[i]}\n";
			}
			return result;
		}
	}
}
