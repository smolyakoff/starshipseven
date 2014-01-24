using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StarshipSeven.GameInterfaces.Factories
{
    public abstract class BaseGameFactory : IGameFactory
    {
        private int _mapWidth = 5;
        private int _mapHeight = 5;
        private int _neutralPlanetsCount = 10;
        private IBattleHandler _battleHandler;
        private ObservableCollection<IPlayer> _players;
        private IPlayerFactory _playerFactory;

        public BaseGameFactory(IPlayerFactory playerFactory)
        {
            PlayerFactory = playerFactory;
            Players = new ObservableCollection<IPlayer>();
        }

        protected void SetWidth(int width)
        {
            if (width < 3)
                throw new ArgumentException("The minimal map width is 3.", "Width", null);
            SetProperty<int>(ref _mapWidth, width, "MapWidth");
            OnPropertyChanged("MaxNeutralPlanetsNumber");
            if (NeutralPlanetsNumber > MaxNeutralPlanetsNumber)
                NeutralPlanetsNumber = MaxNeutralPlanetsNumber;
            else
                GenerateMap();
        }

        protected void SetHeight(int height)
        {
            if (height < 3)
                throw new ArgumentException("The minimal map height is 3.", "Height", null);
            SetProperty<int>(ref _mapHeight, height, "MapHeight");
            OnPropertyChanged("MaxNeutralPlanetsNumber");
            if (NeutralPlanetsNumber > MaxNeutralPlanetsNumber)
                NeutralPlanetsNumber = MaxNeutralPlanetsNumber;
            else
                GenerateMap();
        }

        public int MaxNeutralPlanetsNumber
        {
            get { return MapHeight * MapWidth - Players.Count; }
        }

        protected void SetNeutralPlanetsNumber(int number)
        {
            if (number < 0 || number > MaxNeutralPlanetsNumber)
                throw new ArgumentException(string.Format("The neutral planets number should be from 0 to {0}", MaxNeutralPlanetsNumber));
            SetProperty<int>(ref _neutralPlanetsCount, number, "NeutralPlanetsNumber");
            GenerateMap();
        }

        public bool IsGameReady
        {
            get { return Players.Count >= 2 && BattleHandler != null; }
        }

        public int MapWidth
        {
            get { return _mapWidth; }
            set { SetWidth(value); }
        }

        public int MapHeight
        {
            get { return _mapHeight; }
            set { SetHeight(value); }
        }

        public IBattleHandler BattleHandler
        {
            get { return _battleHandler; }
            set { SetProperty<IBattleHandler>(ref _battleHandler, value, "BattleHandler"); }
        }

        public int NeutralPlanetsNumber
        {
            get { return _neutralPlanetsCount; }
            set { SetNeutralPlanetsNumber(value); }
        }

        public abstract IMap CurrentMap
        {
            get;
        }

        private void OnPlayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            GenerateMap();
        }

        public ObservableCollection<IPlayer> Players
        {
            get { return _players; }
            set 
            {
                if (_players != null)
                    _players.CollectionChanged -= OnPlayersCollectionChanged;
                SetProperty<ObservableCollection<IPlayer>>(ref _players, value, "Players");
                _players.CollectionChanged += OnPlayersCollectionChanged;
                GenerateMap();
            }
        }

        public IPlayerFactory PlayerFactory
        {
            get { return _playerFactory; }
            set { SetProperty<IPlayerFactory>(ref _playerFactory, value, "PlayerFactory"); }
        }

        public virtual void GenerateMap()
        {
            OnPropertyChanged("IsGameReady");
            OnPropertyChanged("CurrentMap");
        }

        public abstract IGame Construct();

        protected void SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
