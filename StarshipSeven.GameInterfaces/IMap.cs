using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces
{
    public interface IMap : IGameEntity, IEnumerable<IPlanet>
    {
        int Width { get; }

        int Height { get; }

        IPlanet GetPlanetOrNull(int x, int y);
    }
}
