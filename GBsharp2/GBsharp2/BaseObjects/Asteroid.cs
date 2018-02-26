using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBsharp2.Primitives;
using GBsharp2.GameEntities;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace GBsharp2.BaseObjects
{
	class Asteroid : BaseObject
	{
		public Brush Color { get; set; } = Brushes.Gray;

		public static byte Sizes { get; } = 3;
		private byte _asteroidSize;
		public static double DefaultBounty { get; set; } = 24.0;
		public double DestroyBounty { get { return DefaultBounty / (_asteroidSize + 1); } }
		public double MissPenalty { get { return DefaultBounty / (Sizes + 1 - _asteroidSize); } }
		public static double AsteroidsDefaultSpeed { get; set; } = 35;

		public bool ToRemove { get; set; } = false;

		public event EventHandler<EventArgs> AsteroidMissed;
		public event EventHandler<EventArgs> AsteroidDestroyed;
		
		public Asteroid(Grid grid, Position pos, GBsharp2.Primitives.Vector vec, byte asteroidSize, double size = 1.0) : base(grid, pos, vec, size)
		{
			_asteroidSize = asteroidSize;	
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
				Fill = Color,
				Stroke = Color
			};
			_canvas.Children.Add(ellipse);
			_canvas.Width = wh;
			_canvas.Height = wh;
		}

		public override void Update(double fps = 1)
		{
			base.Update(fps);
			if (_pos.X < 0 - _canvas.Width)
			{
				ToRemove = true;
				AsteroidMissed?.Invoke(this, new EventArgs());
			}
			else if (_pos.Y < 0)
			{
				_vec.Y = -_vec.Y;
			}
			else if (_pos.Y > _grid.Height - _canvas.Height - 45)
			{
				_vec.Y = -_vec.Y;
			}
		}

		public List<Asteroid> Destroy()
		{
			List<Asteroid> result = new List<Asteroid>();
			ToRemove = true;
			Remove();
			AsteroidDestroyed?.Invoke(this, new EventArgs());
			if (_asteroidSize != 1)
			{
				Asteroid a1 = new Asteroid(_grid, _pos.Copy(), _vec.Copy(), asteroidSize: (byte)(_asteroidSize - 1));

				Position p2 = _pos.Copy();
				p2.X += DefaultSize;
				p2.Y += DefaultSize;
				PRandom pr = new PRandom();
				GBsharp2.Primitives.Vector v2 = new GBsharp2.Primitives.Vector(pr.Next((int)(-AsteroidsDefaultSpeed * 2), (int)(-AsteroidsDefaultSpeed)), pr.Next((int)(-AsteroidsDefaultSpeed / 2), (int)(AsteroidsDefaultSpeed / 2)));
				Asteroid a2 = new Asteroid(_grid, p2, v2, asteroidSize: (byte)(_asteroidSize - 1));

				result.Add(a1);
				result.Add(a2);
			}
			return result;
		}
	}
}
