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

	class Game
	{
		public Background Background { get; private set; }
		private Player _player;
		private List<Missile> _missiles;
		private int _lastShot = 0;
		private double _shotsPerSec = 2.5;

		//public GameState GameState { get; private set; }
		public bool Initialized { get; private set; } = false;

		private Grid _gameGrid;
		private DispatcherTimer _timer;
		public int Fps { get; set; } = 30;

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
				_missiles = new List<Missile>();
				_player.Draw();
				Initialized = true;
			}
		}

		public void Stop()
		{
			if (Initialized)
			{
				_player.Remove();
				_player = null;
				for (int i = 0; i < _missiles.Count; i++)
				{
					_missiles[i].Remove();
				}
				_missiles = null;

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
				_player.Update(Fps);
				_player.Draw();

				for (int i = 0; i < _missiles.Count; i++)
				{
					_missiles[i].Update(Fps);
					_missiles[i].Draw();
					if (_missiles[i].ToRemove)
					{
						_missiles[i].Remove();
						_missiles.Remove(_missiles[i]);
					}
				}

				if (_lastShot < Fps / _shotsPerSec)
					_lastShot++;
			}
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
				CreateMissile();
			}
		}

		private void CreateMissile()
		{
			if (_lastShot >= Fps / _shotsPerSec)
			{
				Position p = _player.Pos;
				p.Y += _player.Height / 2;
				Missile m = new Missile(_gameGrid, p, new Vector(Missile.Acceleration, 0) + _player.Vec);
				m.Draw();
				_missiles.Add(m);
				_lastShot = 0;
			}
		}
	}
}
