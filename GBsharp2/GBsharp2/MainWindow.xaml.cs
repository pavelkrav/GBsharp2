using System;
using System.Windows;
using System.Windows.Input;
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
				StopGame();
			}
		}

		private void StartGame()
		{
			_game.Start();
			_game.ScoreChanged += OnScoreChanged;
			_game.GameOver += OnGameOver;
			tbxScore.Text = "0";
			tbxScore.Visibility = Visibility.Visible;
			MainMenuGrid.Visibility = Visibility.Hidden;
		}

		private void StopGame()
		{
			if (_game.Initialized)
			{
				_game.StartAnimation();
				_game.Stop();
				MainMenuGrid.Visibility = Visibility.Visible;
				tbxScore.Visibility = Visibility.Hidden;
			}
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			StartGame();
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

		private void OnScoreChanged(object sender, EventArgs e)
		{
			tbxScore.Text = (sender as Game).Score.ToString("F0");
		}

		public void OnGameOver(object sender, EventArgs e)
		{
			StopGame();
		}
	}

}
