using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces
{
    public interface IFleet : IGameEntity
    {
        double KillPercentage { get; }

        void Attack(IFleet enemyFleet);

        int Ships { get; }

        bool IsDead { get; }
    }
}
