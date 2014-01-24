using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using StarshipSeven.GameInterfaces.Strategy;
using StarshipSeven.GameInterfaces.Events;

namespace StarshipSeven.GameInterfaces
{
    public interface IGame : IGameEntity
    {
        IPlayer CurrentPlayer { get; }

        int CurrentTurn { get; }

        void Start();

        IMap Map { get; }

        IEnumerable<IAttackFleet> MovingFleetsForCurrentPlayer { get; }

        ObservableCollection<IAttackFleet> FleetsToLaunchForCurrentPlayer { get; }

        IBattleHandler BattleHandler { get; set; }

        event EventHandler BeforeTurnProcess;

        event EventHandler<PlanetChangedEventArgs> PlanetConquered;

        event EventHandler<PlanetChangedEventArgs> PlanetDefended;

        event EventHandler<ReinforcementsArrivedEventArgs> ReinforcementsArrived;

        event EventHandler<PlayerChangedEventArgs> PlayerDead;

        event EventHandler<PlayerChangedEventArgs> PlayerDisqualified;

        event EventHandler<GameOverEventArgs> GameOver;

    }
}
