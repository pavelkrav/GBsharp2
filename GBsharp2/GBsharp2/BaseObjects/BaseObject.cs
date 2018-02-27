using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBsharp2.Primitives;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GBsharp2.BaseObjects
{
	public abstract class BaseObject
	{
		protected Position _pos;
		protected Vector _vec;
		protected double _size;

		public static int DefaultSize { get; } = 10;
		public static uint TotalObjects { get; private set; } = 0;

		protected Canvas _canvas;
		protected Grid _grid;
		protected bool _onGrid = false;
		protected Brush _brush = Brushes.White;

		public BaseObject(Grid grid, Position pos, Vector vec, double size = 1.0)
		{
			_pos = pos;
			_vec = vec;
			_size = size;

			_grid = grid;
			_canvas = null;

			TotalObjects++;
		}

		~BaseObject()
		{
			TotalObjects--;
		}

		protected virtual void DrawOnCanvas()
		{
			if (_canvas == null)
			{
				_canvas = new Canvas();
			}
			else
			{
				_canvas.Children.Clear();
			}

			double wh = DefaultSize * _size / (_pos.Z + 1);
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

		public virtual void Draw()
		{
			if (!(_canvas != null && _vec.Z == 0))
				DrawOnCanvas();
			if (!_onGrid)
			{
				_grid.Children.Add(_canvas);
				_onGrid = true;
			}

			_canvas.Margin = new System.Windows.Thickness(_pos.X, _pos.Y, _grid.Width - (_pos.X + _canvas.Width), _grid.Height - (_pos.Y + _canvas.Height));
		}

		public virtual void Remove()
		{
			if (_onGrid)
			{
				_grid.Children.Remove(_canvas);
				_canvas = null;
				_onGrid = false;
			}
		}

		public virtual void Update(double fps = 1)
		{
			_pos.X += _vec.X / (_pos.Z + 1) / fps;
			_pos.Y += _vec.Y / (_pos.Z + 1) / fps;
			_pos.Z += _vec.Z / fps;
			if (_pos.Z < 0)
				_pos.Z = 0;
		}
	}
}
