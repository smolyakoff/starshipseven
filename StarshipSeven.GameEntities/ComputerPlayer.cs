using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameInterfaces.Strategy;
using System.Windows.Media;

namespace StarshipSeven.GameEntities
{
    public class ComputerPlayer : Player, IComputerPlayer
    {
        private IComputerTurnStrategy _strategy;
        private bool _autoEndTurnEnabled;

        public ComputerPlayer(string name, Color color, IComputerTurnStrategy strategy, bool autoEndTurn)
            :base(name, color)
        {
            Strategy = strategy;
            AutoEndTurnEnabled = autoEndTurn;
        }

        public ComputerPlayer(Guid id)
            : base(id)
        {
        }

        public virtual void Think()
        {
            Strategy.MakeTurn(this);
            if (AutoEndTurnEnabled)
                EndTurn();
        }

        public IEnumerable<IPlanet> EnemyPlanets
        {
            get { return (Game.Map as Map).GetEnemyPlanetsForPlayer(this); }
        }

        public bool AutoEndTurnEnabled
        {
            get { return _autoEndTurnEnabled; }
            set { SetProperty<bool>(ref _autoEndTurnEnabled, value, "AutoEndTurnEnabled"); }
        }

        public IComputerTurnStrategy Strategy
        {
            get { return _strategy; }
            set { SetProperty<IComputerTurnStrategy>(ref _strategy, value, "Strategy"); }
        }
    }
}
