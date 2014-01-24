using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.GameEntities
{
    public class GameStatistics : BaseGameEntity<Guid>, IGameStatistics
    {
        private Player _player;
        private int _enemyShipsDestroyed;
        private int _enemyFleetsDestroyed;
        private int _shipsProduced;
        private int _fleetsLost;
        private int? _deathTurn;

        public GameStatistics(Player player) : base(Guid.NewGuid())
        {
            SetProperty<Player>(ref _player, player, "Player");
        }

        #region IGameStatistics Members

        public int EnemyShipsDestroyed
        {
            get { return _enemyShipsDestroyed; }
            set { SetProperty<int>(ref _enemyShipsDestroyed, value, "EnemyShipsDestroyed"); }
        }

        public int EnemyFleetsDestroyed
        {
            get { return _enemyFleetsDestroyed; }
            set { SetProperty<int>(ref _enemyFleetsDestroyed, value, "EnemyFleetsDestroyed"); }
        }

        public int FleetsLost
        {
            get { return _fleetsLost; }
            set { SetProperty<int>(ref _fleetsLost, value, "Losts"); }
        }

        public int ShipsProduced
        {
            get { return _shipsProduced; }
            set { SetProperty<int>(ref _shipsProduced, value, "ShipsProduced"); }
        }

       
        public int? DeathTurn
        {
            get { return _deathTurn; }
            set { SetProperty<Nullable<int>>(ref _deathTurn, value, "DeathTurn"); }
        }

        #endregion

        public Player Player
        {
            get { return _player; }
        }
    }
}
