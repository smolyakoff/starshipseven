using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StarshipSeven.GameInterfaces.Events;

namespace StarshipSeven.Presentation
{
	public partial class Statistics
	{
        private GameOverEventArgs _gameOverInfo;

		public Statistics(GameOverEventArgs args)
		{
			this.InitializeComponent();
            _gameOverInfo = args;
            DataContext = _gameOverInfo;

			// Insert code required on object creation below this point.
		}

		private void newGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			NavigationService.Navigate(new GameCreator());
		}

		private void exitButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Window.GetWindow(this).Close();
		}
	}
}