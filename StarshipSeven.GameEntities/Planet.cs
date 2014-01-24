using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameEntities.Exceptions;
using StarshipSeven.GameEntities.Events;
using StarshipSeven.GameInterfaces.Events;

namespace StarshipSeven.GameEntities
{
    public class Planet : BaseGameEntity<Guid>, IPlanet, IComparable<Planet>
    {
        private string _name;
        private Position? _position;
        private double _killPercentage;
        private int _productionRate;
        private int _ships;
        private Player _owner;

        public Planet(Player owner) : base(Guid.NewGuid())
        {
            SetProperty<Player>(ref _owner, owner, "Player");
        }

        public Planet(string name) : base(Guid.NewGuid())
        {
            Name = name;
        }

        public Planet(Guid id)
            : base(id)
        {
        }

        #region IPlanet Members

        public string Name
        {
            get { return _name; }
            set { SetProperty<string>(ref _name, value, "Name"); }
        }

        public Position? Position
        {
            get { return _position; }
            set { SetProperty<Nullable<Position>>(ref _position, value, "Position"); }
        }

        public int GetDistanceTo(IPlanet planet)
        {
            if (Position == null || planet.Position == null)
                return int.MaxValue;
            int distance = Math.Abs(Position.Value.X - planet.Position.Value.X) + Math.Abs(Position.Value.Y - planet.Position.Value.Y);
            return distance;
        }

        public int ProductionRate
        {
            get { return _productionRate; }
            set { SetProperty<int>(ref _productionRate, value, "ProductionRate"); }
        }

        public int Ships
        {
            get { return _ships; }
            set { SetProperty<int>(ref _ships, value, "ShipsCount"); }
        }

        public double KillPercentage
        {
            get { return _killPercentage; }
            set { SetProperty<double>(ref _killPercentage, value, "KillPercentage"); }
        }

        public IPlayer Owner
        {
            get { return _owner; }
        }

        #endregion

        internal void SetOwner(Player owner)
        {
            SetProperty<Player>(ref _owner, owner, "Owner");
        }

        internal void Produce()
        {
            Ships += ProductionRate;
            (Owner.Statistics as GameStatistics).ShipsProduced += ProductionRate;
        }

        internal void MeetAttackFleet(AttackFleet fleet)
        {
            if (fleet.Owner.Equals(this.Owner))
                OnReinforcementsArrived(fleet.Ships);
            else
                OnBattle(fleet, ProduceDefenseFleet());

        }

        internal void SetupAttackFleet(AttackFleet fleet)
        {
            fleet.Dead += OnAttackDeath;
        }

        internal AttackFleet ProduceAttackFleet(int ships, Planet destinationPlanet, Player requestingPlayer)
        {
            if (!requestingPlayer.Equals(Owner as Player))
                throw new CheatingException(requestingPlayer);

            if (Ships < ships)
                throw new GameLogicException("Not enough ships to form a fleet!");

            Ships -= ships;
            AttackFleet fleet = new AttackFleet(requestingPlayer, ships, this, destinationPlanet);
            SetupAttackFleet(fleet);
            return fleet;
        }

        internal DefenseFleet ProduceDefenseFleet()
        {
            DefenseFleet defense = new DefenseFleet(this);
            defense.Dead += OnDefenseDeath;
            return defense;
        }

        protected void OnBattle(AttackFleet attack, DefenseFleet defense)
        {
            if (Battle != null)
                Battle(this, new BattleEventArgs(attack, defense));
        }

        private void OnAttackDeath(object sender, FleetDeadEventArgs args)
        {
            if (args.KillerFleet == null)
                throw new CheatingException();

            AttackFleet attack = (AttackFleet)args.DeadFleet;
            DefenseFleet defense = (DefenseFleet)args.KillerFleet;

            (attack.Owner.Statistics as GameStatistics).FleetsLost += 1;
            (defense.Owner.Statistics as GameStatistics).EnemyFleetsDestroyed += 1;

            defense.HomePlanet.Ships += defense.Ships;
            OnDefensed(attack.Owner, defense.Owner, defense.HomePlanet);

        }

        private void OnDefenseDeath(object sender, FleetDeadEventArgs args)
        {
            if (args.KillerFleet == null)
                throw new CheatingException();

            AttackFleet attack = (AttackFleet) args.KillerFleet;
            DefenseFleet defense = (DefenseFleet)args.DeadFleet;

            (attack.Owner.Statistics as GameStatistics).EnemyFleetsDestroyed += 1;
            (defense.Owner.Statistics as GameStatistics).FleetsLost += 1;

            SetOwner(attack.Owner);
            Ships += attack.Ships;
            OnConquered(attack.Owner, defense.Owner);
        }

        internal event EventHandler<PlanetChangedEventArgs> Conquered;

        internal event EventHandler<PlanetChangedEventArgs> Defensed;

        internal event EventHandler<BattleEventArgs> Battle;

        internal event EventHandler<ReinforcementsArrivedEventArgs> ReinforcementsArrived;

        protected void OnReinforcementsArrived(int ships)
        {
            Ships += ships;
            if (ReinforcementsArrived != null)
                ReinforcementsArrived(this, new ReinforcementsArrivedEventArgs(this, ships));
        }

        protected void OnConquered(IPlayer attackingPlayer, IPlayer defendingPlayer)
        {
            if (Conquered != null)
                Conquered(this, new PlanetChangedEventArgs(this, attackingPlayer, defendingPlayer));
        }

        protected void OnDefensed(IPlayer attackingPlayer, IPlayer defendingPlayer, IPlanet defensePlanet)
        {
            if (Defensed != null)
                Defensed(this, new PlanetChangedEventArgs(defensePlanet, attackingPlayer, defendingPlayer));
        }

        public int CompareTo(Planet other)
        {
            int ydelta = Position.Value.Y - other.Position.Value.Y;
            if (ydelta != 0)
                return ydelta;
            int xdelta = Position.Value.X - other.Position.Value.X;
            if (xdelta != 0)
                return xdelta;
            else
                return 0;
        }
    }
}
