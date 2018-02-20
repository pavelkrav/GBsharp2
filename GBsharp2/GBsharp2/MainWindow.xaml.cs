using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GBsharp2.Primitives;
using GBsharp2.BaseObjects;
using GBsharp2.GameEntities;

namespace GBsharp2
{
	public partial class MainWindow : Window
	{
		private Game _game;

		public MainWindow()
		{
			InitializeComponent();

			_game = new Game(BackgroundGrid, GameGrid);
			_game.StartAnimation();
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			double w = this.Width;
			double h = this.Height;

			MainGrid.Width = w;
			MainGrid.Height = h;

			BackgroundGrid.Width = w;
			BackgroundGrid.Height = h;

			GameGrid.Width = w;
			GameGrid.Height = h;

			tblControls.Text = "Move : ← ↑ → ↓\nFire : Spacebar\nPause : P\nUnpause : U\nMain menu : Esc";
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.P)
			{
				if (_game.Initialized)
					_game.StopAnimation();
			}
			if (e.Key == Key.U)
			{
				if (_game.Initialized)
					_game.StartAnimation();
			}
			if (e.Key == Key.Escape)
			{
				if (_game.Initialized)
				{
					_game.StartAnimation();
					_game.Stop();
					MainMenuGrid.Visibility = Visibility.Visible;
				}
			}
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			_game.Start();
			MainMenuGrid.Visibility = Visibility.Hidden;
		}

		private void btnControls_Click(object sender, RoutedEventArgs e)
		{
			if (tblControls.Visibility == Visibility.Hidden)
				tblControls.Visibility = Visibility.Visible;
			else tblControls.Visibility = Visibility.Hidden;
		}

		private void btnLeaderboard_Click(object sender, RoutedEventArgs e)
		{
			if (tblLeaderboard.Visibility == Visibility.Hidden)
				tblLeaderboard.Visibility = Visibility.Visible;
			else tblLeaderboard.Visibility = Visibility.Hidden;
		}
	}

}
