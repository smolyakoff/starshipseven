using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.DataInterfaces.Managers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using StarshipSeven.GameInterfaces;
using StarshipSeven.DataInterfaces.SavegameEntites;
using StarshipSeven.ViewModel.Wrappers;
using StarshipSeven.GameInterfaces.Strategy;

namespace StarshipSeven.ViewModel
{
    public class GameMenuModel : BaseViewModel
    {
        private IGame _loadedGame;
        private ISavegameManager _savegameManager;
        private IUnityContainer _container = new UnityContainer().LoadConfiguration();
        private IGame _currentGame;
        private SavegameInfo _selectedSavegame;
        private IBattleHandler _battleHandler;
        private string _savegameName;
        private MapWrapper _mapWrapper;

        public GameMenuModel(IGame currentGame)
        {
            SavegameName = "Game_" + DateTime.Now.ToString("MMddyyyy_hhmm");
            _savegameManager = _container.Resolve<ISavegameManager>();
            GameStateManager = _container.Resolve<IGameStateManager>();
            _battleHandler = _container.Resolve<IBattleHandler>();
            GameStateManager.StrategyManager = _container.Resolve<IStrategyManager>().LoadConfiguration();
            _currentGame = currentGame;
        }

        private void UpdateMapPreview()
        {
            _loadedGame = GameStateManager.LoadState(SelectedSavegame);
            _loadedGame.BattleHandler = _battleHandler;
            if (MapWrapper == null)
                MapWrapper = new MapWrapper(_loadedGame.Map);
            else
            {
                MapWrapper.Map = _loadedGame.Map;
                OnPropertyChanged("MapWrapper");
            }
        }

        public SavegameInfo SelectedSavegame
        {
            get { return _selectedSavegame; }
            set 
            { 
                SetProperty<SavegameInfo>(ref _selectedSavegame, value, "SelectedSavegame");
                OnPropertyChanged("SelectedSavegamePlanetsNumber");
                OnPropertyChanged("SelectedSavegamePlayersNumber");
                if (value != null)
                    UpdateMapPreview();
                else
                    MapWrapper = null;
            }
        }

        public bool CanSave
        {
            get { return _currentGame != null; }
        }

        public int SelectedSavegamePlanetsNumber
        {
            get
            {
                if (SelectedSavegame == null)
                    return 0;
                return SelectedSavegame.MapInfo.Planets.Count(); 
            }
        }

        public int SelectedSavegamePlayersNumber
        {
            get 
            {
                if (SelectedSavegame == null)
                    return 0;
                return SelectedSavegame.Players.Count(); 
            }
        }

        public IGame CurrentGame
        {
            get { return _currentGame; }
        }

        public IGame LoadedGame
        {
            get { return _loadedGame; }
        }

        public MapWrapper MapWrapper
        {
            private set { SetProperty<MapWrapper>(ref _mapWrapper, value, "MapWrapper"); }
            get { return _mapWrapper; }
        }

        public IEnumerable<SavegameInfo> Savegames
        {
            get { return _savegameManager.LoadAll(); }
        }

        public IGameStateManager GameStateManager { private set; get; }

        public ISavegameManager SavegameManager
        {
            get { return _savegameManager; }
        }

        public void UpdateSavegameList()
        {
            OnPropertyChanged("Savegames");
        }

        public string SavegameName
        {
            get { return _savegameName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    SetProperty<string>(ref _savegameName, value, "SavegameName");
            }
        }
        
    }
}
