using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBsharp2.Leaderboard
{
	class ExPlayer
	{
		public string Name { get; set; }
		public double Score { get; set; }

		public ExPlayer(string name, double score)
		{
			Name = name;
			Score = score;
		}

		public override string ToString()
		{
			return $"{Name}: {Score.ToString("F0")}";
		}
	}
}
