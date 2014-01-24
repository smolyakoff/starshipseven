using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using StarshipSeven.GameEntities.Events;
using StarshipSeven.GameEntities.Exceptions;

namespace StarshipSeven.GameEntities
{
    public abstract class Fleet : BaseGameEntity<Guid>, IFleet
    {
        private int _ships;
        private Player _owner;

        public Fleet(int shipsCount, Player owner) : base(Guid.NewGuid())
        {
            Ships = shipsCount;
            SetProperty<Player>(ref _owner, owner, "Owner");
        }

        #region IFleet Members

        public abstract double KillPercentage { get; }

        public void Attack(IFleet enemyFleet)
        {
            if (!(enemyFleet is Fleet))
                throw new TypeMismatchException(typeof(Fleet), enemyFleet);
            Fleet enemy = (Fleet)enemyFleet;
            if (!enemy.IsDead)
            {
                (Owner.Statistics as GameStatistics).EnemyShipsDestroyed += 1;
                enemy.SetShipsCount(enemy.Ships - 1, this);
            }
        }

        public bool IsDead
        {
            get { return Ships == 0; }
        }

        public int Ships
        {
            get { return _ships; }
            set { SetShipsCount(value, null); }
        }

        #endregion

        internal Player Owner
        {
            get { return _owner; }
        }

        internal event EventHandler<FleetDeadEventArgs> Dead;

        protected void OnDead(Fleet killer)
        {
            if (Dead != null)
                Dead(this, new FleetDeadEventArgs(this, killer));
        }

        internal void SetShipsCount(int count, Fleet possibleKiller)
        {
            int value = (count < 0) ? 0 : count;
            SetProperty<int>(ref _ships, value, "ShipsCount");
            if (value == 0)
                OnDead(possibleKiller);
        }
    }
}
