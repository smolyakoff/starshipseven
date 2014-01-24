using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using System.Collections.ObjectModel;
using StarshipSeven.GameEntities.Exceptions;
using StarshipSeven.GameEntities.Events;
using System.Windows.Media;

namespace StarshipSeven.GameEntities
{
    public class Player : BaseGameEntity<Guid>, IPlayer
    {
        private string _name;
        private Color _color;
        private GameStatistics _statistics;
        private Game _game;
        protected ObservableCollection<IAttackFleet> _fleetsToLaunch = new ObservableCollection<IAttackFleet>();

        public Player() : base(Guid.NewGuid())
        {
            _statistics = new GameStatistics(this);
        }

        public Player(string name, Color color) : base(Guid.NewGuid())
        {
            _statistics = new GameStatistics(this);
            Name = name;
            Color = color;
        }

        public Player(Guid id)
            : base(id)
        {
            _statistics = new GameStatistics(this);
        }

        #region IPlayer Members

        public string Name
        {
            get { return _name; }
            set { SetProperty<string>(ref _name, value, "Name"); }
        }

        public Color Color
        {
            get { return _color; }
            set { SetProperty<Color>(ref _color, value, "Color"); }
        }

        public IEnumerable<IPlanet> ConqueredPlanets
        {
            get { return (Game.Map as Map).GetPlanetsForPlayer(this); }
        }

        public IAttackFleet LaunchFleet(int shipsCount, IPlanet sourcePlanet, IPlanet destinationPlanet)
        {
            if (!(sourcePlanet is Planet && destinationPlanet is Planet))
                throw new TypeMismatchException(typeof(Planet), destinationPlanet);

            if (!(Game.CurrentPlayer as Player).Equals(this))
                throw new CheatingException(this);

            AttackFleet fleet = (sourcePlanet as Planet).ProduceAttackFleet(shipsCount, destinationPlanet as Planet, this);
            fleet.ArrivalTurn = Game.CurrentTurn + sourcePlanet.GetDistanceTo(destinationPlanet);

            _fleetsToLaunch.Add(fleet);

            return fleet;
        }

        public bool CancelLaunch(IAttackFleet fleet)
        {
            if (!(fleet is AttackFleet))
                throw new TypeMismatchException(typeof(AttackFleet), fleet);

            (fleet.SourcePlanet as Planet).Ships += fleet.Ships;

            return _fleetsToLaunch.Remove(fleet as AttackFleet);
        }

        public virtual void EndTurn()
        {
            OnEndTurn();
        }

        public IGameStatistics Statistics
        {
            get { return _statistics; }
        }

        #endregion

        internal Game Game
        {
            get { return _game; }
            set { SetProperty<Game>(ref _game, value, "Game"); }
        }

        internal ObservableCollection<IAttackFleet> FleetsToLaunch
        {
            get { return _fleetsToLaunch; }
        }

        internal event EventHandler<EndTurnEventArgs> EndTurnEvent;

        protected void OnEndTurn()
        {
            if (EndTurnEvent != null)
            {
                List<AttackFleet> orderedFleets = _fleetsToLaunch.Select(x => x as AttackFleet).ToList();
                _fleetsToLaunch.Clear();
                EndTurnEvent(this, new EndTurnEventArgs(orderedFleets));
            }
        }

    }
}
