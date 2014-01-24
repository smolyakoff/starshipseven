using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using StarshipSeven.ViewModel.Wrappers;
using StarshipSeven.GameInterfaces.Factories;
using System.Collections.Specialized;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using StarshipSeven.GameInterfaces.Strategy;
using System.Windows.Media;
using System.ComponentModel;
using StarshipSeven.GameInterfaces;
using StarshipSeven.DataInterfaces.Managers;

namespace StarshipSeven.ViewModel
{
    public class GameConstructorModel : BaseViewModel
    {
        private int _playerCounter;
        private IGameFactory _factory;
        private MapWrapper _mapWrapper;
        private IUnityContainer _container;
        private IStrategyManager _strategyManager;

        public GameConstructorModel()
        {
            Players = new ObservableCollection<PlayerWrapper>();
            Players.CollectionChanged += OnPlayersCollectionChanged;
            _container = new UnityContainer().LoadConfiguration();
            _factory = _container.Resolve<IGameFactory>();
            _factory.BattleHandler = _container.Resolve<IBattleHandler>();
            _factory.PropertyChanged += OnFactoryPropertyChanged;
            _strategyManager = _container.Resolve<IStrategyManager>().LoadConfiguration();
            MapWrapper = new MapWrapper(_factory.CurrentMap);
            AddDefaultPlayer();
        }

        public void Synchronize()
        {
            OnPlayersCollectionChanged(this, null);
        }

        private Color GetDefaultColor(int count)
        {
            int selector = count % 8;
            switch (selector)
            {
                case 0:
                    return Colors.Red;
                case 1:
                    return Colors.Blue;
                case 2:
                    return Colors.Yellow;
                case 3:
                    return Colors.Green;
                case 4:
                    return Colors.White;
                case 5:
                    return Colors.Violet;
                case 6:
                    return Colors.Black;
                case 7:
                    return Colors.Azure;
                default:
                    return Colors.Gray;
            }

        }

        public void AddDefaultPlayer()
        {
            _playerCounter++;
            PlayerWrapper player = new PlayerWrapper(this)
            {
                Name = string.Format("Player {0}", _playerCounter),
                Color = GetDefaultColor(_playerCounter - 1),
                Type = PlayerType.Human,
                AutoEndTurnEnabled = true,
                StrategyInfo = AvailableStrategies.FirstOrDefault()
            };
            Players.Add(player);
        }

        private void OnFactoryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals("CurrentMap"))
                MapWrapper.Map = GameFactory.CurrentMap;   
        }

        private void OnPlayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ObservableCollection<IPlayer> playersToAdd = new ObservableCollection<IPlayer>();
            foreach (PlayerWrapper playerInfo in Players)
            {
                if (playerInfo.Type == PlayerType.Human)
                {
                    var player = GameFactory.PlayerFactory.CreateHumanPlayer(playerInfo.Name, playerInfo.Color);
                    playerInfo.Watch(player);
                    playersToAdd.Add(player);
                }
                else
                {
                    var player = GameFactory.PlayerFactory.CreateComputerPlayer(playerInfo.Name, playerInfo.Color,
                        playerInfo.StrategyInfo.Strategy, playerInfo.AutoEndTurnEnabled);
                    playerInfo.Watch(player);
                    playersToAdd.Add(player);
                }
            }
            GameFactory.Players = playersToAdd;
            MapWrapper.Map = GameFactory.CurrentMap;
        }

        public MapWrapper MapWrapper
        {
            private set { SetProperty<MapWrapper>(ref _mapWrapper, value, "MapWrapper"); }
            get { return _mapWrapper; }
        }

        public IEnumerable<IStrategyInfo> AvailableStrategies
        {
            get { return _strategyManager.AvailableStrategies; }
        }

        public ObservableCollection<PlayerWrapper> Players { private set; get; }

        public IGameFactory GameFactory
        {
            get { return _factory; }
        }
    }
}
