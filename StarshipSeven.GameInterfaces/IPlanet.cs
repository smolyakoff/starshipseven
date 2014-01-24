using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces
{
    public interface IPlanet : IGameEntity
    {
        string Name { set; get; }

        Position? Position { get; }

        int GetDistanceTo(IPlanet planet);

        int ProductionRate { get; }

        int Ships { get; }

        double KillPercentage { get; }

        IPlayer Owner { get; }
    }
}
