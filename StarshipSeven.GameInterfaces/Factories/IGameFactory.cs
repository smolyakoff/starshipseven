using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace StarshipSeven.GameInterfaces.Factories
{
    public interface IGameFactory : INotifyPropertyChanged
    {
        int MapWidth { get; set; }

        int MapHeight { get; set; }

        IBattleHandler BattleHandler { get; set; }

        ObservableCollection<IPlayer> Players { get; set; }

        bool IsGameReady { get; }

        int MaxNeutralPlanetsNumber { get; }

        int NeutralPlanetsNumber { get; set; }

        IPlayerFactory PlayerFactory { get; }

        IMap CurrentMap { get; }

        IGame Construct();
    }
}
