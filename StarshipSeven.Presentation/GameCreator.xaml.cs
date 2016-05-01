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
using StarshipSeven.ViewModel;
using StarshipSeven.ViewModel.Wrappers;

namespace StarshipSeven.Presentation
{
	public partial class GameCreator
	{
        private GameConstructorModel _gameConstructorModel = new GameConstructorModel();

		public GameCreator()
		{
			this.InitializeComponent();
			
			Loaded += delegate
            {
                Window w = Window.GetWindow(this);
                w.MinHeight = this.MinHeight + 50;
                w.MinWidth = this.MinWidth + 20;
            };

            DataContext = _gameConstructorModel;

			// Insert code required on object creation below this point.
		}

		private void addPlayerButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			_gameConstructorModel.AddDefaultPlayer();
        }

		private void removePlayerButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			_gameConstructorModel.Players.Remove(playersListBox.SelectedItem as PlayerWrapper);
		}

		private void randomizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
		    _gameConstructorModel.GameFactory.GenerateMap();
		}

		private void playButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			NavigationService.Navigate(new MainGame(_gameConstructorModel.GameFactory.Construct()));
		}

		private void loadButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			NavigationService.Navigate(new GameMenu());
		}
	}
}