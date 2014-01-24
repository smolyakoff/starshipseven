using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.GameEntities
{
    public class DefenseFleet : Fleet, IDefenseFleet
    {
        private Planet _homePlanet;

        public DefenseFleet(Planet homePlanet)
            :base(homePlanet.Ships, homePlanet.Owner as Player)
        {
            HomePlanet = homePlanet;
            homePlanet.Ships = 0;
        }

        internal Planet HomePlanet
        {
            get { return _homePlanet; }
            set { SetProperty<Planet>(ref _homePlanet, value, "HomePlanet"); }
        }

        public override double KillPercentage
        {
            get { return _homePlanet.KillPercentage; }
        }
    }
}
