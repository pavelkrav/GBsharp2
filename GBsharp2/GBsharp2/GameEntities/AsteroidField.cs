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
		private DateTime _initTime;

		public static double AsteroidsDefaultSpeed { get; set; } = 35;

		public AsteroidField(Grid grid)
		{
			_asteroids = new List<Asteroid>();
			_grid = grid;
		}

		public void Init()
		{
			if (!Initialized)
			{
				_initTime = DateTime.Now;
				Initialized = true;
				_asteroids.Add(CreateAsteroid());
				_asteroids.Add(CreateAsteroid());
				_asteroids.Add(CreateAsteroid());
			}
		}

		private Asteroid CreateAsteroid()
		{
			PRandom pr = new PRandom();
			return new Asteroid(_grid, new Position((int)_grid.Width, pr.Next((int)_grid.Height)),
				new	Vector(pr.Next((int)-AsteroidsDefaultSpeed * 2, (int)-AsteroidsDefaultSpeed), 0),
				asteroidSize: 3);
		}

		public void Draw()
		{
			foreach (var a in _asteroids)
			{
				a.Draw();
			}
		}

		public void Update(double fps = 1)
		{
			foreach (var a in _asteroids)
			{
				a.Update(fps);
			}
			// add new asteroids here
		}

		public void Remove()
		{
			if (Initialized)
			{
				for (int i = 0; i < _asteroids.Count; i++)
				{
					_asteroids[i].Remove();
				}
				_asteroids = null;
				Initialized = false;
			}
		}
	}
}
