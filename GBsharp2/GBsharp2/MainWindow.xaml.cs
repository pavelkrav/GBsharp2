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
		public MainWindow()
		{
			InitializeComponent();

			Game game = new Game(BackgroundGrid, GameGrid);
			game.Background.Init(50);
			game.Background.Draw();
			game.StartAnimation();
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			MainGrid.Width = this.Width;
			MainGrid.Height = this.Height;

			BackgroundGrid.Width = this.Width;
			BackgroundGrid.Height = this.Height;

			GameGrid.Width = this.Width;
			GameGrid.Height = this.Height;
		}
	}

}
