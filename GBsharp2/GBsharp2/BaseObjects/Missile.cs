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
	class Missile : BaseObject
	{
		public Brush Color { get; set; } = Brushes.BlueViolet;
		public static double Acceleration { get; set; } = 300.0;
		public static double Damping { get; set; } = 25.0;
		public bool ToRemove { get; set; } = false;

		public Missile(Grid grid, Position pos, GBsharp2.Primitives.Vector vec, double size = 1.0) : base(grid, pos, vec, size)
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

			double w = DefaultSize;
			double h = 2;

			Line line = new Line()
			{
				X1 = 0,
				Y1 = 0,
				X2 = w,
				Y2 = 0,
				Stroke = Color,
				StrokeThickness = h
			};

			_canvas.Children.Add(line);
			_canvas.Width = w;
			_canvas.Height = h;
		}

		private void TickDamping(double fps = 1)
		{
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

			if (_pos.X > _grid.Width - _canvas.Width || _vec.X == 0)
			{
				ToRemove = true;
			}

			if (_pos.Y < 0)
			{
				_vec.Y = -_vec.Y;
			}
			else if (_pos.Y > _grid.ActualHeight - _canvas.ActualHeight - 45)
			{
				_vec.Y = -_vec.Y;
			}
		}
	}
}
