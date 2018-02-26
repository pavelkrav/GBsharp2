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

		public AsteroidField(Grid grid)
		{
			_asteroids = new List<Asteroid>();
			_grid = grid;
		}

		public event EventHandler<AddScoreEventArgs> AddScore;

		public void Init()
		{
			if (!Initialized)
			{
				_initTime = DateTime.Now;
				Initialized = true;
				for (int i = 0; i < 20; i++)
				{
					AddAsteroid();
				}
			}
		}

		private Asteroid CreateAsteroid()
		{
			PRandom pr = new PRandom();
			return new Asteroid(_grid, new Position((int)_grid.Width, pr.Next((int)(_grid.Height - 80))), // -80
				new	Vector(pr.Next((int)(-Asteroid.AsteroidsDefaultSpeed * 2), (int)(-Asteroid.AsteroidsDefaultSpeed)), pr.Next((int)(-Asteroid.AsteroidsDefaultSpeed / 2), (int)(Asteroid.AsteroidsDefaultSpeed / 2))),
				asteroidSize: 3);
		}

		private void AddAsteroid()
		{
			Asteroid a = CreateAsteroid();
			a.AsteroidDestroyed += OnAsteroidDestroyed;
			a.AsteroidMissed += OnAsteroidMissed;
			_asteroids.Add(a);
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
			for (int i = 0; i < _asteroids.Count; i++)
			{
				_asteroids[i].Update(fps);
				if (_asteroids[i].ToRemove)
				{
					_asteroids[i].Remove();
					_asteroids.Remove(_asteroids[i]);
				}
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

		public void OnAsteroidMissed(object sender, EventArgs e)
		{
			AddScore?.Invoke(this, new AddScoreEventArgs(-(sender as Asteroid).MissPenalty));
		}

		public void OnAsteroidDestroyed(object sender, EventArgs e)
		{
			AddScore?.Invoke(this, new AddScoreEventArgs(-(sender as Asteroid).DestroyBounty));
		}
	}

	public class AddScoreEventArgs : EventArgs
	{
		public double Score { get; private set; }

		public AddScoreEventArgs(double score)
		{
			Score = score;
		}
	}
}
