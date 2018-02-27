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
	public class Player : BaseObject
	{
		public Brush Color { get; set; } = Brushes.BlueViolet;

		public static double Acceleration { get; set; } = 140.0;
		public static double Damping { get; set; } = 55.0;
		public static double BumpDamping { get; set; } = 2.0;

		public double HP { get; private set; }

		public List<Missile> Missiles { get; private set; }
		private int _lastShot = 0;
		private double _shotsPerSec = 2.5;

		public Position Pos { get { return _pos.Copy(); } }
		public GBsharp2.Primitives.Vector Vec { get { return _vec.Copy(); } }
		public double Height { get { return _canvas.Height; } }
		public double Width { get { return _canvas.Width; } }

		public Player(Grid grid, Position pos, GBsharp2.Primitives.Vector vec, double size = 1.0) : base(grid, pos, vec, size)
		{
			Missiles = new List<Missile>();
			HP = 10;
		}

		public event EventHandler<EventArgs> Death;

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

			double h = DefaultSize * 1.3 / (_pos.Z + 1);
			double w = DefaultSize * 1.7 / (_pos.Z + 1);

			Polygon poly = new Polygon()
			{
				Points = new PointCollection()
				{
					new Point(0, 0),
					new Point(0, h),
					new Point(w, h / 2),
				},
				Stroke = Color,
				Fill = Color
			};

			_canvas.Children.Add(poly);
			_canvas.Width = w;
			_canvas.Height = h;
		}

		public override void Remove()
		{
			base.Remove();
			for (int i = 0; i < Missiles.Count; i++)
			{
				Missiles[i].Remove();
			}
			Missiles = null;
			Death = null;
		}

		public override void Draw()
		{
			base.Draw();
			for (int i = 0; i < Missiles.Count; i++)
			{
				Missiles[i].Draw();
			}
		}

		public void CreateMissile(double fps)
		{
			if (_lastShot >= fps / _shotsPerSec)
			{
				Position p = Pos;
				p.Y += Height / 2;
				Missile m = new Missile(_grid, p, new GBsharp2.Primitives.Vector(Missile.Acceleration, 0) + Vec);
				m.Draw();
				Missiles.Add(m);
				_lastShot = 0;
			}
		}

		public void HitByAsteroid(Asteroid asteroid)
		{
			HP = 0;
		}

		public void KeyUp(double fps = 1)
		{
			_vec.Y -= Acceleration / fps;
		}

		public void KeyDown(double fps = 1)
		{
			_vec.Y += Acceleration / fps;
		}

		public void KeyRight(double fps = 1)
		{
			_vec.X += Acceleration / fps;
		}

		public void KeyLeft(double fps = 1)
		{
			_vec.X -= Acceleration / fps;
		}

		private void TickDamping(double fps = 1)
		{
			if (_vec.Y > 0)
			{
				_vec.Y -= Damping / fps;
				if (_vec.Y < 0)
					_vec.Y = 0;
			}
			else if (_vec.Y < 0)
			{
				_vec.Y += Damping / fps;
				if (_vec.Y > 0)
					_vec.Y = 0;
			}

			if (_vec.X > 0)
			{
				_vec.X -= Damping / fps;
				if (_vec.X < 0)
					_vec.X = 0;
			}
			else if (_vec.X < 0)
			{
				_vec.X += Damping / fps;
				if (_vec.X > 0)
					_vec.X = 0;
			}
		}

		public override void Update(double fps = 1)
		{
			base.Update(fps);
			TickDamping(fps);

			if (_pos.X < 0)
			{
				_pos.X = 0;
				_vec.X = -_vec.X / BumpDamping;
			}
			else if (_pos.X > _grid.Width - _canvas.Width - 30)
			{
				_pos.X = _grid.Width - _canvas.Width - 30;
				_vec.X = -_vec.X / BumpDamping;
			}

			if (_pos.Y < 0)
			{
				_pos.Y = 0;
				_vec.Y = -_vec.Y / BumpDamping;
			}
			else if (_pos.Y > _grid.ActualHeight - _canvas.ActualHeight - 45)
			{
				_pos.Y = _grid.ActualHeight - _canvas.ActualHeight - 45;
				_vec.Y = -_vec.Y / BumpDamping;
			}

			for (int i = 0; i < Missiles.Count; i++)
			{
				Missiles[i].Update(fps);
				if (Missiles[i].ToRemove)
				{
					Missiles[i].Remove();
					Missiles.Remove(Missiles[i]);
				}
			}

			if (_lastShot < fps / _shotsPerSec)
				_lastShot++;

			if (HP <= 0)
				Death?.Invoke(this, new EventArgs());
		}
	}
}
