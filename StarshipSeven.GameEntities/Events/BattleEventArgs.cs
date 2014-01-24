using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Events
{
    public class BattleEventArgs : EventArgs
    {
        public BattleEventArgs(AttackFleet attackFleet, DefenseFleet defenseFleet)
        {
            AttackFleet = attackFleet;
            DefenseFleet = defenseFleet;
        }

        public AttackFleet AttackFleet { private set; get; }

        public DefenseFleet DefenseFleet { private set; get; }
    }
}
