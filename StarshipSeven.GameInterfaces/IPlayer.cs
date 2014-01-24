using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace StarshipSeven.GameInterfaces
{
    public interface IPlayer : IGameEntity
    {
        string Name { get; set; }

        Color Color { get; set; }

        IEnumerable<IPlanet> ConqueredPlanets { get; }

        IAttackFleet LaunchFleet(int shipsCount, IPlanet sourcePlanet, IPlanet destinationPlanet);

        bool CancelLaunch(IAttackFleet fleet);

        void EndTurn();

        IGameStatistics Statistics { get; }
    }
}
