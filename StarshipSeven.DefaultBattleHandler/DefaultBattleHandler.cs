using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.DefaultBattleHandler
{
    public class DefaultBattleHandler : IBattleHandler
    {
        private Random _random = new Random();

        public void HandleBattle(IAttackFleet attackFleet, IDefenseFleet defenseFleet)
        {
            while (!attackFleet.IsDead && !defenseFleet.IsDead)
            {
                double attackRoll = _random.NextDouble();
                double defenseRoll = _random.NextDouble();

                if (attackFleet.KillPercentage == 0 && defenseFleet.KillPercentage == 0)
                    if (attackRoll > defenseRoll)
                        attackFleet.Attack(defenseFleet);
                    else
                        defenseFleet.Attack(attackFleet);

                if (defenseRoll < defenseFleet.KillPercentage && !attackFleet.IsDead)
                    defenseFleet.Attack(attackFleet);

                if (attackRoll < attackFleet.KillPercentage && !defenseFleet.IsDead)
                    attackFleet.Attack(defenseFleet);
            }
        }
    }
}
