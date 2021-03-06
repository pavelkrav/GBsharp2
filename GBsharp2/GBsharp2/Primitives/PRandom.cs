﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBsharp2.Primitives
{
	public class PRandom
	{
		public static int Seed { get; private set; } = new Random().Next();
		public static Random SeedMultiplier { get; private set; } = new Random();
		private Random _rand;

		public PRandom()
		{
			if (Seed == int.MaxValue)
				Seed = int.MinValue;
			_rand = new Random((int)(Seed++ * SeedMultiplier.NextDouble()));
		}

		public int Next()
		{
			return _rand.Next();
		}

		public int Next(int lowerThan)
		{
			return _rand.Next(lowerThan);
		}

		public int Next(int min, int max)
		{
			return _rand.Next(min, max);
		}

		public double NextDouble()
		{
			return _rand.NextDouble();
		}
	}
}
