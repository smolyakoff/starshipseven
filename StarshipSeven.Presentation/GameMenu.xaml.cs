using System;
using System.Collections.Generic;
using System.Linq;
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

namespace StarshipSeven.Presentation
{
	public partial class GameMenu
	{
        private GameMenuModel _menuModel;

		public GameMenu(IGame currentGame = null)
		{
			this.InitializeComponent();
            _menuModel = new GameMenuModel(currentGame);
            if (_menuModel.Savegames.Any())
                savegameListBox.SelectedIndex = 0;
            if (currentGame != null)
                savegameNameTextBox.Focus();
            DataContext = _menuModel;

			// Insert code required on object creation below this point.
		}

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void saveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	var state = _menuModel.GameStateManager.SaveState(_menuModel.CurrentGame);
            state.Name = _menuModel.SavegameName;
            bool confirm = true;
            if (_menuModel.SavegameManager.Exists(state.Name))
            {
                string msg = string.Format("Do you really want to overwrite {0}?", state.Name);
                var result = MessageBox.Show(msg, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                confirm = result == MessageBoxResult.Yes;
            }
            if (confirm)
            {
                _menuModel.SavegameManager.Save(state);
                NavigationService.GoBack();
            }
            
        }

        private void loadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainGame(_menuModel.LoadedGame));
        }

        private void deleteButtom_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	_menuModel.SavegameManager.Delete(_menuModel.SelectedSavegame.Name);
            _menuModel.UpdateSavegameList();
            if (savegameListBox.HasItems)
                savegameListBox.SelectedIndex = 0;
        }

        private void newGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(new GameCreator());
        }

        private void savegameListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	if (_menuModel.SelectedSavegame != null)
				_menuModel.SavegameName = _menuModel.SelectedSavegame.Name;
        }
	}
}