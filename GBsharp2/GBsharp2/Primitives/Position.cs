using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GBsharp2.Primitives
{
	public class Position
	{
		public double X { get; set; }
		public double Y { get; set; }
		/// <summary>
		/// Depth
		/// </summary>
		public double Z { get; set; }

		public Position()
		{
			X = 0;
			Y = 0;
			Z = 0;
		}

		public Position (double x, double y, double z = 0)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static double DistanceD2 (Position p1, Position p2)
		{
			return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
		}

		public static double DistanceD3(Position p1, Position p2)
		{
			return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y) + (p2.Z - p1.Z) * (p2.Z - p1.Z));
		}

		public Position Copy()
		{
			return new Position(X, Y, Z);
		}

		public Point ToPoint()
		{
			return new Point(X, Y);
		}
	}
}
