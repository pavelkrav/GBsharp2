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

namespace GBsharp2.GameEntities
{
	class Game
	{
		public Background Background { get; private set; }
		private Grid _gameGrid;
		private DispatcherTimer _timer;
		public int Fps { get; set; } = 30;

		public Game(Grid backgroundGrid, Grid gameGrid)
		{
			Background = new Background(backgroundGrid);
			_gameGrid = gameGrid;
			_timer = new DispatcherTimer();
			_timer.Tick += new EventHandler(Tick);
			_timer.Interval = TimeSpan.FromMilliseconds(1000.0 / Fps);
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
		}
	}
}
