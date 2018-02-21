using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBsharp2.BaseObjects;
using GBsharp2.Primitives;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GBsharp2.GameEntities
{
	class AsteroidField
	{
		private List<Asteroid> _asteroids;
		private Grid _grid;
		public bool Initialized { get; private set; } = false;

		public AsteroidField(Grid grid)
		{
			_asteroids = new List<Asteroid>();
			_grid = grid;
		}

		//public void Init()
	}
}
