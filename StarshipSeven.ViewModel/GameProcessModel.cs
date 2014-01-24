using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using StarshipSeven.ViewModel.Wrappers;
using System.ComponentModel;
using StarshipSeven.ViewModel.EventsModel;
using StarshipSeven.GameInterfaces.Events;

namespace StarshipSeven.ViewModel
{
    public class GameProcessModel : BaseViewModel
    {
        private MapTile _mapTile;
        private IGame _game;
        private MapWrapper _mapWrapper;
        private int _shipsToLaunch = 1;
        private List<IGameEvent> _gameEvents = new List<IGameEvent>();

        public GameProcessModel(IGame game)
        {
            _game = game;
            MapWrapper = new MapWrapper(_game.Map);
            MapWrapper.PropertyChanged += OnSourceDestinationSelectionChanged;
            Game.BeforeTurnProcess += new EventHandler((o, e) => _gameEvents.Clear());
            Game.PropertyChanged += OnGameChanged;
            Game.PlanetConquered += new EventHandler<PlanetChangedEventArgs>((o, e) => _gameEvents.Add(new PlanetConqueredEvent(e)));
            Game.PlanetDefended += new EventHandler<PlanetChangedEventArgs>((o, e) => _gameEvents.Add(new PlanetDefendedEvent(e)));
            Game.PlayerDead += new EventHandler<PlayerChangedEventArgs>((o, e) => _gameEvents.Add(new PlayerDeadEvent(e)));
            Game.ReinforcementsArrived += new EventHandler<ReinforcementsArrivedEventArgs>((o, e) => _gameEvents.Add(new ReinforcementsArrivedEvent(e)));
        }

        private void OnGameChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged("MaxShips");
            if (args.PropertyName == "CurrentPlayer")
            {
                OnPropertyChanged("Game");
                OnPropertyChanged("ControlEnabled");
                OnPropertyChanged("Events");
            }
        }

        private void OnSourceDestinationSelectionChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CurrentSourcePlanet" || args.PropertyName == "CurrentDestinationPlanet")
            {
                OnPropertyChanged("IsReadyToLaunch");
                OnPropertyChanged("FleetArrivalTurn");
                OnPropertyChanged("MaxShips");
                if (SelectedTile.IsSourcePlanet)
                    ShipsToLaunch = SelectedTile.Planet.Ships;
            }
        }

        public MapTile SelectedTile
        {
            get { return _mapTile; }
            set 
            { 
                SetProperty<MapTile>(ref _mapTile, value, "SelectedTile");
                OnPropertyChanged("CurrentPlayerOwnsSelectedPlanet");
            }
        }

        public bool CurrentPlayerOwnsSelectedPlanet
        {
            get 
            {
                if (SelectedTile.IsEmpty)
                    return false;
                return SelectedTile.Planet.Owner == _game.CurrentPlayer; 
            }
        }

        public bool ControlEnabled
        {
            get { return !(_game.CurrentPlayer is IComputerPlayer); }
        }

        public int FleetArrivalTurn
        {
            get
            {
                if (!IsReadyToLaunch)
                    return 0;
                return _game.CurrentTurn + MapWrapper.CurrentSourcePlanet.Planet.GetDistanceTo(MapWrapper.CurrentDestinationPlanet.Planet);
            }
        }

        public int ShipsToLaunch
        {
            get { return _shipsToLaunch; }
            set { SetProperty<int>(ref _shipsToLaunch, value, "ShipsToLaunch"); }
        }

        public bool IsReadyToLaunch
        {
            get { return MapWrapper.CurrentSourcePlanet != null && MapWrapper.CurrentDestinationPlanet != null; }
        }

        public int MaxShips
        {
            get { return (MapWrapper.CurrentSourcePlanet == null) ? 0 : MapWrapper.CurrentSourcePlanet.Planet.Ships; }
        }

        public IGame Game
        {
            get { return _game; }
        }

        public MapWrapper MapWrapper
        {
            get { return _mapWrapper; }
            set { SetProperty<MapWrapper>(ref _mapWrapper, value, "MapWrapper"); }
        }

        public IEnumerable<IGameEvent> Events
        {
            get { return _gameEvents.OrderByDescending(x => x, new EventComparer(_game.CurrentPlayer)); }
        }

    }
}
