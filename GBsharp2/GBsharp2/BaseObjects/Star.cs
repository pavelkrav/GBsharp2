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
	class Star : BaseObject
	{
		private int _drawStyle;
		private static int DrawStyles { get; } = 2;

		public Star(Grid grid, Position pos, Vector vec, double size = 1.0) : base(grid, pos, vec, size)
		{
			_drawStyle = new PRandom().Next(DrawStyles);
		}

		protected override void DrawOnCanvas()
		{
			switch (_drawStyle) {
				case 0:
					base.DrawOnCanvas();
					break;
				case 1:
					if (_canvas == null)
					{
						_canvas = new Canvas();
					}
					else
					{
						_canvas.Children.Clear();
					}
					double wh = DefaultSize / 1.7 / (_pos.Z + 1);
					Line line1 = new Line() { X1 = 0, Y1 = 0, X2 = wh, Y2 = wh, Stroke = _brush };
					Line line2 = new Line() { X1 = wh, Y1 = 0, X2 = 0, Y2 = wh, Stroke = _brush };
					_canvas.Children.Add(line1);
					_canvas.Children.Add(line2);
					_canvas.Width = wh;
					_canvas.Height = wh;
					break;
				default:
					base.DrawOnCanvas();
					break;
			}
		}

		public override void Update(int fps = 1)
		{
			base.Update(fps);
			if (_pos.X < 0 - _canvas.Width)
			{
				_pos.X = _grid.Width + _canvas.Width;
				_pos.Y = new PRandom().Next((int)_grid.Height);
				_pos.Z = new PRandom().NextDouble();
			}
		}
	}
}
