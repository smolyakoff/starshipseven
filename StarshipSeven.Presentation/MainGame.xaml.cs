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
using StarshipSeven.GameInterfaces;
using StarshipSeven.ViewModel.Wrappers;
using StarshipSeven.GameInterfaces.Events;

namespace StarshipSeven.Presentation
{
	public partial class MainGame
	{
        private GameProcessModel _gameProcess;
        private GameOverEventArgs _gameOverEventArgs;

		public MainGame(IGame game)
		{
			this.InitializeComponent();

            _gameProcess = new GameProcessModel(game);
            _gameProcess.Game.GameOver += OnGameOver;
            _gameProcess.Game.Start();
            DataContext = _gameProcess;
            mapListBox.SelectedIndex = 0;
            

			// Insert code required on object creation below this point.
		}

        private void DoGameOver(GameOverEventArgs args)
        {
            NavigationService.Navigate(new Statistics(args));
        }

        private void OnGameOver(object sender, GameOverEventArgs args)
        {
            if (this.NavigationService == null)
                _gameOverEventArgs = args;
            else
                DoGameOver(args);
                
        }

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			_gameProcess.Game.CurrentPlayer.EndTurn();
		}

		private void mapListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            if (_gameProcess.SelectedTile == null || _gameProcess.Game.CurrentPlayer is IComputerPlayer)
                return;
            MapTile selectedTile = _gameProcess.SelectedTile;
            if (selectedTile.HasPlanet && selectedTile.Planet.Owner == _gameProcess.Game.CurrentPlayer && selectedTile.Planet.Ships > 0)
                _gameProcess.MapWrapper.CurrentSourcePlanet = selectedTile;
		}

        private void mapListBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_gameProcess.SelectedTile == null || _gameProcess.Game.CurrentPlayer is IComputerPlayer)
                return;
            MapTile selectedTile = _gameProcess.SelectedTile;
            if (selectedTile.HasPlanet)
                _gameProcess.MapWrapper.CurrentDestinationPlanet = selectedTile;
        }

        private void mapListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _gameProcess.SelectedTile = (MapTile) mapListBox.SelectedValue;
        }

        private void setSourceButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	mapListBox_MouseDoubleClick(this, null);
        }

        private void setDestinationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	mapListBox_MouseRightButtonUp(this, null);
        }

        private void bigRedButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	IPlanet source = _gameProcess.MapWrapper.CurrentSourcePlanet.Planet;
			IPlanet destination = _gameProcess.MapWrapper.CurrentDestinationPlanet.Planet;
			int ships = _gameProcess.ShipsToLaunch;
			_gameProcess.Game.CurrentPlayer.LaunchFleet(ships, source, destination);
			_gameProcess.MapWrapper.CurrentSourcePlanet = null;
			_gameProcess.MapWrapper.CurrentDestinationPlanet = null;
        }

        private void cancelFleetButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	IAttackFleet fleet = (IAttackFleet) orderedFleetsListBox.SelectedValue;
			_gameProcess.Game.CurrentPlayer.CancelLaunch(fleet);
        }

        private void endTurnButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	_gameProcess.Game.CurrentPlayer.EndTurn();
        }

        private void MainGamePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_gameOverEventArgs != null)
                DoGameOver(_gameOverEventArgs);
        }

        private void menuButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new GameMenu(_gameProcess.Game));
        }

	}
}