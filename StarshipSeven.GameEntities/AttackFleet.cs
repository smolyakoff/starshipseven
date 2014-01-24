using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.GameEntities
{
    public class AttackFleet : Fleet, IAttackFleet
    {
        private Planet _sourcePlanet;
        private Planet _destinationPlanet;
        private int? _arrivalTurn;

        public AttackFleet(Player owner, int shipsCount, Planet sourcePlanet, Planet destinationPlanet)
            :base(shipsCount, owner)
        {
            SetProperty<Planet>(ref _sourcePlanet, sourcePlanet, "SourcePlanet");
            SetProperty<Planet>(ref _destinationPlanet, destinationPlanet, "DestinationPlanet");
        }

        #region IAttackFleet Members

        public IPlanet SourcePlanet
        {
            get { return _sourcePlanet; }
        }

        public IPlanet DestinationPlanet
        {
            get { return _destinationPlanet; }
        }

        public int? ArrivalTurn
        {
            get { return _arrivalTurn; }
            set { SetProperty<Nullable<int>>(ref _arrivalTurn, value, "ArrivalTurn"); }
        }

        #endregion

        public override double KillPercentage
        {
            get { return SourcePlanet.KillPercentage; }
        }
        
    }
}
