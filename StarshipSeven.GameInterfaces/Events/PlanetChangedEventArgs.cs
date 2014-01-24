using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameInterfaces.Events
{
    public class PlanetChangedEventArgs : EventArgs
    {
        public IPlanet ChangedPlanet { private set; get; }

        public IPlayer AttackingPlayer { get; set; }

        public IPlayer DefendingPlayer { get; set; }

        public PlanetChangedEventArgs(IPlanet changedPlanet, IPlayer attackingPlayer, IPlayer defendingPlayer)
        {
            ChangedPlanet = changedPlanet;
            AttackingPlayer = attackingPlayer;
            DefendingPlayer = defendingPlayer;
        }
    }
}
