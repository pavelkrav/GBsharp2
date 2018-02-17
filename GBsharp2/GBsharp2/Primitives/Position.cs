using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GBsharp2.Primitives
{
	class Position
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

		public Point ToPoint()
		{
			return new Point(X, Y);
		}
	}
}
