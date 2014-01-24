using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;
using System.Windows.Media;
using StarshipSeven.GameInterfaces;
using System.ComponentModel;

namespace StarshipSeven.ViewModel.Wrappers
{
    public class PlayerWrapper : BaseViewModel
    {
        private GameConstructorModel _owner;
        private string _name;
        private Color _color;
        private PlayerType _type;
        private IStrategyInfo _strategyInfo;
        private bool _autoEndTurn;
        private IPlayer _watchablePlayer;

        public PlayerWrapper(GameConstructorModel owner)
        {
            _owner = owner;
            PropertyChanged += OnWrapperPropertyChanged;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    SetProperty<string>(ref _name, value, "Name");
            }
        }

        public bool AutoEndTurnEnabled
        {
            get { return _autoEndTurn; }
            set { SetProperty<bool>(ref _autoEndTurn, value, "AutoEndTurnEnabled"); }
        }

        public Color Color
        {
            get { return _color; }
            set { SetProperty<Color>(ref _color, value, "Color"); }
        }

        public PlayerType Type
        {
            get { return _type; }
            set 
            {
                bool changed = _type != value;
                SetProperty<PlayerType>(ref _type, value, "Type");
                if (changed)
                    _owner.Synchronize();
                OnPropertyChanged("IsComputer");
            }
        }

        public IEnumerable<PlayerType> PlayerTypes
        {
            get { return Enum.GetValues(typeof(PlayerType)).Cast<PlayerType>(); }
        }

        public bool IsComputer
        {
            get { return (Type == PlayerType.Computer); }
        }

        public IEnumerable<IStrategyInfo> AvailableStrategies
        {
            get { return _owner.AvailableStrategies; }
        }

        public IStrategyInfo StrategyInfo
        {
            get { return _strategyInfo; }
            set { SetProperty<IStrategyInfo>(ref _strategyInfo, value, "Strategy"); }
        }

        private void OnWrapperPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (_watchablePlayer == null)
                return;
            _watchablePlayer.Name = this.Name;
            _watchablePlayer.Color = this.Color;
            if (_watchablePlayer is IComputerPlayer)
            {
                IComputerPlayer player = _watchablePlayer as IComputerPlayer;
                player.AutoEndTurnEnabled = this.AutoEndTurnEnabled;
                player.Strategy = this.StrategyInfo.Strategy;
            }     
        }

        internal void Watch(IPlayer player)
        {
            _watchablePlayer = player;
        }

    }
}
