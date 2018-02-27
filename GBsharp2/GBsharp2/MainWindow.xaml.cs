using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GBsharp2.GameEntities;
using GBsharp2.Leaderboard;

namespace GBsharp2
{
	public partial class MainWindow : Window
	{
		private Game _game;
		private Lboard _lboard;

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

			_lboard = Lboard.Load();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			_lboard.Save();
			base.OnClosing(e);
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
			_game.PlayerHpChanged += OnPlayerHpChanged;
			tbxScore.Text = "Score: " + _game.Score.ToString("F0");
			tbxHp.Text = "HP: " + _game.HP.ToString("F0");
			tbxScore.Visibility = Visibility.Visible;
			tbxHp.Visibility = Visibility.Visible;
			MainMenuGrid.Visibility = Visibility.Hidden;
		}

		private void StopGame()
		{
			if (_game.Initialized)
			{
				double score = _game.Score;
				_game.StartAnimation();
				_game.Stop();
				LeaderboardGrid.Visibility = Visibility.Visible;
				tbxScore.Visibility = Visibility.Hidden;
				tbxHp.Visibility = Visibility.Hidden;
				lblScore.Content = score.ToString("F0");
				tblControls.Visibility = Visibility.Hidden;
				tblLeaderboard.Visibility = Visibility.Hidden;
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
			{
				tblLeaderboard.Visibility = Visibility.Visible;
				tblLeaderboard.Text = _lboard.GetLeaders();
			}
			else tblLeaderboard.Visibility = Visibility.Hidden;
		}

		private void OnScoreChanged(object sender, EventArgs e)
		{
			tbxScore.Text = "Score: " + (sender as Game).Score.ToString("F0");
		}

		private void OnPlayerHpChanged(object sender, EventArgs e)
		{
			tbxHp.Text = "HP: " + (sender as Game).HP.ToString("F0");
		}

		public void OnGameOver(object sender, EventArgs e)
		{
			StopGame();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			ExPlayer player = new ExPlayer(tbxName.Text, Convert.ToDouble(lblScore.Content));
			_lboard.Players.Add(player);
			MainMenuGrid.Visibility = Visibility.Visible;
			LeaderboardGrid.Visibility = Visibility.Hidden;
		}
	}

}
