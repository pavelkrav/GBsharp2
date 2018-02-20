using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBsharp2.BaseObjects;
using GBsharp2.Primitives;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GBsharp2.GameEntities
{
	class Background
	{
		protected List<Star> _stars;
		protected Grid _grid;
		public bool Initialized { get; protected set; } = false;

		public Background(Grid grid)
		{
			_stars = new List<Star>();
			_grid = grid;
		}

		public void Init(List<Star> stars)
		{
			if (stars != null)
			{
				_stars = stars;
				Initialized = true;
			}
		}

		public void Init(int stars)
		{
			if (stars > 0)
			{
				PRandom pr = new PRandom();
				for (int i = 0; i < stars; i++)
				{
					_stars.Add(new Star(_grid,
						new Position(pr.Next((int)_grid.Width), pr.Next((int)_grid.Height), pr.NextDouble()),
						new Vector(-15, 0)));
				}
				Initialized = true;
			}
		}

		public void Draw()
		{
			foreach (var s in _stars)
			{
				s.Draw();
			}
		}

		public void Update(int fps = 1)
		{
			foreach (var s in _stars)
			{
				s.Update(fps);
			}
		}
	}
}
