﻿using System;
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
		private AsteroidField _asteroidField;

		//public GameState GameState { get; private set; }
		public bool Initialized { get; private set; } = false;

		private Grid _gameGrid;
		private DispatcherTimer _timer;
		public double Fps { get; set; } = 30;

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
				_player.Draw();
				//_asteroids = new List<Asteroid>();
				//_shotsPerSec *= _speed;
				Initialized = true;
			}
		}

		public void Stop()
		{
			if (Initialized)
			{
				_player.Remove();
				_player = null;

				//for (int i = 0; i < _asteroids.Count; i++)
				//{
				//	_asteroids[i].Remove();
				//}
				//_asteroids = null;

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
	}
}
