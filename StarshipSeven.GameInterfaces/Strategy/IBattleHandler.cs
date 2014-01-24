using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Strategy
{
    public interface IBattleHandler
    {
        void HandleBattle(IAttackFleet attackFleet, IDefenseFleet defenseFleet);
    }
}
