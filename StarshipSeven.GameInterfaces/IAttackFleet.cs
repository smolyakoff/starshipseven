using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces
{
    public interface IAttackFleet : IFleet
    {
        IPlanet SourcePlanet { get; }

        IPlanet DestinationPlanet { get; }

        int? ArrivalTurn { get; }
    }
}
