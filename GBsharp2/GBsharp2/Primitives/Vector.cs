using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBsharp2.Primitives
{
	public class Vector
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public Vector()
		{
			X = 0;
			Y = 0;
			Z = 0;
		}

		public Vector(double x, double y, double z = 0)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector Copy()
		{
			return new Vector(X, Y, Z);
		}

		public static Vector operator +(Vector left, Vector right)
		{
			return new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
	}
}
