using System;
using System.Threading;
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
using System.Windows.Input;

namespace GBsharp2.GameEntities
{
	//enum GameState { Void, Running, Paused }

	public class Game
	{
		public Background Background { get; private set; }
		private Player _player;
		private AsteroidField _asteroidField;

		//public GameState GameState { get; private set; }
		public bool Initialized { get; private set; } = false;

		private Grid _gameGrid;
		private DispatcherTimer _timer;
		public double Fps { get; set; } = 30;

		public double Score { get; private set; }
		public double HP { get { return _player.HP; } }

		public event EventHandler<EventArgs> ScoreChanged;
		public event EventHandler<EventArgs> PlayerHpChanged;
		public event EventHandler<EventArgs> GameOver;

		public Game(Grid backgroundGrid, Grid gameGrid)
		{
			Background = new Background(backgroundGrid);
			_gameGrid = gameGrid;

			Background.Init(35);
			Background.Draw();

			_timer = new DispatcherTimer();
			_timer.Tick += new EventHandler(Tick);
			_timer.Interval = TimeSpan.FromMilliseconds(1000.0 / Fps);
		}

		public void Start()
		{
			if (!Initialized)
			{
				_player = new Player(_gameGrid, new Position(40, _gameGrid.Height / 2), new Vector(0, 0));
				_player.Death += OnPlayerDeath;
				_player.HpChanged += OnPlayerHpChanged;
				_player.Draw();
				_asteroidField = new AsteroidField(_gameGrid);
				_asteroidField.AddScore += OnScoreAdd;
				_asteroidField.Init();
				_asteroidField.Draw();
				Score = 0;
				Initialized = true;
			}
		}

		public void Stop()
		{
			if (Initialized)
			{
				_player.Remove();
				_player = null;
				_asteroidField.Remove();
				_asteroidField = null;
				ScoreChanged = null;
				GameOver = null;
				PlayerHpChanged = null;
				Initialized = false;
			}
		}

		public void StartAnimation()
		{
			_timer.Start();
		}

		public void StopAnimation()
		{
			_timer.Stop();
		}

		private void Tick(object sender, EventArgs e)
		{
			Background.Update(Fps);
			Background.Draw();

			if (Initialized)
			{
				KeyboardTick();
				_player?.Update(Fps);
				_player?.Draw();
				_asteroidField?.Update(Fps);
				_asteroidField?.Draw();
				_asteroidField?.CollisionTick(_player);
			}
			//Console.WriteLine(BaseObject.TotalObjects);
		}

		private void KeyboardTick()
		{
			if (Keyboard.IsKeyDown(Key.Up))
			{
				_player.KeyUp(Fps);
			}
			else if (Keyboard.IsKeyDown(Key.Down))
			{
				_player.KeyDown(Fps);
			}
			if (Keyboard.IsKeyDown(Key.Right))
			{
				_player.KeyRight(Fps);
			}
			else if (Keyboard.IsKeyDown(Key.Left))
			{
				_player.KeyLeft(Fps);
			}

			if (Keyboard.IsKeyDown(Key.Space))
			{
				_player.CreateMissile(Fps);
			}
		}

		private void OnScoreAdd(object sender, AddScoreEventArgs e)
		{
			Score += e.Score;
			ScoreChanged?.Invoke(this, new EventArgs());
		}

		private void OnPlayerDeath(object sender, EventArgs e)
		{
			GameOver?.Invoke(this, new EventArgs());
		}

		private void OnPlayerHpChanged(object sender, EventArgs e)
		{
			PlayerHpChanged?.Invoke(this, new EventArgs());
		}
	}
}
