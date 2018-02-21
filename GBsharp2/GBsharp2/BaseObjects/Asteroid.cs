using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBsharp2.Primitives;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace GBsharp2.BaseObjects
{
	class Asteroid : BaseObject
	{
		public static byte Sizes { get; } = 3;
		private byte _asteroidSize;
		public int DefaultBounty { get; set; } = 24;
		public int DestroyBounty { get { return DefaultBounty / (_asteroidSize + 1); } }
		public int MissPenalty { get { return DefaultBounty / (Sizes + 1 - _asteroidSize); } }


		public Asteroid(Grid grid, Position pos, GBsharp2.Primitives.Vector vec, double size = 1.0) : base(grid, pos, vec, size)
		{
			
		}

		protected override void DrawOnCanvas()
		{
			if (_canvas == null)
			{
				_canvas = new Canvas();
			}
			else
			{
				_canvas.Children.Clear();
			}

			double wh = DefaultSize * _size / (_pos.Z + 1) * _asteroidSize;
			Ellipse ellipse = new Ellipse()
			{
				Width = wh,
				Height = wh,
				Fill = Brushes.Transparent,
				Stroke = _brush
			};
			_canvas.Children.Add(ellipse);
			_canvas.Width = wh;
			_canvas.Height = wh;
		}
	}
}
