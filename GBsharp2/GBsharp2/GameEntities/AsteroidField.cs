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
	public class AsteroidField
	{
		private List<Asteroid> _asteroids;
		private Grid _grid;
		public bool Initialized { get; private set; } = false;
		private DateTime _initTime;
		private PRandom _rand;

		public static double CommonChance { get; set; } = 0.12;
		public static double ChanceMinuteStep { get; set; } = 0.04;
		public static double ChanceLimit { get; set; } = 0.4;

		public AsteroidField(Grid grid)
		{
			_asteroids = new List<Asteroid>();
			_rand = new PRandom();
			_grid = grid;
		}

		public event EventHandler<AddScoreEventArgs> AddScore;

		public void Init()
		{
			if (!Initialized)
			{
				_initTime = DateTime.Now;
				Initialized = true;
				AddAsteroid();
			}
		}

		private Asteroid CreateAsteroid()
		{
			PRandom pr = new PRandom();
			return new Asteroid(_grid, new Position((int)_grid.Width, pr.Next((int)(_grid.Height - 80))), // -80 wtf
				new Vector(pr.Next((int)(-Asteroid.AsteroidsDefaultSpeed * 2), (int)(-Asteroid.AsteroidsDefaultSpeed)), pr.Next((int)(-Asteroid.AsteroidsDefaultSpeed / 2), (int)(Asteroid.AsteroidsDefaultSpeed / 2))),
				asteroidSize: 3);
		}

		private void AddAsteroid(int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				Asteroid a = CreateAsteroid();
				a.AsteroidDestroyed += OnAsteroidDestroyed;
				a.AsteroidMissed += OnAsteroidMissed;
				_asteroids.Add(a);
			}
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
			double weight = 0.0;
			for (int i = 0; i < _asteroids.Count; i++)
			{
				_asteroids[i].Update(fps);
				weight += _asteroids[i].Size;
				if (_asteroids[i].ToRemove)
				{
					_asteroids[i].Remove();
					_asteroids.Remove(_asteroids[i]);
				}
			}
			// adding asteroids
			//TODO: rework
			double minutes = (DateTime.Now - _initTime).TotalSeconds / 60.0;
			double chance = CommonChance + minutes * ChanceMinuteStep;
			chance = chance > ChanceLimit ? ChanceLimit : chance;
			chance /= fps;
			chance = weight < 3 ? 1.0 : chance;
			//Console.WriteLine(chance);
			if (_rand.NextDouble() <= chance)
				AddAsteroid();
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
				AddScore = null;
				Initialized = false;
			}
		}

		public void CollisionTick(Player player)
		{
			List<Asteroid> toAdd = new List<Asteroid>();
			foreach (var ast in _asteroids)
			{
				foreach (var mis in player.Missiles)
				{
					if (ast.MissileCollides(mis))
					{
						List<Asteroid> newAst = ast.Destroy();
						if (newAst.Count > 0)
						{
							foreach (var a in newAst)
								a.Draw();
							toAdd.AddRange(newAst);
						}
						mis.ToRemove = true;
						mis.Remove();
					}
				}
				if (ast.PlayerCollides(player))
				{
					player.HitByAsteroid(ast);
				}
			}
			if (toAdd.Count > 0)
			{
				_asteroids.AddRange(toAdd);
				foreach (var a in toAdd)
				{
					a.AsteroidDestroyed += OnAsteroidDestroyed;
					a.AsteroidMissed += OnAsteroidMissed;
				}
			}
		}

		public void OnAsteroidMissed(object sender, EventArgs e)
		{
			AddScore?.Invoke(this, new AddScoreEventArgs(-(sender as Asteroid).MissPenalty));
		}

		public void OnAsteroidDestroyed(object sender, EventArgs e)
		{
			AddScore?.Invoke(this, new AddScoreEventArgs((sender as Asteroid).DestroyBounty));
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

	//public class AsteroidEventArgs : EventArgs
	//{
	//	public Asteroid Asteroid { get; private set; }

	//	public AsteroidEventArgs(Asteroid asteroid)
	//	{
	//		Asteroid = asteroid;
	//	}
	//}
		
}
