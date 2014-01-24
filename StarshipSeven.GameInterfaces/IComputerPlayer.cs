using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces.Strategy;

namespace StarshipSeven.GameInterfaces
{
    public interface IComputerPlayer : IPlayer
    {
        IEnumerable<IPlanet> EnemyPlanets { get; }

        bool AutoEndTurnEnabled { get; set; }

        IComputerTurnStrategy Strategy { get; set; }
    }
}
